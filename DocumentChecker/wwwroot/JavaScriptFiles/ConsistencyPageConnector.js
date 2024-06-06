/**
 * Represents the data service.
 */
var dataService = null;

/**
 * Represents the current paragraph.
 */
var currentParagraphGlobal = null;

/**
 * Represents the consistency parameters to load.
 */
const consistencyParamsToLoad =
  "text, alignment, font, style, fields, listItemOrNullObject, listOrNullObject, tableNestingLevel, inlinePictures, isLastParagraph, uniqueLocalId";

/**
 * Represents the regular expressions for dots, commas, colons with space.
 */
const dotsComasColonsSpaceRegex = [/ \./, / ,/, / :/];

/**
 * Represents the regular expressions for dots, commas, colons without space.
 */
const dotsComasColonsNoSpaceRegex = [/\.(?=[^\s\d])/, /\,(?=[^\s\d])/, /:\S/];

/**
 * Represents the cross reference error messages.
 */
const crossReferenceError = [
  "Error! Reference source not found.",
  "Chyba! Nenašiel sa žiaden zdroj odkazov.",
];

/**
 * Represents the consistency error types.
 */
const consistencyErrorTypes = {
  DOUBLE_SPACES: "DoubleSpaces",
  EMPTY_LINES: "EmptyLines",
  INVALID_CROSS_REFERENCE: "InvalidCrossRef",
  INVALID_HEADING_CONTINUITY: "InvalidHeadingContinuity",
  INVALID_HEADING_CONSISTENCY: "InvalidHeadingConsistency",
  INVALID_HEADING_NUMBER_CONSISTENCY: "InvalidHeadingNumberConsistency",
  INCONSISTENT_FORMATTING: "InconsistentFormatting",
  INVALID_PARENTHESIS: "InvalidParenthesis",
  INVALID_DOTS_COMAS_COLONS: "InvalidDotsComasColons",
  CAPTION_MISSING: "CaptionMissing",
  INVALID_LIST_CONSISTENCY: "InvalidListConsistency",
};

/**
 * Represents the heading format types.
 */
const headingFormatTypes = {
  UPPER_CASE: "UpperCase",
  LOWER_CASE: "LowerCase",
  FIRST_UPPER_CASE: "FirstUpperCase",
  UNKNOWN: "Unknown",
};

/**
 * Represents the previous list.
 */
var previousList = undefined;

/**
 * Represents the previous paragraphs by style.
 */
var previousParagraphsByStyle = {};

/**
 * Represents the previous paragraphs keys.
 */
const previousParagraphsKeys = {
  HEADING: "Heading",
  NUMBERED_HEADING: "NumberedHeading",
  LIST_ITEM: "PreviousListItem",
  PREVIOUS_PARAGRAPH: "PreviousParagraph",
};

/**
 * Represents the previous paragraphs.
 */
var previousParagraphs = {};

/**
 * Represents the consistency connector.
 */
