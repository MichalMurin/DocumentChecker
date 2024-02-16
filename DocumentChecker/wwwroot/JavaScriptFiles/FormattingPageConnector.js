// dataService to store data about formatiing
var dataService = undefined;
const DELTA = 0.1;
window.formattingConnector = {
    checkFormatting: async (start, data) => {
        console.log("retireved args: ", start, data);
        if (start) {
            // if we are starting the scan, we load all paragraphs and refresh all ref fields
            console.log("Starting consistency scan");
            dataService = data;
            await getAllParagraphs("text, font, alignment, lineSpacing, style, leftIndent, rightIndent");
            CURRENT_PARAGRAPG_INDEX = 0;
        }
        else {
            // if we are continuing with scan, we just continue from the last paragraph
            if (CURRENT_PARAGRAPG_INDEX == GLOBAL_PARAGRAPHS.items.length - 1) {
                // if current paragraph was the last one, we cannot continue
                return;
            }
            // we increase the paragraph index to continue from the next one
            CURRENT_PARAGRAPG_INDEX++;
        }
        return await startFormattingScan();
    },

    correctFormatting: async (idToCorrect, data) => {
        await correctParagraph(idToCorrect, data); 
    }
}

async function correctParagraph(idToCorrect) {
    console.log("Correcting paragraph " + idToCorrect);
    await Word.run(async (context) => {
        const paragraphs = context.document.body.paragraphs;
        paragraphs.load('uniqueLocalId');
        await context.sync();
        console.log("Current paragraph: ", GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX].uniqueLocalId);
        if (GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX].uniqueLocalId === idToCorrect) {
            console.log('Setting paragraph formatting', dataService);
            var paragraph = paragraphs.items[CURRENT_PARAGRAPG_INDEX];
            paragraph.font.name = dataService.fontName;
            paragraph.font.size = dataService.fontSize;
            paragraph.alignment = dataService.alligment;
            paragraph.lineSpacing = dataService.lineSpacingInPoints;
            paragraph.leftIndent = dataService.leftIndentInPoints;
            paragraph.rightIndent = dataService.rightIndentInPoints;
        }
        else {
            console.log("Current paragraph is not the one we are looking for");
            // Current paragraph is not the one we are looking for
        }
        await context.sync();
    });
}


async function startFormattingScan() {
    // list of found errors
    const errors = [];
    var paraId = undefined;
    var isErrorr = false;
    for (var i = CURRENT_PARAGRAPG_INDEX; i < GLOBAL_PARAGRAPHS.items.length; i++) {
        paragraph = GLOBAL_PARAGRAPHS.items[i];
        CURRENT_PARAGRAPG_INDEX = i;
        console.log("Checking: ", paragraph.text);
        if (dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId) || paragraph.text === "") {
            continue;
        }
        const styleChecks = [
            { condition: paragraph.font.name !== dataService.fontName, errorType: 'IncorrectFontName' },
            { condition: paragraph.font.size !== dataService.fontSize, errorType: 'IncorrectFontSize' },
            { condition: paragraph.alignment !== dataService.alligment, errorType: 'IncorrectAlignment' },
            { condition: paragraph.lineSpacing !== dataService.lineSpacingInPoints, errorType: 'IncorrectLineSpacing' },
            { condition: paragraph.leftIndent > dataService.leftIndentInPoints + DELTA || paragraph.leftIndent < dataService.leftIndentInPoints - DELTA, errorType: 'IncorrectLeftIndent' },
            { condition: paragraph.rightIndent > dataService.rightIndentInPoints + DELTA || paragraph.rightIndent < dataService.rightIndentInPoints - DELTA , errorType: 'IncorrectRightIndent' },
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

