﻿var dataService = null;
const consistencyParamsToLoad = 'text, alignment, font, style, fields, listItemOrNullObject, tableNestingLevel, inlinePictures, isLastParagraph, uniqueLocalId';
const dotsComasColonsSpaceRegex = [/ \./, / ,/, / :/];
const dotsComasColonsNoSpaceRegex = [/\.[^\s]/, /,\S/, /:\S/];
const crossReferenceError = ["Error! Reference source not found.", "Chyba! Nenašiel sa žiaden zdroj odkazov."];
const consistencyErrorTypes = {
    DOUBLE_SPACES: 'DoubleSpaces',
    EMPTY_LINES: 'EmptyLines',
    INVALID_CROSS_REFERENCE: 'InvalidCrossRef',
    INVALID_HEADING_CONTINUITY: 'InvalidHeadingContinuity',
    INVALID_HEADING_CONSISTENCY: 'InvalidHeadingConsistency',
    INVALID_HEADING_NUMBER_CONSISTENCY: 'InvalidHeadingNumberConsistency',
    INCONSISTENT_FORMATTING: 'InconsistentFormatting',
    INVALID_PARENTHESIS: 'InvalidParenthesis',
    INVALID_DOTS_COMAS_COLONS: 'InvalidDotsComasColons',
    CAPTION_MISSING: 'DescriptionMissing',
    INVALID_LIST_CONSISTENCY: 'InvalidListConsistency'
};
// key: paragraphStyle, value: paragraph
var previousParagraphsByStyle = {};
const previousParagraphsKeys = {
    HEADING: "Heading",
    NUMBERED_HEADING: "NumberedHeading",
    LIST_ITEM: "PreviousListItem",
    PREVIOUS_PARAGRAPH: "PreviousParagraph"
};
// key "previousParagraphsKeys". value: paragraph
var previousParagraphs = {};

window.consistencyConnector = {
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
    corectParagraph: async (idToCorrect, data) => {
        result = false;
        console.log("Correcting paragraph " + idToCorrect + " with data: ", data);
        await Word.run(async (context) => {
            var selection = context.document.getSelection();
            // Load the paragraph that contains the selection
            selection.paragraphs.load('uniqueLocalId, text, style, listItemOrNullObject');
            await context.sync();
            var paragraph = selection.paragraphs.items.find(para => para.uniqueLocalId === idToCorrect);
            if (paragraph === undefined) {
                console.log('Selection has changed, the id is not correct');
                result = false;
                return;
            }
            sourceParagraphText = paragraph.text;
            //console.log("Correcting paragraph: ", paragraph.text);
            data.forEach((element) => {
                switch (element) {
                    case consistencyErrorTypes.DOUBLE_SPACES:
                        sourceParagraphText = sourceParagraphText.replace(/ {2,}/g, ' ');
                        paragraph.insertText(sourceParagraphText, 'Replace');
                        break;
                    case consistencyErrorTypes.EMPTY_LINES:
                        console.log('Correcting empty lines');
                        // We are checking empty line only on not last paragraphs -> there will be always next paragraph
                        nextParagraph = paragraph.getNext();
                        paragraph.delete();
                        paragraph = nextParagraph;
                        break;
                    case consistencyErrorTypes.INVALID_CROSS_REFERENCE:
                        // Code for INVALID_CROSS_REFERENCE error type
                        console.log("Correcting invalid cross reference", sourceParagraphText);
                        var newText = paragraph.text;
                        crossReferenceError.forEach((error) => {
                            var regex = new RegExp(error, 'g');
                            newText = newText.replace(regex, "");
                        });
                        newText = newText.replace(/ {2,}/g, ' ');
                        console.log("Corrected text in paragraph: ", newText);
                        paragraph.insertText(newText, 'Replace');

                        // Cannot fix this issue automaticaly
                        break;
                    case consistencyErrorTypes.INVALID_HEADING_CONTINUITY:
                        // Code for INVALID_HEADING_CONTINUITY error type
                        // TODO - fix the heading + update previous numberedHeading
                        break;
                    case consistencyErrorTypes.INVALID_HEADING_CONSISTENCY:
                        // Code for INVALID_HEADING_CONSISTENCY error type
                        // TODO - fix the heading + update previous heading
                        break;
                    case consistencyErrorTypes.INVALID_HEADING_NUMBER_CONSISTENCY:
                        // Code for INVALID_HEADING_CONSISTENCY error type
                        // TODO - fix the heading + update previous heading
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
                    case consistencyErrorTypes.INVALID_LIST_CONSISTENCY:
                        var previousListItem = previousParagraphs[previousParagraphsKeys.LIST_ITEM];
                        if (previousListItem !== undefined && !paragraph.listItemOrNullObject.isNullObject) {
                            let previousListString = previousListItem.listItemOrNullObject.listString;
                            paragraph.listItemOrNullObject.listString = previousListString;
                        }
                        break;
                    case consistencyErrorTypes.INVALID_DOTS_COMAS_COLONS:
                        console.log("Correcting invalid dots, comas, colons", sourceParagraphText);
                        // Code for INVALID_DOTS_COMAS_COLONS error type
                        dotsComasColonsNoSpaceRegex.forEach((regex) => {
                            let regexWithG = new RegExp(regex.source, 'g');
                            //sourceParagraphText = sourceParagraphText.replace(regexWithG, ' ');
                            sourceParagraphText = sourceParagraphText.replace(/\.[^\s]/g, function (match) {
                                return match[0] + ' ' + match[1];
                            });
                            console.log("First correction", sourceParagraphText);

                            paragraph.insertText(sourceParagraphText, 'Replace');
                        });
                        dotsComasColonsSpaceRegex.forEach((regex) => {
                            let regexWithG = new RegExp(regex.source, 'g');
                            sourceParagraphText = sourceParagraphText.replace(regexWithG, function (match) {
                                return match.trim();
                            });
                            console.log("Second correction", sourceParagraphText);
                            paragraph.insertText(sourceParagraphText, 'Replace');
                        });
                        break;
                    case consistencyErrorTypes.INVALID_PARENTHESIS:
                        // Code for INVALID_PARENTHESIS error type
                        console.log("Correcting invalid parenthesis - THIS ERROR CANNOT BE CORRECTED AUTOMATICALLY");
                        break;
                    case consistencyErrorTypes.CAPTION_MISSING:
                        // Code for INVALID_PARENTHESIS error type
                        console.log("Correcting caption missing - THIS ERROR CANNOT BE CORRECTED AUTOMATICALLY");
                        break;
                    default:
                        // Code for default case
                        break;
                }
            });
            paragraph.select();
            await context.sync();
            //await saveSelectedParagraphAtCurrentIndex(consistencyParamsToLoad);
            result = true;
        });
        return result;
    }
}

