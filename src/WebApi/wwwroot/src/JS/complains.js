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
    return sendAsync(URLS.ComplaintTopic + topicId, request);
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
    return sendAsync(URLS.ComplaintComment + commentId, request);
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
    return sendAsync(URLS.ComplaintTopic + topicId, request);
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
    return sendAsync(URLS.ComplaintComment + commentId, request);
}
/**
 * 
 * @param {number} complaintId - id of complaint
 * @returns promise to response with complaint data or error
 */
const getComplaint = (complaintId)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.Complaints + `/${complaintId}`, request);
}
/**
 * @param {number} perPage - count of pages in current page
 * @param {number} page - number of page  
 * @returns list of complaints
 */
const getComplaints = (perPage, page)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.Complaints + `?Number=${page}&Size=${perPage}`, request);
}
/**
 * Deletes complaint from database
 * @param {number} complaintId - id of complaint
 * @returns promise with answer
 */
const deleteComplaint = (complaintId)=>{
    const request = {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.Complaints + `/${complaintId}`, request);
}