window.consistencyConnector = {
  /**
   * Checks the consistency.
   * @param {boolean} start - Indicates if the scan is starting.
   * @param {object} data - The data to be used for consistency check.
   * @returns {Promise<object>} - The result of the consistency scan.
   */
  checkConsistency: async (start, data) => {
    console.log(data);
    dataService = data;
    if (start) {
      // if we are starting the scan, we load all paragraphs and refresh all ref fields
      console.log("Starting consistency scan");
      resetAtrributes();
      await refresshAllRefFields();
    }
    return await startConsistencyScan(start);
  },

  /**
   * Handles the ignored paragraph.
   * @param {string} ignoredId - The ID of the ignored paragraph.
   * @param {Array<string>} foundErrors - The errors found in the ignored paragraph.
   * @returns {Promise<boolean>} - Indicates if the ignored paragraph was handled successfully.
   */
  handleIgnoredParagraph: async (ignoredId, foundErrors) => {
    console.log(
      "Handling ingored paragraph " + ignoredId + " with errors: ",
      foundErrors
    );
    console.log("Current paragraph: ", currentParagraphGlobal);
    if (
      currentParagraphGlobal !== null &&
      ignoredId === currentParagraphGlobal.uniqueLocalId
    ) {
      // handling ignored paragraph if the previous one was
      foundErrors.forEach((error) => {
        switch (error) {
          case consistencyErrorTypes.CAPTION_MISSING:
            console.log("Handling caption missing - ignored");
            console.log(
              "previous: ",
              previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH]
            );
            // setting ignored paragraph as previous paragraph, so it wont be expecting any caption again
            previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH] =
              currentParagraphGlobal;
            console.log(
              "previous: ",
              previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH]
            );
            break;
          case consistencyErrorTypes.INVALID_LIST_CONSISTENCY:
            // setting ignored paragraph as previous paragraph
            if (!currentParagraphGlobal.listOrNullObject.isNullObject) {
              previousList = currentParagraphGlobal.listOrNullObject;
            }
            break;
          case consistencyErrorTypes.INVALID_HEADING_CONSISTENCY:
          case consistencyErrorTypes.INVALID_HEADING_NUMBER_CONSISTENCY:
            if (
              !foundErrors.includes(
                consistencyErrorTypes.INVALID_HEADING_CONTINUITY
              ) &&
              GetNumberOfHeading(currentParagraphGlobal) !== null
            ) {
              previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING] =
                currentParagraphGlobal;
            }
            break;
          default:
            // Code for default case
            break;
        }
      });
      return true;
    }
    return false;
  },

  /**
   * Corrects the paragraph.
   * @param {string} idToCorrect - The ID of the paragraph to correct.
   * @param {Array<string>} errorToCorrect - The errors to correct in the paragraph.
   * @returns {Promise<boolean>} - Indicates if the paragraph was corrected successfully.
   */
  corectParagraph: async (idToCorrect, errorToCorrect) => {
    let result = false;
    console.log(
      "Correcting paragraph " + idToCorrect + " with data: ",
      errorToCorrect
    );
    await Word.run(async (context) => {
      var selection = context.document.getSelection();
      // Load the paragraph that contains the selection
      selection.paragraphs.load(
        "uniqueLocalId, text, style, listItemOrNullObject"
      );
      await context.sync();
      var paragraph = selection.paragraphs.items.find(
        (para) => para.uniqueLocalId === idToCorrect
      );
      if (paragraph === undefined) {
        console.log("Selection has changed, the id is not correct");
        return false;
      }
      var textItem = paragraph.getText();
      await context.sync();
      let sourceParagraphText = textItem.value.trimEnd();
      console.log("Correcting paragraph: ", sourceParagraphText);
      try {
        errorToCorrect.forEach((foundError) => {
          switch (foundError) {
            case consistencyErrorTypes.DOUBLE_SPACES:
              sourceParagraphText = sourceParagraphText.replace(/ {2,}/g, " ");
              paragraph.insertText(sourceParagraphText, "Replace");
              break;
            case consistencyErrorTypes.EMPTY_LINES:
              console.log("Correcting empty lines");
              // We are checking empty line only on not last paragraphs -> there will be always next paragraph
              var nextParagraph = paragraph.getNext();
              paragraph.delete();
              paragraph = nextParagraph;
              break;
            case consistencyErrorTypes.INVALID_CROSS_REFERENCE:
              // Code for INVALID_CROSS_REFERENCE error type
              console.log(
                "Correcting invalid cross reference",
                sourceParagraphText
              );
              var newText = paragraph.text;
              crossReferenceError.forEach((error) => {
                let regex = new RegExp(error, "g");
                newText = newText.replace(regex, "");
              });
              newText = newText.replace(/ {2,}/g, " ");
              console.log("Corrected text in paragraph: ", newText);
              paragraph.insertText(newText, "Replace");

              // Cannot fix this issue automaticaly
              break;
            case consistencyErrorTypes.INVALID_HEADING_CONSISTENCY:
              // Code for INVALID_HEADING_CONSISTENCY error type
              var previousHeading =
                previousParagraphs[previousParagraphsKeys.HEADING];
              var previousHeadingFormatType = getHeadingFormatType(
                previousHeading.text
              );
              sourceParagraphText = updateHeadingFormatType(
                sourceParagraphText,
                previousHeadingFormatType
              );
              paragraph.insertText(sourceParagraphText, "Replace");
              // TODO - fix the heading + update previous heading
              break;
            case consistencyErrorTypes.INVALID_HEADING_NUMBER_CONSISTENCY:
              // Code for INVALID_HEADING_CONSISTENCY error type
              console.log(
                "Correcting heading number consistency",
                sourceParagraphText
              );
              sourceParagraphText =
                updateHeadingNumberFormat(sourceParagraphText);
              console.log("Corrected text in paragraph: ", sourceParagraphText);
              paragraph.insertText(sourceParagraphText, "Replace");
              break;
            case consistencyErrorTypes.INCONSISTENT_FORMATTING:
              // Code for INCONSISTENT_FORMATTING error type
              var previousPara = previousParagraphsByStyle[paragraph.style];
              if (previousPara !== undefined) {
                // checking formatting with previous paragraph with same style
                console.log("Correcting formatting: ", paragraph, previousPara);
                paragraph.alignment = previousPara.alignment;
                paragraph.font.name = previousPara.font.name;
                paragraph.font.size = previousPara.font.size;
              }
              break;
            case consistencyErrorTypes.INVALID_DOTS_COMAS_COLONS:
              console.log(
                "Correcting invalid dots, comas, colons",
                sourceParagraphText
              );
              // Code for INVALID_DOTS_COMAS_COLONS error type
              var prefix = "";
              if (isParagraphHeading(paragraph) && /^\d/.test(paragraph.text)) {
                prefix = GetNumberOfHeading(paragraph);
                sourceParagraphText = sourceParagraphText.slice(prefix.length);
                // paragraph is heading and it starts with number -> do not correct dots in the number of heading
              }
              dotsComasColonsNoSpaceRegex.forEach((regex) => {
                var regexWithG = new RegExp(regex.source, "g");
                sourceParagraphText = sourceParagraphText.replace(
                  regexWithG,
                  function (match) {
                    console.log("Match: ", match);
                    //return match[0] + ' ' + match[1];
                    return match[0] + " ";
                  }
                );
                console.log("First correction", regex, sourceParagraphText);
              });
              sourceParagraphText = prefix + sourceParagraphText;
              dotsComasColonsSpaceRegex.forEach((regex) => {
                var regexWithG = new RegExp(regex.source, "g");
                sourceParagraphText = sourceParagraphText.replace(
                  regexWithG,
                  function (match) {
                    return match.trim();
                  }
                );
                console.log("Second correction", sourceParagraphText);
              });
              paragraph.insertText(sourceParagraphText, "Replace");
              break;
            case consistencyErrorTypes.INVALID_LIST_CONSISTENCY:
              // Code for INVALID_PARENTHESIS error type
              console.log(
                "Correcting list inconsistency - THIS ERROR CANNOT BE CORRECTED AUTOMATICALLY"
              );
              break;
            case consistencyErrorTypes.INVALID_PARENTHESIS:
              // Code for INVALID_PARENTHESIS error type
              console.log(
                "Correcting invalid parenthesis - THIS ERROR CANNOT BE CORRECTED AUTOMATICALLY"
              );
              break;
            case consistencyErrorTypes.CAPTION_MISSING:
              // Code for INVALID_PARENTHESIS error type
              console.log(
                "Correcting caption missing - THIS ERROR CANNOT BE CORRECTED AUTOMATICALLY"
              );
              break;
            case consistencyErrorTypes.INVALID_HEADING_CONTINUITY:
              // Code for INVALID_HEADING_CONTINUITY error type
              console.log(
                "Correcting heading continuity - THIS ERROR CANNOT BE CORRECTED AUTOMATICALLY"
              );
              break;
            default:
              // Code for default case
              break;
          }
        });
      } catch (error) {
        console.log("Error while correcting paragraph: ", error);
        return false;
      }
      paragraph.select();
      await context.sync();
      result = true;
    });
    return result;
  },
};

