// dataService to store data about formatiing
var dataService = undefined;
const DELTA = 0.1;
const formattingParamsToLoad = "text, font, alignment, lineSpacing, style, leftIndent, rightIndent, listItemOrNullObject, uniqueLocalId";

// Enum for error types
const formattingErrorTypes = {
    INCORRECT_FONT_NAME: 'IncorrectFontName',
    INCORRECT_FONT_SIZE: 'IncorrectFontSize',
    INCORRECT_ALIGNMENT: 'IncorrectAlignment',
    INCORRECT_LINE_SPACING: 'IncorrectLineSpacing',
    INCORRECT_LEFT_INDENT: 'IncorrectLeftIndent',
    INCORRECT_RIGHT_INDENT: 'IncorrectRightIndent'
};

window.formattingConnector = {
    checkFormatting: async (start, data) => {
        console.log("retireved args: ", start, data);
        dataService = data;
        return await startFormattingScan(start);
    },

    correctFormatting: async (idToCorrect, errors) => {
        // we are assuiming that the paragraph is already selected
        console.log("Correcting paragraph errors " + errors);
        result = false;
        console.log("Correcting paragraph " + idToCorrect);
        await Word.run(async (context) => {
            var selection = context.document.getSelection();
            // Load the paragraph that contains the selection
            selection.paragraphs.load('uniqueLocalId, style, listItemOrNullObject');
            await context.sync();
            var paragraph = selection.paragraphs.items.find(para => para.uniqueLocalId === idToCorrect);
            if (paragraph === undefined) {
                console.log('Selection has changed, the id is not correct');
                result = false;
                return;
            }
            console.log('Paragraph = ', paragraph);
            console.log('Setting paragraph formatting', dataService);
            errors.forEach(error => {
                switch (error) {
                    case formattingErrorTypes.INCORRECT_FONT_NAME:
                        paragraph.font.name = dataService.fontName;
                        break;
                    case formattingErrorTypes.INCORRECT_FONT_SIZE:
                        paragraph.font.size = GetExpectedFontSize(paragraph);
                        break;
                    case formattingErrorTypes.INCORRECT_ALIGNMENT:
                        paragraph.alignment = dataService.alligment;
                        break;
                    case formattingErrorTypes.INCORRECT_LINE_SPACING:
                        paragraph.lineSpacing = dataService.lineSpacingInPoints;
                        break;
                    case formattingErrorTypes.INCORRECT_LEFT_INDENT:
                        paragraph.leftIndent = dataService.leftIndentInPoints;
                        break;
                    case formattingErrorTypes.INCORRECT_RIGHT_INDENT:
                        paragraph.rightIndent = dataService.rightIndentInPoints;
                        break;
                    default:
                        break;
                }
            });
            console.log('Setting formatting done, selecting paragraph again');
            paragraph.select();
            await context.sync();
            result = true;
        });
        return result;
    }
}

function GetExpectedFontSize(paragraph) {
    if (paragraph.style.includes("Heading") || paragraph.style.includes("Nadpis")) {
        if (!paragraph.listItemOrNullObject.isNullObject) {
            // this is part of the list
            var level = paragraph.listItemOrNullObject.level;
            switch (level) {
                case 0:
                    return dataService.heading1FontSize;
                case 1:
                    return dataService.heading2FontSize;
                case 2:
                    return dataService.heading3FontSize;
                case 3:
                    return dataService.heading4FontSize;
                default:
                    return dataService.fontSize;
            }
        }
        return dataService.heading1FontSize;
    }
    return dataService.fontSize;
}

async function startFormattingScan(start) {
    // list of found errors
    const errors = [];
    var paraId = undefined;
    var isErrorr = false;
    return Word.run(async (context) => {
        var paragraph = null;
        if (start) {
            console.log("Getting first paragraph");
            paragraph = context.document.body.paragraphs.getFirst();
            paragraph.load(formattingParamsToLoad);
            await context.sync();
        }
        else {
            console.log("Getting selected paragraph");
            var selection = context.document.getSelection();
            // Load the paragraph that contains the selection
            selection.paragraphs.load(formattingParamsToLoad);
            await context.sync();
            if (selection.paragraphs.items.length > 0) {
                paragraph = selection.paragraphs.items[0];
            }
            else {
                // TODO - check if the paragraph stayed selected - if it is not null
                console.log("Selection has changed, I cannot find any paragraph");
            }
        }
        while (paragraph !== null && !paragraph.isNullObject) {
            console.log(dataService.ignoredParagraphs);
            console.log("Checking: ", paragraph.text, paragraph.uniqueLocalId, dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId));
            if (!dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId) && paragraph.text !== "") {

                const styleChecks = [
                    { condition: paragraph.font.name !== dataService.fontName, errorType: formattingErrorTypes.INCORRECT_FONT_NAME },
                    { condition: paragraph.font.size !== GetExpectedFontSize(paragraph), errorType: formattingErrorTypes.INCORRECT_FONT_SIZE },
                    { condition: paragraph.alignment !== dataService.alligment, errorType: formattingErrorTypes.INCORRECT_ALIGNMENT },
                    { condition: paragraph.lineSpacing !== dataService.lineSpacingInPoints, errorType: formattingErrorTypes.INCORRECT_LINE_SPACING },
                    { condition: paragraph.leftIndent > dataService.leftIndentInPoints + DELTA || paragraph.leftIndent < dataService.leftIndentInPoints - DELTA, errorType: formattingErrorTypes.INCORRECT_LEFT_INDENT },
                    { condition: paragraph.rightIndent > dataService.rightIndentInPoints + DELTA || paragraph.rightIndent < dataService.rightIndentInPoints - DELTA, errorType: formattingErrorTypes.INCORRECT_RIGHT_INDENT },
                ];
                // Iterate over checks
                styleChecks.forEach(check => {
                    if (check.condition) {
                        errors.push(check.errorType);
                    }
                });
                if (errors.length > 0) {
                    console.log("This paragraph is wrong: ", paragraph.text, paragraph.style, paragraph.font.size, paragraph.font.name, paragraph.alignment, paragraph.lineSpacing);
                    paraId = paragraph.uniqueLocalId;
                    isErrorr = true;
                    paragraph.select();
                    //await selectParagraph(CURRENT_PARAGRAPG_INDEX);
                    console.log(errors);
                    break;
                }
            }
            console.log("Moving to next paragraph");
            paragraph = paragraph.getNextOrNullObject();
            paragraph.load(formattingParamsToLoad);
            await context.sync();
            console.log("Next paragraph is null?", paragraph.isNullObject);
        }
        const FormattingReturnValue = {
            FoundError: isErrorr,
            ParagraphId: paraId,
            ErrorTypes: errors
        };
        console.log(JSON.stringify(FormattingReturnValue));
        return FormattingReturnValue;
    });
}

