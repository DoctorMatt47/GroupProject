const COMMENTS_SORTING = {
    time:"CreationTime",
    complaint:"ComplaintCount",
    verify:"VerifyBefore"
};
/**
 * 
 * @param {object} params - parameters for comments getting
 * @returns promise to response with list of comments in topic or error
 */
const getComments = (params)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(addParameters(URLS.Comments, params), request);
};
/**
 * @param {string} topicId - id of topic
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page  
 * @returns promise to response with list of comments in topic or error
 */
const getTopicComments = (topicId, perPage, page)=>{
    return getComments({"Page.Size":perPage,"Page.Number":page, "OrderBy":COMMENTS_SORTING.time, "TopicId":topicId});
};
/**
 * @param {string} userId - id of user
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page  
 * @returns promise to response with list of comments in topic or error
 */
const getUserComments = (userId, perPage, page)=>{
    return getComments({"Page.Size":perPage,"Page.Number":page, "OrderBy":COMMENTS_SORTING.time, "UserId":userId});
};
/**
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page  
 * @returns promise to response with list of comments to verify or error
 */
const getVerifyComments = (perPage, page) =>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(addParameters(URLS.Comments, 
        {"Page.Size":perPage,"Page.Number":page, "OrderBy":COMMENTS_SORTING.verify}), request);
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
/**
 * 
 * @param {string} commentId - id of comment
 * @returns promise to response with the comment data or error
 */
const getComment = (commentId)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(URLS.Comments + `/${commentId}`, request);
};
/**
 * Deletes comment from database
 * @param {string} commentId - id of comment
 * @returns promise with answer
 */
const deleteComment = (commentId)=>{
    const request = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.Comments + `/${commentId}`, request);
};
/**
 * 
 * @param {string} commentId - id of comment to verify
 * @returns promise with answer
 */
const verifyComment = (commentId)=>{
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.Comments + `/${commentId}/Verify`, request);
};