function resetAtrributes() {
    previousParagraphsByStyle = {};
    previousParagraphs = {};
}

async function startConsistencyScan(start) {
    // list of found errors
    const errors = [];
    var paraId = undefined;
    var isErrorr = false;
    return Word.run(async (context) => {
        if (start) {
            console.log("Getting first paragraph");
            paragraph = context.document.body.paragraphs.getFirst();
            paragraph.load(consistencyParamsToLoad);
            await context.sync();
        }
        else {
            console.log("Getting selected paragraph");
            var selection = context.document.getSelection();
            // Load the paragraph that contains the selection
            selection.paragraphs.load(consistencyParamsToLoad);
            await context.sync();
            if (selection.paragraphs.items.length > 0) {
                paragraph = selection.paragraphs.items[0];
            }
            else {
                // TODO - check if the paragraph stayed selected - if it is not null
                console.log("Selection has changed, I cannot find any paragraph");
            }
        }

        while (paragraph !== null && !paragraph.isNullObject) {
            console.log("Checking: ", paragraph.text);
            console.log("Ignored paragraphs: ", dataService.ignoredParagraphs);
            console.log("This id", paragraph.uniqueLocalId, " is ignored: ", dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId));
            if (!dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId)) {
                const styleChecks = prepareChecks(paragraph);
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
            console.log("Saving this paragraph as previous, before going to the next one")
            UpdatePreviousParagraph(paragraph);
            console.log("Moving to next paragraph");
            paragraph = paragraph.getNextOrNullObject();
            paragraph.load(consistencyParamsToLoad);
            await context.sync();
            console.log("Next paragraph is null?", paragraph.isNullObject);
        }
        const ScanReturnValue = {
            FoundError: isErrorr,
            ParagraphId: paraId,
            ErrorTypes: errors
        };
        console.log(JSON.stringify(ScanReturnValue));
        return ScanReturnValue;
    });
}