/**
 * Resets the attributes.
 */
function resetAtrributes() {
  previousList = undefined;
  previousParagraphsByStyle = {};
  previousParagraphs = {};
  currentParagraphGlobal = null;
}

/**
 * Starts the consistency scan.
 * @param {boolean} start - Indicates if the scan is starting.
 * @returns {Promise<object>} - The result of the consistency scan.
 */
async function startConsistencyScan(start) {
  // list of found errors
  const errors = [];
  var paraId = undefined;
  var isErrorr = false;
  return Word.run(async (context) => {
    if (start) {
      console.log("Getting first paragraph");
      var paragraph = context.document.body.paragraphs.getFirst();
      paragraph.load(consistencyParamsToLoad);
      await context.sync();
    } else {
      console.log("Getting selected paragraph");
      let selection = context.document.getSelection();
      // Load the paragraph that contains the selection
      selection.paragraphs.load(consistencyParamsToLoad);
      await context.sync();
      if (selection.paragraphs.items.length > 0) {
        paragraph = selection.paragraphs.items[0];
      } else {
        // TODO - check if the paragraph stayed selected - if it is not null
        console.log("Selection has changed, I cannot find any paragraph");
      }
    }
    let paragraphTextItem = paragraph.getText();
    await context.sync();
    while (paragraph !== null && !paragraph.isNullObject) {
      currentParagraphGlobal = paragraph;
      let paragraphText = paragraphTextItem.value.trimEnd();
      paragraph.text = console.log("Checking: ", paragraph);
      console.log("Checking: ", paragraph.text);
      console.log("Ignored paragraphs: ", dataService.ignoredParagraphs);
      console.log(
        "This id",
        paragraph.uniqueLocalId,
        " is ignored: ",
        dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId)
      );
      if (!dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId)) {
        const styleChecks = prepareChecks(paragraph, paragraphText);
        console.log("Style checks: ", styleChecks);
        // Iterate over checks
        styleChecks.forEach((check) => {
          console.log("Checking: ", check.errorType);
          console.log("Result: ", check.condition);
          if (check.condition) {
            errors.push(check.errorType);
          }
        });
        if (errors.length > 0) {
          console.log("This paragraph is wrong: ", paragraph.text);
          paraId = paragraph.uniqueLocalId;
          isErrorr = true;
          paragraph.select();
          console.log(errors);
          break;
        }
      }
      console.log(
        "Saving this paragraph as previous, before going to the next one"
      );
      UpdatePreviousParagraph(paragraph);
      console.log("Moving to next paragraph");
      paragraph = paragraph.getNextOrNullObject();
      paragraph.load(consistencyParamsToLoad);
      paragraphTextItem = paragraph.getText();
      await context.sync();
      console.log("Next paragraph is null?", paragraph.isNullObject);
    }
    const ScanReturnValue = {
      FoundError: isErrorr,
      ParagraphId: paraId,
      ErrorTypes: errors,
    };
    console.log(JSON.stringify(ScanReturnValue));
    return ScanReturnValue;
  });
}

