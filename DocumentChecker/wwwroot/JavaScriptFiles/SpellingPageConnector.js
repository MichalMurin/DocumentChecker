/**
 * Represents the spelling connector object.
 */
window.spellingConnector = {
  /**
   * Collects all paragraphs in the document.
   * @returns {Array} The list of paragraphs.
   */
  collectAllParagraphs: async () => {
    let resultList = [];
    await Word.run(async (context) => {
      const paragraphs = context.document.body.paragraphs;
      paragraphs.load("uniqueLocalId");
      await context.sync();
      for (let [index, paragraph] of paragraphs.items.entries()) {
        var paraTextItem = paragraph.getText();
        await context.sync();
        let paragraphData = {
          index: index,
          id: paragraph.uniqueLocalId,
          text: paraTextItem.value.trimEnd(),
        };
        resultList.push(paragraphData);
      }
    });
    console.log("Returning paragraphs");
    console.log(JSON.stringify(resultList));
    return resultList;
  },

  /**
   * Selects a paragraph at the specified index.
   * @param {number} index - The index of the paragraph to select.
   */
  selectParagraphAtIndex: async (index) => {
    await selectParagraph(index);
  },

  /**
   * Replaces the selected text with the specified text.
   * @param {string} text - The text to replace with.
   */
  replaceSelectedText: async (text) => {
    console.log("Starting text replacement");
    await Word.run(async (context) => {
      const range = context.document.getSelection();
      range.load("text");
      await context.sync();
      console.log("Current selected text: " + range.text);
      range.insertText(text, Word.InsertLocation.replace);
      await context.sync();
      console.log("Text replaced with: " + text);
    });
    console.log("Text replacement finished");
  },
};