function UpdatePreviousParagraph(paragraph) {
    if (!dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId)) {
        if (paragraph.text !== '') {
            previousParagraphsByStyle[paragraph.style] = paragraph;
            if (isParagraphHeading(paragraph)) {
                previousParagraphs[previousParagraphsKeys.HEADING] = paragraph;
                if (GetNumberOfHeading(paragraph) !== null) {
                    previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING] = paragraph;
                }
            }
            if (!paragraph.listItemOrNullObject.isNullObject) {
                previousParagraphs[previousParagraphsKeys.LIST_ITEM] = paragraph;
            }
        }
        previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH] = paragraph;
    }
}

function prepareChecks(paragraph) {
    console.log("Preparing checks: ", dataService.doubleSpaces);
    var wrongHeadingContinuity = false;
    var wrongHeadingConsistency = false;
    var wrongHeadingNumberConsistency = false;
    if (isParagraphHeading(paragraph)) {
        var currentheadingNumber = GetNumberOfHeading(paragraph);
        if (currentheadingNumber !== null) {
            // if paragraph is numbered heading, it has number -> we check heading continuity
            wrongHeadingContinuity = !checkHeadingContinuity(currentheadingNumber)
            wrongHeadingNumberConsistency = !checkHeadingNumberConsistency(currentheadingNumber);
        }
        wrongHeadingConsistency = !checkchHeadingConsistency(paragraph);
    }
    const styleChecks = [
        { condition: dataService.doubleSpaces && checkDoubleSpaces(paragraph), errorType: consistencyErrorTypes.DOUBLE_SPACES },
        { condition: dataService.emptyLines && isEmptyLine(paragraph), errorType: consistencyErrorTypes.EMPTY_LINES },
        { condition: dataService.crossReferenceFunctionality && checkInvalidCrossReference(paragraph), errorType: consistencyErrorTypes.INVALID_CROSS_REFERENCE },
        { condition: dataService.titleConsistency && wrongHeadingContinuity, errorType: consistencyErrorTypes.INVALID_HEADING_CONTINUITY },
        { condition: dataService.titleConsistency && wrongHeadingNumberConsistency, errorType: consistencyErrorTypes.INVALID_HEADING_NUMBER_CONSISTENCY },
        { condition: dataService.titleConsistency && wrongHeadingConsistency, errorType: consistencyErrorTypes.INVALID_HEADING_CONSISTENCY },
        { condition: dataService.documentAlignment && isDifferentFormatting(paragraph), errorType: consistencyErrorTypes.INCONSISTENT_FORMATTING },
        { condition: dataService.parenthesesValidation && !isValidParenthesis(paragraph), errorType: consistencyErrorTypes.INVALID_PARENTHESIS },
        { condition: dataService.dotsComasColonsValidation && !checkDotsComasColons(paragraph), errorType: consistencyErrorTypes.INVALID_DOTS_COMAS_COLONS },
        { condition: dataService.captionValidation && !isCaptionPresent(paragraph), errorType: consistencyErrorTypes.CAPTION_MISSING },
        { condition: dataService.listValidation && !isListConsistent(paragraph), errorType: consistencyErrorTypes.INVALID_LIST_CONSISTENCY },
        // Add more checks as needed
    ];
    return styleChecks;
}

function isParagraphHeading(paragraph) {
    return paragraph.style.includes('Heading') || paragraph.style.includes('Nadpis');
}

function GetNumberOfHeading(paragraph) {
    if (!paragraph.listItemOrNullObject.isNullObject) {
        // if paragraph is part of a list, we consider it as a heading
        return paragraph.listItemOrNullObject.listString;
    }
    else if (/^\d/.test(paragraph.text)) {
        // if it is not par of a list, but it starts with a number, we consider it as a heading
        return paragraph.text.split(" ")[0];
    }
    else {
        return null;
    }
}