/**
 * Updates the previous paragraph.
 * @param {object} paragraph - The paragraph to update.
 */
function UpdatePreviousParagraph(paragraph) {
  var isHeading = isParagraphHeading(paragraph);
  if (!dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId)) {
    if (paragraph.text !== "") {
      previousParagraphsByStyle[paragraph.style] = paragraph;
      if (isHeading) {
        previousParagraphs[previousParagraphsKeys.HEADING] = paragraph;
        if (GetNumberOfHeading(paragraph) !== null) {
          previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING] =
            paragraph;
        }
      }
      if (!paragraph.listItemOrNullObject.isNullObject) {
        previousParagraphs[previousParagraphsKeys.LIST_ITEM] = paragraph;
      }
    }
    if (!paragraph.listOrNullObject.isNullObject && !isHeading) {
      previousList = paragraph.listOrNullObject;
    }
    previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH] = paragraph;
  }
}

function prepareChecks(paragraph, paragraphCleanText) {
  if (paragraph.text === "") {
    let styleChecks = [
      {
        condition: dataService.emptyLines && isEmptyLine(paragraph),
        errorType: consistencyErrorTypes.EMPTY_LINES,
      },
    ];
    return styleChecks;
  }

  console.log("Preparing checks: ", dataService.doubleSpaces);
  let wrongHeadingContinuity = false;
  let wrongHeadingConsistency = false;
  let wrongHeadingNumberConsistency = false;
  if (isParagraphHeading(paragraph)) {
    console.log("This is heading, checking it: ", paragraph.text);
    let currentheadingNumber = GetNumberOfHeading(paragraph);
    if (currentheadingNumber !== null) {
      // if paragraph is numbered heading, it has number -> we check heading continuity
      wrongHeadingContinuity = !checkHeadingContinuity(currentheadingNumber);
      wrongHeadingNumberConsistency =
        !checkHeadingNumberConsistency(currentheadingNumber);
    }
    wrongHeadingConsistency = !checkchHeadingConsistency(paragraph);
  }
  const styleChecks = [
    {
      condition: dataService.doubleSpaces && checkDoubleSpaces(paragraph),
      errorType: consistencyErrorTypes.DOUBLE_SPACES,
    },
    {
      condition:
        dataService.crossReferenceFunctionality &&
        checkInvalidCrossReference(paragraph),
      errorType: consistencyErrorTypes.INVALID_CROSS_REFERENCE,
    },
    {
      condition: dataService.titleContinutity && wrongHeadingContinuity,
      errorType: consistencyErrorTypes.INVALID_HEADING_CONTINUITY,
    },
    {
      condition: dataService.titleConsistency && wrongHeadingNumberConsistency,
      errorType: consistencyErrorTypes.INVALID_HEADING_NUMBER_CONSISTENCY,
    },
    {
      condition: dataService.titleConsistency && wrongHeadingConsistency,
      errorType: consistencyErrorTypes.INVALID_HEADING_CONSISTENCY,
    },
    {
      condition:
        dataService.documentAlignment && isDifferentFormatting(paragraph),
      errorType: consistencyErrorTypes.INCONSISTENT_FORMATTING,
    },
    {
      condition:
        dataService.parenthesesValidation && !isValidParenthesis(paragraph),
      errorType: consistencyErrorTypes.INVALID_PARENTHESIS,
    },
    {
      condition:
        dataService.dotsComasColonsValidation &&
        !checkDotsComasColons(paragraphCleanText, paragraph),
      errorType: consistencyErrorTypes.INVALID_DOTS_COMAS_COLONS,
    },
    {
      condition: dataService.captionValidation && !isCaptionPresent(paragraph),
      errorType: consistencyErrorTypes.CAPTION_MISSING,
    },
    {
      condition: dataService.listValidation && !isListConsistent(paragraph),
      errorType: consistencyErrorTypes.INVALID_LIST_CONSISTENCY,
    },
    // Add more checks as needed
  ];
  return styleChecks;
}

