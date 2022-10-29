/**
 * @param {string} section - section of topic
 * @param {string} header - header or title of topic
 * @param {string} description - description of topic
 * @param {string} code - code to add it to topic
 * @param {string} language - key of programing language from languages.js
 * @returns promise to response with new topic or error
 */
const createTopic= (section, header, description, code, language)=>{
    const body = {header: header, description: description, code:code};
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body),
    };
    return sendAsync(URLS.Topics, request);
};
/**
 * 
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page
 * @returns promise to response with list of topic data or error
 */
const getTopics = (perPage, page)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(URLS.TopicsOrderedByCreationTime + `?perPage=${perPage}&page=${page}`, request);
};
/**
 * 
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page  
 * @returns promise to response with list of recommended topics or error
 */
const getRecommendedTopics = (perPage, page)=>{
    return getTopics(perPage, page);
};
/**
 * 
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page
 * @returns promise to response with list of popular topics or error
 */
const getPopularTopics=(perPage, page)=>{
    return getTopics(perPage, page);
};
/**
 * 
 * @param {number} topicId - id of topic
 * @returns promise to response with topic data or error
 */
const getTopic = (topicId)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(URLS.Topics + `/${topicId}`, request);
}