async function refresshAllRefFields() {
    return Word.run(async (context) => {
        const fields = context.document.body.fields.load("items");
        await context.sync();
        fields.items.forEach(field => {
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
    var result = false;
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
    previousHeading = previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING];
    if (previousHeading !== undefined) {
        previousNumber = GetNumberOfHeading(previousHeading);
        continuingNumbers = GetNextPossibleNumbers(previousNumber);
        console.log('Possible next numbers: ', continuingNumbers);
        if (continuingNumbers.includes(currentNumber)) {
            return true;
        }
        else {
            return false;
        }
    }
    // if there is no previous heading, we cannot check the continuity -> this is the first heading
    return true;
}

function checkHeadingNumberConsistency(currentNumber) {
    console.log("Checking number consistency ... Current number: ", currentNumber);
    previousHeading = previousParagraphs[previousParagraphsKeys.NUMBERED_HEADING];
    if (previousHeading !== undefined) {
        previousNumber = GetNumberOfHeading(previousHeading);
        if ((previousNumber.endsWith('.') && currentNumber.endsWith('.')) || ((!previousNumber.endsWith('.') && !currentNumber.endsWith('.')))) {
            return true;
        }
        else {
            return false;
        }
    }
    // if there is no previous heading, we cannot check the continuity -> this is the first heading
    return true;
}
function checkchHeadingConsistency(paragraph) {
    previousHeading = previousParagraphs[previousParagraphsKeys.HEADING];
    if (previousHeading !== undefined) {
        previousHeader = previousHeading.text;
        currentHeader = paragraph.text;
        console.log("Checking heading consistency: ", previousHeader, currentHeader);
        result = true;
        // Check if the previous text is all uppercase
        if (previousHeader === previousHeader.toUpperCase()) {
            result = currentHeader === currentHeader.toUpperCase();
        }
        // Check if the previous text is all lowercase
        else if (previousHeader === previousHeader.toLowerCase()) {
            result = currentHeader === currentHeader.toLowerCase();
        }
        if (result) {
            previousHeading = paragraph;
        }
        return result;
    }
    return true;  
}

function isValidParenthesis(paragraph) {
    const text = paragraph.text;
    const stack = [];

    for (let i = 0; i < text.length; i++) {
        const char = text[i];
        if (char === '(' || char === '[' || char === '{' || char === '“' || char === '„') {
            stack.push(char);
            console.log("Pushed: ", char, "Stack: ", stack)
        } else if (char === ')' || char === ']' || char === '}' || char === '”') {
            if (stack.length === 0) {
                return false;
            }
            const top = stack.pop();
            console.log("Popped: ", top, "Stack: ", stack)
            if (
                (char === ')' && top !== '(') ||
                (char === ']' && top !== '[') ||
                (char === '}' && top !== '{') ||
                (char === '”' && (top !== '„' && top !== '“'))
            ) {
                return false;
            }
        }
    }
    return stack.length === 0;
}

function checkDotsComasColons(paragraph) {
    var text = paragraph.text;
    if (isParagraphHeading(paragraph)) {
        var headingNumber = GetNumberOfHeading(paragraph)
        if (headingNumber !== null) {
            text = text.slice(headingNumber.length);
        }
    }
    var result = dotsComasColonsSpaceRegex.every((regex) => {
        // testing every regex
        return !regex.test(text);
    });
    if (result) {
        // if first test passed, testign second array of regex
        result = dotsComasColonsNoSpaceRegex.every((regex) => {
            return !regex.test(text);
        });
    }
    return result;
}

function GetNextPossibleNumbers(previousNumber) {
    var endsWithDot = previousNumber.endsWith('.');
    if (endsWithDot) {
        previousNumber = previousNumber.slice(0, -1);
    }

    var parts = previousNumber.split('.');
    var results = [];
    console.log("Previous parts: ", parts);

    // Increment each part from the end
    for (var i = parts.length - 1; i >= 0; i--) {
        var newParts = parts.slice();
        newParts[i] = parseInt(newParts[i]) + 1;
        results.push(newParts.join('.'));
    }

    // Add the next major heading
    results.push(`${previousNumber}.1`);

    if (endsWithDot) {
        results = results.map(function (result) {
            return result + '.';
        });
    }
    return results;
}

function isEmptyLine(paragraph) {
    var previousPara = previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH];
    console.log("checking empty line, previous: ", previousPara, "current: ", paragraph);
    if (!paragraph.isLastParagraph && previousPara !== undefined && previousPara.text.trim() === '' && paragraph.text.trim() === '') {
        return true;
    }
    return false;
}

function isDifferentFormatting(paragraph) {
    var previousPara = previousParagraphsByStyle[paragraph.style];
    if (previousPara !== undefined) {
        // checking formatting with previous paragraph with same style
        console.log("Checking formatting: ", paragraph, previousPara);
        return paragraph.alignment !== previousPara.alignment ||
            paragraph.font.name !== previousPara.font.name ||
            paragraph.font.size !== previousPara.font.size;
    }
    return false;
}

function isCaptionPresent(currentParagraph) {
    previousPara = previousParagraphs[previousParagraphsKeys.PREVIOUS_PARAGRAPH];
    if (previousPara !== undefined) {
        console.log("Previous paragraph: ", previousPara, "This paragraph: ", currentParagraph)
        if (previousPara.tableNestingLevel > 0 && currentParagraph.tableNestingLevel === 0) {
            console.log("Previous paragraph is in table, this is not -> so this should be a caption");
            return currentParagraph.style.includes('Caption') || currentParagraph.style.includes('Popis');
        }
        else if (previousPara.inlinePictures.items.length > 0 && currentParagraph.inlinePictures.items.length === 0) {
            console.log('Previous paragraph was image, and this is not -> this should be caption');
            return currentParagraph.style.includes('Caption') || currentParagraph.style.includes('Popis');
        }
    }
    return true;
}

function isListConsistent(currentParagraph) {
    var previousListItem = previousParagraphs[previousParagraphsKeys.LIST_ITEM];
    if (previousListItem !== undefined && !currentParagraph.listItemOrNullObject.isNullObject) {
        let previousListString = previousListItem.listItemOrNullObject.listString;
        let currentListString = currentParagraph.listItemOrNullObject.listString;
        if ((/^\d/.test(previousListString) && /^\d/.test(currentListString)) || (previousListString === currentListString)) {
            return true;
        }
        else {
            return false;
        }
    }
    return true;
}


//////////////////////////////ARCHIVE//////////////////////////////////////

async function startConsistencyScanBAK() {
    // list of found errors
    const errors = [];
    var paraId = undefined;
    var isErrorr = false;
    for (var i = CURRENT_PARAGRAPG_INDEX; i < GLOBAL_PARAGRAPHS.items.length; i++) {
        paragraph = GLOBAL_PARAGRAPHS.items[i];
        CURRENT_PARAGRAPG_INDEX = i;
        console.log("Checking: ", paragraph.text);
        console.log("Ignored paragraphs: ", dataService.ignoredParagraphs);
        console.log("This id", paragraph.uniqueLocalId, " is ignored: ", dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId));
        if (dataService.ignoredParagraphs.includes(paragraph.uniqueLocalId)) {
            continue;
        }
        const styleChecks = prepareChecks(paragraph);
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
            await selectParagraph(CURRENT_PARAGRAPG_INDEX);
            console.log(errors);
            break;
        }
    }
    const ScanReturnValue = {
        FoundError: isErrorr,
        ParagraphId: paraId,
        ErrorTypes: errors
    }
    console.log(JSON.stringify(ScanReturnValue));
    return ScanReturnValue;
}