function isParagraphHeading(paragraph) {
  console.log("Checking if paragraph is heading: ", paragraph.style);
  return (
    paragraph.text !== "" &&
    (paragraph.style.includes("Heading") || paragraph.style.includes("Nadpis"))
  );
}

function GetNumberOfHeading(paragraph) {
  if (!paragraph.listItemOrNullObject.isNullObject) {
    // if paragraph is part of a list, we consider it as a heading
    let listString = paragraph.listItemOrNullObject.listString;
    if (/^[0-9.]+$/.test(listString)) {
      // if list string is only numbers and dots, we consider it as a number in heading
      return listString;
    } else {
      return null;
    }
  } else if (/^\d/.test(paragraph.text)) {
    // if it is not par of a list, but it starts with a number, we consider it as a heading
    return paragraph.text.split(" ")[0];
  } else {
    return null;
  }
}

async function refresshAllRefFields() {
  return Word.run(async (context) => {
    const fields = context.document.body.fields.load("items");
    await context.sync();
    fields.items.forEach((field) => {
      if (field.type === "Ref") {
        field.updateResult();
      }
    });
  });
}

function checkDoubleSpaces(paragraph) {
  return /\s{2,}/.test(paragraph.text);
}

function checkInvalidCrossReference(paragraph) {
  let result = false;
  console.log("Checking cross reference: ", paragraph.text);
  crossReferenceError.forEach((error) => {
    console.log("Checking ref error: ", error);
    if (paragraph.text.includes(error)) {
      result = true;
    }
  });
  return result;
}

