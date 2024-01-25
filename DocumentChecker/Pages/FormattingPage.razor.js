
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