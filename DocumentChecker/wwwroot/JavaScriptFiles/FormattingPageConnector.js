// dataService to store data about formatiing
var dataService = undefined;
const DELTA = 0.1;
const formattingParamsToLoad = "text, font, alignment, lineSpacing, style, leftIndent, rightIndent, uniqueLocalId";

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
        if (start) {
            // if we are starting the scan, we load all paragraphs and refresh all ref fields
            console.log("Starting consistency scan");
            await getAllParagraphs(formattingParamsToLoad);
            CURRENT_PARAGRAPG_INDEX = 0;
        }
        return await startFormattingScan();
    },

    correctFormatting: async (idToCorrect) => {
        // we are assuiming that the paragraph is already selected
        result = false;
        console.log("Correcting paragraph " + idToCorrect);
        if (GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX].uniqueLocalId !== idToCorrect) {
            console.log("Current paragraph is not the one we are looking for");
            // Current paragraph is not the one we are looking for
            // TODO - find the paragraph with the idToCorrect
            result = false;
            return;
        }
        await Word.run(async (context) => {
            var selection = context.document.getSelection();
            // Load the paragraph that contains the selection
            selection.paragraphs.load('uniqueLocalId');
            await context.sync();
            var paragraph = selection.paragraphs.items.find(para => para.uniqueLocalId === idToCorrect);
            if (paragraph === undefined) {
                console.log('Selection has changed, the id is not correct');
                result = false;
                return;
            }
            console.log('Setting paragraph formatting', dataService);
            paragraph.font.name = dataService.fontName;
            paragraph.font.size = dataService.fontSize;
            paragraph.alignment = dataService.alligment;
            paragraph.lineSpacing = dataService.lineSpacingInPoints;
            paragraph.leftIndent = dataService.leftIndentInPoints;
            paragraph.rightIndent = dataService.rightIndentInPoints;
            paragraph.select();
            await context.sync();
            await saveSelectedParagraphAtCurrentIndex(formattingParamsToLoad);
            result = true;
        });
        return result;
    }
}


async function startFormattingScan() {
    // list of found errors
    const errors = [];
    var paraId = undefined;
    var isErrorr = false;
    for (var i = CURRENT_PARAGRAPG_INDEX; i < GLOBAL_PARAGRAPHS.items.length; i++) {
        paragraph = GLOBAL_PARAGRAPHS.items[i];
        CURRENT_PARAGRAPG_INDEX = i;
        console.log(dataService.ignoredParagraphs);
        console.log("Checking: ", paragraph.text, paragraph.uniqueLocalId, dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId));
        if (dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId) || paragraph.text === "") {
            continue;
        }
        const styleChecks = [
            { condition: paragraph.font.name !== dataService.fontName, errorType: formattingErrorTypes.INCORRECT_FONT_NAME },
            { condition: paragraph.font.size !== dataService.fontSize, errorType: formattingErrorTypes.INCORRECT_FONT_SIZE },
            { condition: paragraph.alignment !== dataService.alligment, errorType: formattingErrorTypes.INCORRECT_ALIGNMENT },
            { condition: paragraph.lineSpacing !== dataService.lineSpacingInPoints, errorType: formattingErrorTypes.INCORRECT_LINE_SPACING },
            { condition: paragraph.leftIndent > dataService.leftIndentInPoints + DELTA || paragraph.leftIndent < dataService.leftIndentInPoints - DELTA, errorType: formattingErrorTypes.INCORRECT_LEFT_INDENT },
            { condition: paragraph.rightIndent > dataService.rightIndentInPoints + DELTA || paragraph.rightIndent < dataService.rightIndentInPoints - DELTA , errorType: formattingErrorTypes.INCORRECT_RIGHT_INDENT },
        ];
        // Iterate over checks
        styleChecks.forEach(check => {
            if (check.condition) {
                errors.push(check.errorType);
            }
        });
        if (errors.length > 0) {
            paraId = paragraph.uniqueLocalId;
            isErrorr = true;
            console.log("This paragraph is wrong: ", paragraph.text, paragraph.style, paragraph.font.size, paragraph.font.name, paragraph.alignment, paragraph.lineSpacing);
            await selectParagraph(CURRENT_PARAGRAPG_INDEX);
            console.log(errors);
            break;
        }
    }
    const FormattingReturnValue = {
        FoundError: isErrorr,
        ParagraphId: paraId,
        ErrorTypes: errors
    };
    console.log(JSON.stringify(FormattingReturnValue));
    return FormattingReturnValue;
}
