var dataService = null;
const consistencyParamsToLoad = 'text, alignment, style, fields, listItemOrNullObject, uniqueLocalId';
dotsComasColonsSpaceRegex = [/ \./, / ,/, / :/];
dotsComasColonsNoSpaceRegex = [/\.[^\s]/,/,\S/, /:\S/];
const consistencyErrorTypes = {
    DOUBLE_SPACES: 'DoubleSpaces',
    EMPTY_LINES: 'EmptyLines',
    INVALID_CROSS_REFERENCE: 'InvalidCrossRef',
    INVALID_HEADING_CONTINUITY: 'InvalidHeadingContinuity',
    INVALID_HEADING_CONSISTENCY: 'InvalidHeadingConsistency',
    INCONSISTENT_FORMATTING: 'InconsistentFormatting',
    INVALID_PARENTHESIS: 'InvalidParenthesis',
    INVALID_DOTS_COMAS_COLONS: 'InvalidDotsComasColons'
}

// variable to store previous heading, for heading continuity check
var previousNumberedHeading = { text: null, number: null };
var previousHeading = null;

window.consistencyConnector = {
    checkConsistency: async (start, data) => {
        console.log(data);
        dataService = data;
        if (start) {
            // if we are starting the scan, we load all paragraphs and refresh all ref fields
            console.log("Starting consistency scan");
            resetAtrributes();
            await refresshAllRefFields();
            await getAllParagraphs(consistencyParamsToLoad);
            CURRENT_PARAGRAPG_INDEX = 0;
        }
        return await startConsistencyScan();
    },
    corectParagraph: async (idToCorrect, data) => {
        result = false;
        console.log("Correcting paragraph " + idToCorrect + " with data: ", data);
        if (GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX].uniqueLocalId !== idToCorrect) {
            console.log("Current paragraph is not the one we are looking for");
            // Current paragraph is not the one we are looking for
            // TODO - find the paragraph with the idToCorrect
            result = false;
            return;
        }
        await Word.run(async (context) => {
            var selection = context.document.getSelection();
            // Load the paragraph that contains the selection
            selection.paragraphs.load('uniqueLocalId');
            await context.sync();
            var paragraph = selection.paragraphs.items.find(para => para.uniqueLocalId === idToCorrect);
            if (paragraph === undefined) {
                console.log('Selection has changed, the id is not correct');
                result = false;
                return;
            }
            sourceParagraph = GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX];
            sourceParagraphText = sourceParagraph.text;
            //console.log("Correcting paragraph: ", paragraph.text);
            data.forEach((element) => {
                switch (element) {
                    case consistencyErrorTypes.DOUBLE_SPACES:
                        sourceParagraphText = sourceParagraphText.replace(/ {2,}/g, ' ');
                        paragraph.insertText(sourceParagraphText, 'Replace');
                        break;
                    case consistencyErrorTypes.EMPTY_LINES:
                        // Code for EMPTY_LINES error type
                        // TODO - remove this paragraph?? -> start check from begining, because the indexes will be broken
                        break;
                    case consistencyErrorTypes.INVALID_CROSS_REFERENCE:
                        // Code for INVALID_CROSS_REFERENCE error type
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
                    case consistencyErrorTypes.INCONSISTENT_FORMATTING:
                        // Code for INCONSISTENT_FORMATTING error type
                        paragraph.alignment = GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX - 1].alignment;
                        break;
                    case consistencyErrorTypes.INVALID_PARENTHESIS:
                        // Code for INVALID_PARENTHESIS error type
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
                    default:
                        // Code for default case
                        break;
                }
            });
            paragraph.select();
            await context.sync();

            await saveSelectedParagraphAtCurrentIndex(consistencyParamsToLoad);
            result = true;
        });
        return result;
    }
}

function resetAtrributes() {
    previousNumberedHeading = { text: null, number: null };
    previousHeading = null;
}

