var PARAGRAPHS = [];



window.spellingConnector = {
    collectAllParagraphs: async () => {
        let resultList = [];
        await Word.run(async (context) => {
            const paragraphs = context.document.body.paragraphs;
            paragraphs.load('uniqueLocalId, text');
            await context.sync();
            paragraphs.items.forEach((paragraph, index) => {
                let paragraphData = {
                    index: index,
                    id: paragraph.uniqueLocalId,
                    text: paragraph.text
                };
                PARAGRAPHS.push(paragraph);
                resultList.push(paragraphData);
            });
        });     
        console.log('Returning paragraphs');
        console.log(JSON.stringify(resultList));
        return resultList;
    }
}