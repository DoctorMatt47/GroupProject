/**
 * 
 * @returns response to array with sections or error
 */
const getSections = ()=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(URLS.Section, request);
}