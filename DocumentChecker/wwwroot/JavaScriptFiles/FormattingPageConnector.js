
window.formattingConnector = {
    insertText: async (text) => {
        await Word.run(async (context) => {
        // Create a proxy object for the document body.
        const body = context.document.body;
        // Queue a command to insert text at the end of the document body.
        body.insertText(text, Word.InsertLocation.end);
        // Queue a command to get the current selection.
        // Synchronize the document state by executing the queued commands,
        // and return a promise to indicate task completion.
        await context.sync();
        console.log('Inserted text ');
        })
        .catch(function (error) {
            console.log('Error: ' + JSON.stringify(error));
            if (error instanceof OfficeExtension.Error) {
                console.log('Debug info: ' + JSON.stringify(error.debugInfo));
            }
        });
    },

    checkParagraphs: async (ignoredIds, fontName, fontSize, alligment, lineSpacing, leftIndent, rightIndent, idToCorrect) => {
        console.log("retireved args: ", ignoredIds, fontName, fontSize, alligment, lineSpacing, leftIndent, rightIndent);
        let errors = [];
        let paraId = "";
        let isErrorr = false;
        await Word.run(async (context) => {
            const paragraphs = context.document.body.paragraphs;
            paragraphs.load("text, font, alignment, lineSpacing, style, uniqueLocalId, leftIndent, rightIndent");
            await context.sync();
            paragraphs.items.every((paragraph) =>
            {
                console.log("Checking: ", paragraph.text);
                if (paragraph.uniqueLocalId === idToCorrect)
                {
                    paragraph.font.name = fontName;
                    paragraph.font.size = fontSize;
                    paragraph.alignment = alligment;
                    paragraph.lineSpacing = lineSpacing;
                    paragraph.leftIndent = leftIndent;
                    paragraph.rightIndent = rightIndent;
                    return true;
                }
                if (ignoredIds.includes(paragraph.uniqueLocalId) || paragraph.text === "")
                {
                    return true;
                }
                const styleChecks = [
                    { condition: paragraph.font.name !== fontName, errorType: 'IncorrectFontName' },
                    { condition: paragraph.font.size !== fontSize, errorType: 'IncorrectFontSize' },
                    { condition: paragraph.alignment !== alligment, errorType: 'IncorrectAlignment' },
                    { condition: paragraph.lineSpacing !== lineSpacing, errorType: 'IncorrectLineSpacing' },
                    { condition: paragraph.leftIndent !== leftIndent, errorType: 'IncorrectLeftIndent' },
                    { condition: paragraph.rightIndent !== rightIndent, errorType: 'IncorrectRightIndent' },
                ];
                // Iterate over checks
                styleChecks.forEach(check => {
                    if (check.condition)
                    {
                        errors.push(check.errorType);
                    }
                });
                if (errors.length > 0) {
                    paraId = paragraph.uniqueLocalId;
                    isErrorr = true;
                    console.log("This paragraph is wrong: ", paragraph.text, paragraph.style, paragraph.font.size, paragraph.font.name, paragraph.alignment, paragraph.lineSpacing);
                    paragraph.select();
                    console.log(errors);
                    return false;
                }
                return true;
            });
            await context.sync();
        });
        const FormattingReturnValue = {
            FoundError: isErrorr,
            ParagraphId: paraId,
            ErrorTypes: errors
        };
        console.log(JSON.stringify(FormattingReturnValue));
        return FormattingReturnValue;
    }
}