//function isValidContinuation(previous, current) {

//    const previousParts = previous.replace(/\.$/, '').split('.');
//    const currentParts = current.replace(/\.$/, '').split('.');
//    console.log("Previous parts: ", previousParts);
//    console.log("Current parts: ", currentParts);
//    // If the current heading has more parts than the previous one,
//    // it should only have one more part and that part should be '1'.
//    if (currentParts.length > previousParts.length) {
//        return currentParts.length === previousParts.length + 1 && currentParts[currentParts.length - 1] === '1';
//    }

//    // If the current heading has the same number of parts as the previous one,
//    // all parts should be the same as the previous one except for the last part,
//    // which should be one greater than the last part of the previous one.
//    else if (currentParts.length === previousParts.length) {
//        for (let i = 0; i < currentParts.length - 1; i++) {
//            if (currentParts[i] !== previousParts[i]) {
//                return false;
//            }
//        }
//        return parseInt(currentParts[currentParts.length - 1]) === parseInt(previousParts[previousParts.length - 1]) + 1;
//    }

//    // If the current heading has fewer parts than the previous one,
//    // the first parts should be the same and the last part of the current heading
//    // should be one greater than the corresponding part of the previous heading.
//    else if (currentParts.length < previousParts.length) {
//        for (let i = 0; i < currentParts.length - 1; i++) {
//            if (currentParts[i] !== previousParts[i]) {
//                return false;
//            }
//        }
//        return parseInt(currentParts[currentParts.length - 1]) === parseInt(previousParts[currentParts.length - 1]) + 1;
//    }
//    return false;
//}