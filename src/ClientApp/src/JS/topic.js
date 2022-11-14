const TOPICS_SORTING = {
    time:"CreationTime",
    view:"ViewCount",
    complaint:"ComplaintCount",
    verify:"VerifyBefore"
};
/**
 * @param {number} sectionId - id of section of topic
 * @param {string} header - header or title of topic
 * @param {string} description - description of topic
 * @param {string} code - code to add it to topic
 * @param {string} language - key of programing language from languages.js
 * @returns promise to response with new topic or error
 */
const createTopic = (sectionId, header, description, code, language)=>{
    const body = {
        header: header, 
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
    return sendAsync(URLS.TopicsCreate + sectionId, request);
};
/**
 * 
 * @param {Object} params - parameters for topics getting
 * @returns promise to response with list of topic data or error
 */
const getTopics = (params)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(addParameters(URLS.Topics, params), request);
};
/**
 * 
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page  
 * @returns promise to response with list of recommended topics or error
 */
const getRecommendedTopics = (perPage, page)=>{
    return getTopics({"Page.Size":perPage,"Page.Number":page, "OrderBy":TOPICS_SORTING.time, "OnlyOpen":true});
};
/**
 * 
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page
 * @returns promise to response with list of popular topics or error
 */
const getPopularTopics=(perPage, page)=>{
    return getTopics({"Page.Size":perPage,"Page.Number":page, "OrderBy":TOPICS_SORTING.view, "OnlyOpen":true});
};
/**
 * @param {string} userId - id of user
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page
 * @returns promise to response with list of user topics or error
 */
const getUserTopics=(userId, perPage, page)=>{
    return getTopics({"Page.Size":perPage,"Page.Number":page, "OrderBy":TOPICS_SORTING.time, "OnlyOpen":false, "UserId":userId});
};
/**
 * @param {string} sectionId - id of section
 * @param {number} perPage - count of pages in current page
 * @param {number}  page - number of page
 * @returns promise to response with list of section topics or error
 */
const getSectionTopics=(sectionId, perPage, page)=>{
    return getTopics({"Page.Size":perPage,"Page.Number":page, "OrderBy":TOPICS_SORTING.time, "OnlyOpen":true, "SectionId":sectionId});
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
/**
 * 
 * @param {string} topicId - id of topic to close
 * @returns 
 */
const closeTopic = (topicId)=>{
    const request = {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        }
    };
    return sendAsync(URLS.Topics + `/${topicId}/Close`, request);
};