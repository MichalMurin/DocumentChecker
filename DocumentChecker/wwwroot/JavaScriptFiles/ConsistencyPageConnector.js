window.consisntencyConnector = {
    insertTextTest: async (text) => {
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
}