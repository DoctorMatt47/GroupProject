/**
 * 
 * @returns promise to array with sections or error
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
 * @param {string} header - section title
 * @param {string} description - section description
 * @returns promise to section id or error
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
/**
 * TODO: make with request
 * Update old section
 * @param {string} sectionId - id of section
 * @param {string} header - section title
 * @param {string} description - section description
 * @returns promise to section id or error
 */
const updateSection = (sectionId, header, description)=>{
    return getSections();
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
/**
 * TODO: make with request
 * Deletes section
 * @param {string} sectionId - id of section
 * @returns promise to answer
 */
const deleteSection = (sectionId)=>{
    return getSections();
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