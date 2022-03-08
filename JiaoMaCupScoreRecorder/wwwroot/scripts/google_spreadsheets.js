/**
 *  On load, called to load the auth2 library and API client library.
 */
function handleClientLoad() {
    window.gapi.load("client:auth2", initClient);
}

/**
 *  Initializes the API client library and sets up sign-in state
 *  listeners.
 */
function getGoogleAuth() {
    return window.DotNet.invokeMethodAsync("JiaoMaCupScoreRecorder", "GetGoogleSheetsAuthConfig");
}

async function initClient() {
    const googleAuth = await getGoogleAuth();
    window.gapi.client.init(googleAuth).then(function() {
            // Listen for sign-in state changes.
            window.gapi.auth2.getAuthInstance().isSignedIn.listen(updateSigninStatus);
            // Handle the initial sign-in state.
            updateSigninStatus(window.gapi.auth2.getAuthInstance().isSignedIn.get());

        },
        function(error) {
            appendPre(JSON.stringify(error, null, 2));
        });
};

/**
 *  Called when the signed in status changes, to update the UI
 *  appropriately. After a sign-in, the API is called.
 */
function updateSigninStatus(isSignedIn) {
    if (isSignedIn) {
        console.log(isSignedIn);
    }
}

/**
 *  Sign in the user upon button click.
 */
async function handleAuthClick(event) {
    await window.gapi.auth2.getAuthInstance().signIn();
    const isSignedIn = await getSigninStatus();
    return isSignedIn;
}

/**
 *  Sign out the user upon button click.
 */
async function handleSignoutClick(event) {
    await window.gapi.auth2.getAuthInstance().signOut();
    const isSignedIn = await getSigninStatus();
    return isSignedIn;

}

/**
 * Append a pre element to the body containing the given message
 * as its text node. Used to display the results of the API call.
 *
 * @param {string} message Text to be placed in pre element.
 */
function appendPre(message) {
    const pre = document.getElementById("content");
    const textContent = document.createTextNode(message + "\n");
    pre.appendChild(textContent);
}

async function getSigninStatus() {
    const isSignedIn = await window.gapi.auth2.getAuthInstance().isSignedIn.get();
    console.log(`getSigninStatus: ${isSignedIn}`);
    return isSignedIn;
}

/**
 * Print the names and majors of students in a sample spreadsheet:
 * https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
 */
async function getSheetData(requestBodyJson) {
    const requestBody = JSON.parse(requestBodyJson);
    if (requestBody.majorDimension === null || requestBody.majorDimension === undefined) {
        requestBody.majorDimension = "ROWS";
    }
    const request = window.gapi.client.sheets.spreadsheets.values.get(requestBody);
    const result = await request.then((response) => {
            const result = response.result.values;
            return result;
        },
        (response) => {
            console.log(response.result.error.message);
        });
    return result;
}

async function batchUpdateSheetData(spreadsheetId, requestBodyJson) {
    const requestParams = {
        spreadsheetId: spreadsheetId
    };
    const requestBody = JSON.parse(requestBodyJson);
    const request = window.gapi.client.sheets.spreadsheets.batchUpdate(requestParams, requestBody);
    const result = await request.then((response) => {
            return response;
        },
        (response) => {
            return response;
        });
    return result.status;
}

async function batchUpdateValues(spreadsheetId, requestBodyJson) {
    const requestParams = {
        spreadsheetId: spreadsheetId
    };
    const requestBody = JSON.parse(requestBodyJson);
    const request = window.gapi.client.sheets.spreadsheets.values.batchUpdate(requestParams, requestBody);
    const result = await request.then((response) => {
            return response;
        },
        (response) => {
            return response;
        });
    return result.status;
}

async function checkSheetExist(spreadsheetId, requestBody) {
    const requestParams = {
        spreadsheetId: spreadsheetId
    };
    const request = window.gapi.client.sheets.spreadsheets.getByDataFilter(requestParams, requestBody);
    const result = await request.then((response) => {
            const result = response.status;
            return result;
        },
        (response) => {
            const result = response.status;
            return result;
        });
    return result;
}