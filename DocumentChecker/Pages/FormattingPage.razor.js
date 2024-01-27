
export async function InsertText(text) {
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
}

export async function GetAllText() {
    let text = "";
    await Word.run(async (context) => {

        // Create a proxy object for the document.
        const body = context.document.body;
        body.load("text");
        await context.sync();
        text = body.text;
        console.log("Text is" + text);
    })
        .catch(function (error) {
            console.log('Error: ' + JSON.stringify(error));
            if (error instanceof OfficeExtension.Error) {
                console.log('Debug info: ' + JSON.stringify(error.debugInfo));
            }
        });
    return { Value: text };
}

export async function GetWordInfos() {
    const wordInfoArray = [];
    await Word.run(async (context) => {
        /**
         * Insert your Word code here
         */

        var body = context.document.body;

        const docRange = context.document.body.getRange("Whole");
        docRange.load("text");
        await context.sync();
        const documentText = docRange.text;

        // Split the text into an array of words
        const words = documentText.split(/\s+/);

        // Array to store word information
        

        // Iterate through each word
        for (let i = 0; i < words.length; i++) {
            const word = words[i];

            // Search for the word in the document
            if (word === "") {
                continue;
            }
            console.log("Seraching for word", word);
            const searchResults = context.document.body.search(word, { matchCase: true, matchWholeWord: true });

            // Load properties of the search results
            searchResults.load("items");

            // Synchronize the search results
            await context.sync();

            // Iterate through each search result
            for (let j = 0; j < searchResults.items.length; j++) {
                const wordRange = searchResults.items[j];

                // Load properties of the word range
                wordRange.load("text, font, style, paragraphs");

                // Synchronize the word range
                await context.sync();

                // Extract information about the word
                const wordInfo = {
                    text: wordRange.text,
                    font: wordRange.font,
                    style: wordRange.style,
                    paragraphs: wordRange.paragraphs.items,
                };

                // Add the word information to the array
                wordInfoArray.push(wordInfo);
            }
        }

        // Serialize the array to JSON
        const wordInfoJson = JSON.stringify(wordInfoArray);
        // Log or process the JSON
        console.log(wordInfoJson);
    });
    return { Value: wordInfoArray };
}
