

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

///////////////////////ARCHIVE/////////////////////////

//// Global parameter, where are stored curently loaded paragraphs
//GLOBAL_PARAGRAPHS = undefined;
//// Index of curently checked paragraph
//CURRENT_PARAGRAPG_INDEX = 0;

//async function getAllParagraphs(atributesToLoad) {
//    console.log('Getting all paragraphs');
//    if (!atributesToLoad.includes('uniqueLocalId')) {
//        atributesToLoad += ', uniqueLocalId';
//    }
//    retParagraphs = undefined;
//    await Word.run(async (context) => {
//        const paragraphs = context.document.body.paragraphs;
//        paragraphs.load(atributesToLoad);
//        await context.sync();
//        retParagraphs = paragraphs;
//    });
//    GLOBAL_PARAGRAPHS = retParagraphs;
//    return retParagraphs;
//}



//async function selectParagraph(index) {
//    console.log("Selecting paragraph at index " + index);
//    await Word.run(async (context) => {
//        var paragraph = context.document.body.paragraphs.items[index];
//        paragraph.load('uniqueLocalId');
//        paragraph.select();
//        await context.sync();
//        //paragraphs.select();
//        //await context.sync();
//    });
//}

async function saveSelectedParagraphAtCurrentIndex(paramsToLoad) {
    console.log('Saving selected paragraph at index ' + CURRENT_PARAGRAPG_INDEX);
    await Word.run(async (context) => {
        let newSelection = context.document.getSelection();
        // Load the paragraph that contains the selection
        newSelection.paragraphs.load(paramsToLoad);
        await context.sync();
        let newParagraph = newSelection.paragraphs.items[0];
        console.log("Paragraph corrected: ", newParagraph.text);
        console.log('Settign new item to global paragraphs');
        console.log('POaragraph', newParagraph.text , ' saving to index: ', CURRENT_PARAGRAPG_INDEX);
        GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX] = newParagraph;
    });
}