function checkHeadingContinuity(currentNumber) {
  console.log("Checking continuity ... Current number: ", currentNumber);
  let previousHeading =
    previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING];
  if (previousHeading !== undefined) {
    let previousNumber = removeEndDot(GetNumberOfHeading(previousHeading));
    currentNumber = removeEndDot(currentNumber);
    let continuingNumbers = GetNextPossibleNumbers(previousNumber);
    console.log("Possible next numbers: ", continuingNumbers);
    if (continuingNumbers.includes(currentNumber)) {
      return true;
    } else {
      return false;
    }
  }
  // if there is no previous heading, we cannot check the continuity -> this is the first heading
  return true;
}

function removeEndDot(number) {
  if (number.endsWith(".")) {
    return number.slice(0, -1);
  }
  return number;
}

function checkHeadingNumberConsistency(currentNumber) {
  console.log(
    "Checking number consistency ... Current number: ",
    currentNumber
  );
  let previousHeading =
    previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING];
  if (previousHeading !== undefined) {
    let previousNumber = GetNumberOfHeading(previousHeading);
    if (
      (previousNumber.endsWith(".") && currentNumber.endsWith(".")) ||
      (!previousNumber.endsWith(".") && !currentNumber.endsWith("."))
    ) {
      return true;
    } else {
      return false;
    }
  }
  // if there is no previous heading, we cannot check the continuity -> this is the first heading
  return true;
}

function updateHeadingNumberFormat(heading) {
  let previousHeading =
    previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING];
  if (previousHeading !== undefined && /^\d/.test(heading)) {
    let previousNumber = GetNumberOfHeading(previousHeading);
    let whitespaceIndex = heading.indexOf(" ");
    if (previousNumber.endsWith(".")) {
      return (
        heading.slice(0, whitespaceIndex) + "." + heading.slice(whitespaceIndex)
      );
    } else if (!previousNumber.endsWith(".")) {
      if (heading.slice(0, whitespaceIndex).endsWith(".")) {
        return (
          heading.slice(0, whitespaceIndex - 1) +
          " " +
          heading.slice(whitespaceIndex + 1)
        );
      }
    }
  }
  return heading;
}

function checkchHeadingConsistency(paragraph) {
  let previousHeading = previousParagraphs[previousParagraphsKeys.HEADING];
  if (previousHeading !== undefined) {
    console.log(
      "previous heading: ",
      previousHeading.text,
      "current heading: ",
      paragraph.text
    );
    let previousHeadingFormatType = getHeadingFormatType(previousHeading.text);
    let currentHeadingFormatType = getHeadingFormatType(paragraph.text);
    // Check if the previous text is all uppercase
    if (previousHeadingFormatType !== currentHeadingFormatType) {
      return false;
    } else {
      previousHeading = paragraph;
    }
  }
  return true;
}

function getHeadingFormatType(heading) {
  let headingWords = heading.split(/\s+/);
  let regexToCheckFirstCapitalLetterWithDiacritics =
    /^[A-Z\u00C0-\u00FF\u0100-\u017F][a-z\u00C0-\u00FF\u0100-\u017F]*$/;
  if (heading === heading.toUpperCase()) {
    return headingFormatTypes.UPPER_CASE;
  } else if (heading === heading.toLowerCase()) {
    return headingFormatTypes.LOWER_CASE;
  } else if (
    regexToCheckFirstCapitalLetterWithDiacritics.test(headingWords[0])
  ) {
    return headingFormatTypes.FIRST_UPPER_CASE;
  } else {
    return headingFormatTypes.UNKNOWN;
  }
}

function updateHeadingFormatType(heading, type) {
  switch (type) {
    case headingFormatTypes.LOWER_CASE:
      return heading.toLowerCase();
    case headingFormatTypes.UPPER_CASE:
      return heading.toUpperCase();
    case headingFormatTypes.FIRST_UPPER_CASE:
      return heading.charAt(0).toUpperCase() + heading.slice(1).toLowerCase();
    default:
      return heading;
  }
}

function isValidParenthesis(paragraph) {
  const text = paragraph.text;
  const stack = [];
  for (let i = 0; i < text.length; i++) {
    const char = text[i];
    if (char === "(" || char === "[" || char === "{" || char === "„") {
      stack.push(char);
      console.log("Pushed: ", char, "Stack: ", stack);
    } else if (char === ")" || char === "]" || char === "}" || char === "“") {
      if (stack.length === 0) {
        return false;
      }
      const top = stack.pop();
      console.log("Popped: ", top, "Stack: ", stack);
      if (
        (char === ")" && top !== "(") ||
        (char === "]" && top !== "[") ||
        (char === "}" && top !== "{") ||
        (char === "“" && top !== "„")
      ) {
        return false;
      }
    }
  }
  return stack.length === 0;
}

