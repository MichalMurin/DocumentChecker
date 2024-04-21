/**
 * Saves a file with the given URI and filename.
 * @param {string} uri - The URI of the file to be saved.
 * @param {string} filename - The desired filename for the saved file.
 */
window.saveAsFile = (uri, filename) => {
    var link = document.createElement('a');
    link.href = uri;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

/**
 * Triggers the import action for the element with the specified ID.
 * @param {string} elemntId - The ID of the element to trigger the import action.
 */
window.TriggerImport = (elemntId) => {
    document.getElementById(elemntId).click();
}

/**
 * Inserts the specified text at the end of the document body.
 * @param {string} text - The text to be inserted.
 */
window.InsertText = async (text) => {
    await Word.run(async (context) => {
        // Create a proxy object for the document body.
        const body = context.document.body;
        // Queue a command to insert text at the end of the document body.
        body.insertText(text, Word.InsertLocation.end);
        // Queue a command to get the current selection.
        // Synchronize the document state by executing the queued commands,
        // and return a promise to indicate task completion.
        await context.sync();
        console.log('Inserted text');
    })
        .catch(function (error) {
            console.log('Error: ' + JSON.stringify(error));
            if (error instanceof OfficeExtension.Error) {
                console.log('Debug info: ' + JSON.stringify(error.debugInfo));
            }
        });
}

/**
 * Selects the paragraph at the specified index.
 * @param {number} index - The index of the paragraph to be selected.
 */
async function selectParagraph(index) {
    console.log("Selecting paragraph at index " + index);
    await Word.run(async (context) => {
        const paragraphs = context.document.body.paragraphs;
        paragraphs.load('uniqueLocalId');
        await context.sync();
        paragraphs.items[index].select();
        await context.sync();
    });
}
