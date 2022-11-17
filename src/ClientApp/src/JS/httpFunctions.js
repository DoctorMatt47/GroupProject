/**
 * 
 * @param {string} endpoint - path to server request
 * @param {*} request - request to send it to server
 * @returns promise to server answer data or error
 */
const sendAsync = async(endpoint, request)=>{
    const response = await fetch(endpoint, request);
    if (!response.ok) {
        const text = await response.text();
        console.log(text);
        throw new Error(text);
    }
    if(response.statusText === "No Content") return response.text();
    return response.json();
};