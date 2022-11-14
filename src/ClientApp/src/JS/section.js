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
/**
 * Creates new section
 * @returns response to section id or error
 */
const createSection = (header, description)=>{
    const body = {
        header: header, 
        description: description, 
    };
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body),
    };
    return sendAsync(URLS.Section, request);
}