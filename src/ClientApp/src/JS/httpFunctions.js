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
        throw new Error(text);
    }
    return response.json();
};