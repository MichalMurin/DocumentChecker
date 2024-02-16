
// Global parameter, where are stored curently loaded paragraphs
GLOBAL_PARAGRAPHS = undefined; 
// Index of curently checked paragraph
CURRENT_PARAGRAPG_INDEX = 0;

window.saveAsFile = (uri, filename) => {
    var link = document.createElement('a');
    link.href = uri;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.TriggerImport = (elemntId) => {
    document.getElementById(elemntId).click();
}

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
        console.log('Inserted text ');
    })
        .catch(function (error) {
            console.log('Error: ' + JSON.stringify(error));
            if (error instanceof OfficeExtension.Error) {
                console.log('Debug info: ' + JSON.stringify(error.debugInfo));
            }
        });
}
async function getAllParagraphs(atributesToLoad) {
    console.log('Getting all paragraphs');
    if (!atributesToLoad.includes('uniqueLocalId')) {
        atributesToLoad += ', uniqueLocalId';
    }
    retParagraphs = undefined;
    await Word.run(async (context) => {
        const paragraphs = context.document.body.paragraphs;
        paragraphs.load(atributesToLoad);
        await context.sync();
        retParagraphs = paragraphs;
    });
    GLOBAL_PARAGRAPHS = retParagraphs;
    //CURRENT_PARAGRAPG_INDEX = 0;
    return retParagraphs;
}

async function selectParagraph(index) {
    console.log("Selecting paragraph at index " + index);
    await Word.run(async (context) => {
        const paragraphs = context.document.body.paragraphs;
        paragraphs.load('uniqueLocalId');
        await context.sync();
        if (paragraphs.items.length === GLOBAL_PARAGRAPHS.items.length) {
            paragraphs.items[index].select();
        }
        else {
            console.log('Paragraphs are not the same');
            var id = GLOBAL_PARAGRAPHS.items[index].uniqueLocalId;
            for (let i = 0; i < paragraphs.items.length; i++) {
                if (paragraphs.items[i].uniqueLocalId === id) {
                    paragraphs.items[i].select();
                    break;
                }
            }
        }
        await context.sync();
    });
}