async function startConsistencyScan() {
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

function prepareChecks(paragraph) {
    console.log("Preparing checks: ", dataService.doubleSpaces);
    var wrongHeadingContinuity = false;
    wrongHeadingConsistency = false;
    var currentheadingNumber = GetNumberOfHeading(paragraph);
    if (currentheadingNumber !== null) {
        // if paragraph is numbered heading, it has number -> we check heading continuity and consistency
        wrongHeadingContinuity = !checkHeadingContinuity(currentheadingNumber, paragraph.text)
        wrongHeadingConsistency = !checkchHeadingConsistency(paragraph);
    }
    else if (paragraph.style.includes('Heading')) {
        // if paragraph is heading, but it is not numbered, we check only heading consistency
        wrongHeadingConsistency = !checkchHeadingConsistency(paragraph);
    }
    const styleChecks = [
        { condition: dataService.doubleSpaces && checkDoubleSpaces(paragraph), errorType: consistencyErrorTypes.DOUBLE_SPACES },
        { condition: dataService.emptyLines && isEmptyLine(paragraph), errorType: consistencyErrorTypes.EMPTY_LINES },
        { condition: dataService.crossReferenceFunctionality && checkInvalidCrossReference(paragraph), errorType: consistencyErrorTypes.INVALID_CROSS_REFERENCE },
        { condition: dataService.titleConsistency && wrongHeadingContinuity, errorType: consistencyErrorTypes.INVALID_HEADING_CONTINUITY },
        { condition: dataService.titleConsistency && wrongHeadingConsistency, errorType: consistencyErrorTypes.INVALID_HEADING_CONSISTENCY },
        { condition: dataService.documentAlignment && isDifferentFormatting(paragraph), errorType: consistencyErrorTypes.INCONSISTENT_FORMATTING },
        { condition: dataService.parenthesesValidation && !isValidParenthesis(paragraph), errorType: consistencyErrorTypes.INVALID_PARENTHESIS },
        { condition: dataService.dotsComasColonsValidation && !checkDotsComasColons(paragraph), errorType: consistencyErrorTypes.INVALID_DOTS_COMAS_COLONS },
        // Add more checks as needed
    ];
    return styleChecks;
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
    return paragraph.text.includes("Error! Reference source not found.");
}

function checkHeadingContinuity(currentNumber, currentText) {
    console.log("Checking heading continuity, previous heading: ", previousNumberedHeading.text, previousNumberedHeading.number);
    console.log("Checking continuity ... Current number: ", currentNumber);
    if (previousNumberedHeading.text === null) {
        console.log("Previous heading is undefined");
        // previous heading does not exists, this is the first one
        previousNumberedHeading.text = currentText;
        previousNumberedHeading.number = currentNumber;
        return true;
    }
    else {
        if (isValidContinuation(previousNumberedHeading.number, currentNumber)) {
            console.log("Heading has valid continuation", previousNumberedHeading.number, currentNumber);
            previousNumberedHeading.text = currentText;
            previousNumberedHeading.number = currentNumber;
            return true;
        }
        else {
            console.log("Heading has invalid continuation", previousNumberedHeading.number, currentNumber);
            return false;
        }
    }
}

function checkchHeadingConsistency(paragraph) {
    if (previousHeading === null) {
        console.log("Previous heading is undefined")
        // this is first heading
        previousHeading = paragraph;
        return true;
    }
    else {
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
        // Check if the previous text has the first letter uppercase and the rest lowercase
        else if (previousHeader === previousHeader.charAt(0).toUpperCase() + previousHeader.slice(1).toLowerCase()) {
            result = currentHeader === currentHeader.charAt(0).toUpperCase() + currentHeader.slice(1).toLowerCase();
        }
        else {
            result = false;
        }
        if (result) {
            previousHeading = paragraph;
        }
        return result;
    }    
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
    const text = paragraph.text;
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
    //if (/\.[^\s]/.test(text)) {
    //    console.log("There is a dot without leading space");
    //    // there is a dot without leading space
    //    return false;
    //}
    //if (/ \./.test(text)) {
    //    console.log("There is a dot with previous space");
    //    // there is a dot without leading space
    //    return false;
    //}
    //if (/,\S/.test(text)) {
    //    // there is a comma without leading space
    //    console.log("There is a comma without leading space");
    //    return false;
    //}
    //if (/ ,/.test(text)) {
    //    console.log("There is a comma with previous space");
    //    // there is a dot without leading space
    //    return false;
    //}
    //if (/:\S/.test(text)) {
    //    // there is a comma without leading space
    //    console.log("There is a colon without leading space");
    //    return false;
    //}
    //if (/ :/.test(text)) {
    //    console.log("There is a colon with previous space");
    //    // there is a dot without leading space
    //    return false;
    //}
    //return true;
}



function isValidContinuation(previous, current) {

    const previousParts = previous.replace(/\.$/, '').split('.');
    const currentParts = current.replace(/\.$/, '').split('.');
    console.log("Previous parts: ", previousParts);
    console.log("Current parts: ", currentParts);
    // If the current heading has more parts than the previous one,
    // it should only have one more part and that part should be '1'.
    if (currentParts.length > previousParts.length) {
        return currentParts.length === previousParts.length + 1 && currentParts[currentParts.length - 1] === '1';
    }

    // If the current heading has the same number of parts as the previous one,
    // all parts should be the same as the previous one except for the last part,
    // which should be one greater than the last part of the previous one.
    else if (currentParts.length === previousParts.length) {
        for (let i = 0; i < currentParts.length - 1; i++) {
            if (currentParts[i] !== previousParts[i]) {
                return false;
            }
        }
        return parseInt(currentParts[currentParts.length - 1]) === parseInt(previousParts[previousParts.length - 1]) + 1;
    }

    // If the current heading has fewer parts than the previous one,
    // the first parts should be the same and the last part of the current heading
    // should be one greater than the corresponding part of the previous heading.
    else if (currentParts.length < previousParts.length) {
        for (let i = 0; i < currentParts.length - 1; i++) {
            if (currentParts[i] !== previousParts[i]) {
                return false;
            }
        }
        return parseInt(currentParts[currentParts.length - 1]) === parseInt(previousParts[currentParts.length - 1]) + 1;
    }
    return false;
}

function isEmptyLine(paragraph) {
    if (CURRENT_PARAGRAPG_INDEX === 0) {
        return false;
    }
    if (paragraph.text.trim() === '' && GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX - 1].text.trim() === '') {
        return true;
    }
    else {
        return false;
    }
}

function isDifferentFormatting(paragraph) {
    if (CURRENT_PARAGRAPG_INDEX === 0) {
        return false;
    }
    if (paragraph.alignment !== GLOBAL_PARAGRAPHS.items[CURRENT_PARAGRAPG_INDEX - 1].alignment) {
        return true;
    }
    else {
        return false;
    }
}

