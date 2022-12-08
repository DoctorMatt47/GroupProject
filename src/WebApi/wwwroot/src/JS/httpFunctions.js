/**
 * 
 * @param {string} endpoint - path to server request
 * @param {*} request - request to send it to server
 * @returns promise to server answer data or error
 */
const sendAsync = async(endpoint, request)=>{
    const response = await fetch(endpoint, request);
    if (!response.ok) {
        const bodyText = await response.text();
        let text = response.statusText;
        if(bodyText.length > 0){
            text = bodyText;
        }
        else if(response.status == 403 ){
            text = JSON.stringify({"message":response.statusText, "howToFix":"Log in to an account with appropriate permissions", "howToPrevent":"Do not use this feature from an account without appropriate permissions"});
        }
        throw new Error(text);
    }
    if(response.statusText === "No Content") return response.text();
    return response.json();
};