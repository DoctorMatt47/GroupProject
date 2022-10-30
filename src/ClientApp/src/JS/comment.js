/**
 * 
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page  
 * @returns promise to response with list of comments in topic or error
 */
const getComments = (topicId, perPage, page)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(URLS.CommentsByTopic + `${topicId}?perPage=${perPage}&page=${page}`, request);
};
/**
 * @param {number} topicId - id of topic
 * @param {string} description - description of comment
 * @param {string} code - code to add it to comment
 * @param {string} language - key of programing language from languages.js
 * @returns promise to response with new comment or error
 */
const createComment = (topicId, description, code, language)=>{
    const body = {
        description: description, 
        compileOptions:{
            code:code,
            language: language
    }};
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body),
    };
    return sendAsync(URLS.CommentsCreate + topicId, request);
};