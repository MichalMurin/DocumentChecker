﻿//var PARAGRAPHS = [];



window.spellingConnector = {
    collectAllParagraphs: async () => {
        let resultList = [];
        await Word.run(async (context) => {
            const paragraphs = context.document.body.paragraphs;
            paragraphs.load('uniqueLocalId');
            await context.sync();
            for (let [index, paragraph] of paragraphs.items.entries()) {
                var paraTextItem = paragraph.getText();
                await context.sync();
                let paragraphData = {
                    index: index,
                    id: paragraph.uniqueLocalId,
                    text: paraTextItem.value.trimEnd()
                };
                resultList.push(paragraphData);
            }
        });     
        console.log('Returning paragraphs');
        console.log(JSON.stringify(resultList));
        return resultList;
    },
    selectParagraphAtIndex: async (index) => {
        await selectParagraph(index);
    },
    replaceSelectedText: async (text) => {
        console.log('Starting text replacement');
        await Word.run(async (context) => {
            const range = context.document.getSelection();
            range.load('text');
            await context.sync();
            console.log('Current selected text: ' + range.text);
            range.insertText(text, Word.InsertLocation.replace);
            await context.sync();
            console.log('Text replaced with: ' + text);
        });
        console.log('Text replacement finished');
    }
}