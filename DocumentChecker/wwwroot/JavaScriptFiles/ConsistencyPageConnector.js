﻿window.consistencyConnector = {
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
    },
    checkConsistency: async () => {
        await refresshAllRefFields();
        const errors = [];
        let previousHeading = { text: undefined, number: undefined };
        // Define checks
        return Word.run(async (context) => {
            const paragraphs = context.document.body.paragraphs;
            paragraphs.load("text, font, alignment, lineSpacing, style, fields, listItemOrNullObject");

            await context.sync();
            paragraphs.items.every((paragraph) => {
                console.log("Checking: ", paragraph.text);
                const styleChecks = [
                    { condition: checkDoubleSpaces(paragraph), errorType: "Double spaces" },
                    { condition: checkInvalidCrossReference(paragraph), errorType: "Invalid Cross ref" },
                    { condition: !checkHeadingContinuity(paragraph, previousHeading), errorType: "Invalid Heading Continuity" },
                    { condition: !isValidParenthesis(paragraph), errorType: "Invalid Parenthesis" },
                    { condition: !checkSentenceFormat(paragraph), errorType: "Invalid Sentence Format" },
                    // Add more checks as needed
                ];

                // Iterate over checks
                styleChecks.forEach((check) => {
                    if (check.condition) {
                        errors.push(check.errorType);
                    }
                });
                if (errors.length > 0) {
                    console.log(
                        "This paragraph is wrong: ",
                        paragraph.text,
                        paragraph.style,
                        paragraph.font.size,
                        paragraph.font.name,
                        paragraph.alignment,
                        paragraph.lineSpacing
                    );
                    paragraph.select();
                    console.log(errors);
                    return false;
                }
                return true;
            });
            await context.sync();
        });
    },
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
        if (char === '(' || char === '[' || char === '{') {
            stack.push(char);
        } else if (char === ')' || char === ']' || char === '}') {
            if (stack.length === 0) {
                return false;
            }
            const top = stack.pop();
            if (
                (char === ')' && top !== '(') ||
                (char === ']' && top !== '[') ||
                (char === '}' && top !== '{')
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