function checkDotsComasColons(paragraphCleanText, paragraph) {
  let text = paragraphCleanText;
  console.log("Checking dots, comas, colons: ", text);
  if (isParagraphHeading(paragraph)) {
    let headingNumber = GetNumberOfHeading(paragraph);
    if (headingNumber !== null) {
      text = text.slice(headingNumber.length);
    }
  }
  let result = dotsComasColonsSpaceRegex.every((regex) => {
    // testing every regex
    return !regex.test(text);
  });
  console.log("First test result: ", result);
  if (result) {
    // if first test passed, testign second array of regex
    result = dotsComasColonsNoSpaceRegex.every((regex) => {
      return !regex.test(text);
    });
  }
  console.log("Second test result: ", result);
  return result;
}

function GetNextPossibleNumbers(previousNumber) {
  previousNumber = removeEndDot(previousNumber);
  let parts = previousNumber.split(".");
  let results = [];
  console.log("Previous parts: ", parts);

  // Increment each part from the end
  for (let i = parts.length - 1; i >= 0; i--) {
    let newParts = parts.slice(0, i + 1);
    newParts[i] = parseInt(newParts[i]) + 1;
    results.push(newParts.join("."));
  }

  // Add the next major heading
  results.push(`${previousNumber}.1`);
  return results;
}

function isEmptyLine(paragraph) {
  let previousPara =
    previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH];
  console.log(
    "checking empty line, previous: ",
    previousPara,
    "current: ",
    paragraph
  );
  if (
    !paragraph.isLastParagraph &&
    previousPara !== undefined &&
    previousPara.text.trim() === "" &&
    paragraph.text.trim() === ""
  ) {
    return true;
  }
  return false;
}

function isDifferentFormatting(paragraph) {
  let previousPara = previousParagraphsByStyle[paragraph.style];
  if (previousPara !== undefined) {
    // checking formatting with previous paragraph with same style
    console.log(
      "Checking formatting: ",
      paragraph,
      "previousPara: ",
      previousPara
    );
    return (
      paragraph.alignment !== previousPara.alignment ||
      paragraph.font.name !== previousPara.font.name ||
      paragraph.font.size !== previousPara.font.size
    );
  }
  return false;
}

function isCaptionPresent(currentParagraph) {
  let previousPara =
    previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH];
  if (previousPara !== undefined) {
    console.log(
      "Previous paragraph: ",
      previousPara,
      "This paragraph: ",
      currentParagraph
    );
    if (
      previousPara.tableNestingLevel > 0 &&
      currentParagraph.tableNestingLevel === 0
    ) {
      console.log(
        "Previous paragraph is in table, this is not -> so this should be a caption"
      );
      return (
        currentParagraph.style.includes("Caption") ||
        currentParagraph.style.includes("Popis")
      );
    } else if (
      previousPara.inlinePictures.items.length > 0 &&
      currentParagraph.inlinePictures.items.length === 0
    ) {
      console.log(
        "Previous paragraph was image, and this is not -> this should be caption"
      );
      return (
        currentParagraph.style.includes("Caption") ||
        currentParagraph.style.includes("Popis")
      );
    }
  }
  return true;
}

function isListConsistent(currentParagraph) {
  console.log(
    "Checking list consistency: ",
    previousList,
    currentParagraph.listOrNullObject
  );
  if (
    previousList !== undefined &&
    !currentParagraph.listOrNullObject.isNullObject &&
    previousList.id !== currentParagraph.listOrNullObject.id &&
    !isParagraphHeading(currentParagraph)
  ) {
    console.log("This is list, checking level types");
    for (let i = 0; i < previousList.levelTypes.length; i++) {
      let level = currentParagraph.listOrNullObject.levelTypes[i];
      let previousLevel = previousList.levelTypes[i];
      console.log("Checking level: ", level, "Previous level: ", previousLevel);
      if (level !== previousLevel) {
        return false;
      }
    }
  }
  return true;
}
