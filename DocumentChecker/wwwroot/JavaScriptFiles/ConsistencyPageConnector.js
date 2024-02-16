// variable to store previous heading, for heading continuity check
var previousHeading = { text: undefined, number: undefined };
var dataService = undefined;

window.consistencyConnector = {
    checkConsistency: async (start, data) => {
        console.log(data);        
        if (start) {
            // if we are starting the scan, we load all paragraphs and refresh all ref fields
            console.log("Starting consistency scan");
            dataService = data;
            previousHeading = { text: undefined, number: undefined };
            await refresshAllRefFields();
            await getAllParagraphs("text, font, alignment, lineSpacing, style, fields, listItemOrNullObject");
            CURRENT_PARAGRAPG_INDEX = 0;
        }
        else {
            // if we are continuing with scan, we just continue from the last paragraph
            if (CURRENT_PARAGRAPG_INDEX == GLOBAL_PARAGRAPHS.items.length - 1) {
                // if current paragraph was the last one, we cannot continue
                return;
            }
            // we increase the paragraph index to continue from the next one
            CURRENT_PARAGRAPG_INDEX++;
        }
        await startConsistencyScan();
    }
}

async function startConsistencyScan() {
    // list of found errors
    const errors = [];
    for (var i = CURRENT_PARAGRAPG_INDEX; i < GLOBAL_PARAGRAPHS.items.length; i++) {
        paragraph = GLOBAL_PARAGRAPHS.items[i];
        CURRENT_PARAGRAPG_INDEX = i;
        console.log("Checking: ", paragraph.text);
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
            await selectParagraph(CURRENT_PARAGRAPG_INDEX);
            console.log(errors);
            return false;
        }
    }
    return true;
}

function prepareChecks(paragraph) {
    console.log("Preparing checks: ", dataService.doubleSpaces);
    console.log(typeof dataService.doubleSpaces);
    const styleChecks = [
        { condition: dataService.doubleSpaces && checkDoubleSpaces(paragraph), errorType: "DoubleSpaces" },
        { condition: dataService.crossReferenceFunctionality && checkInvalidCrossReference(paragraph), errorType: "InvalidCrossRef" },
        { condition: dataService.titleConsistency && !checkHeadingContinuity(paragraph, previousHeading), errorType: "InvalidHeadingContinuity" },
        { condition: dataService.parenthesesValidation && !isValidParenthesis(paragraph), errorType: "InvalidParenthesis" },
        { condition: dataService.dotsComasColonsValidation && !checkSentenceFormat(paragraph), errorType: "InvalidDotsComasColons" },
        // Add more checks as needed
    ];
    return styleChecks;
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
    return paragraph.text.includes("  ");
}

function checkInvalidCrossReference(paragraph) {
    return paragraph.text.includes("Error! Reference source not found.");
}

function checkHeadingContinuity(paragraph, previousHeading) {
    // TODO: kontrola ci nadpis pred tym mal rovnake formatovanie .. tzn (velke pismeno na zaciatku, bodka na konci)
    // ak je nadpis prvej urovne .. je na novej strane??
    console.log("Checking heading continuity, previous heading: ", previousHeading.text, previousHeading.number);
    let currentNumber = undefined;
    if (!paragraph.listItemOrNullObject.isNullObject) {
        currentNumber = paragraph.listItemOrNullObject.listString;
    }
    else if (/^\d/.test(paragraph.text)) {
        currentNumber = paragraph.text.split(" ")[0];
    }
    if (currentNumber !== undefined) {
        console.log("Checking continuity ... Current number: ", currentNumber);
        if (previousHeading.text === undefined) {
            console.log("Previous heading is undefined");
            // previous heading does not exists, this is the first one
            previousHeading.text = paragraph.text;
            previousHeading.number = currentNumber;
            return true;
        }
        else {
            if (isValidContinuation(previousHeading.number, currentNumber)) {
                console.log("Heading has valid continuation", previousHeading.number, currentNumber);
                previousHeading.text = paragraph.text;
                previousHeading.number = currentNumber;
                return true;
            }
            else {
                console.log("Heading has invalid continuation", previousHeading.number, currentNumber);
                return false;
            }
        }
    }
    else {
        console.log("This is not a heading");
        return true;
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

function checkSentenceFormat(paragraph) {
    const text = paragraph.text;
    if (/\.[^\s]/.test(text)) {
        console.log("There is a dot without leading space");
        // there is a dot without leading space
        return false;
    }
    if (/ \./.test(text)) {
        console.log("There is a dot with previous space");
        // there is a dot without leading space
        return false;
    }
    if (/,\S/.test(text)) {
        // there is a comma without leading space
        console.log("There is a comma without leading space");
        return false;
    }
    if (/ ,/.test(text)) {
        console.log("There is a comma with previous space");
        // there is a dot without leading space
        return false;
    }
    return true;
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

