/**
 * @param {number} topicId - id of topic to add to it complain
 * @param {string} description - description of complain
 * @returns promise to response with new complain or error
 */
const createTopicComplaint = (topicId, description)=>{
    const body = {description: description};
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body),
    };
    return sendAsync(URLS.ComplaintTopicCreate + topicId, request);
};
/**
 * @param {number} commentId - id of comment to add to it complain
 * @param {string} description - description of complain
 * @returns promise to response with new complain or error
 */
const createCommentComplaint = (commentId, description)=>{
    const body = {description: description};
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body),
    };
    return sendAsync(URLS.ComplaintCommentCreate + commentId, request);
};
/**
 * 
 * @param {number} topicId - id of topic
 * @returns promise to response with complaint data or error
 */
const getTopicComplaint = (topicId)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.ComplaintByTopic + topicId, request);
}
/**
 * 
 * @param {number} commentId - id of comment
 * @returns promise to response with complaint data or error
 */
const getCommentComplaint = (commentId)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.ComplaintByComment + commentId, request);
}