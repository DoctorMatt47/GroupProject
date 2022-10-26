//Replace all functions to topic/script.js

const addLanguagesToSelect = (selectId)=>{
    let select = document.getElementById(selectId)
    for(let type in LANGUAGES){
        let option = document.createElement("option")
        option.value = type
        option.textContent = LANGUAGES[type]
        select.appendChild(option)
    }
}
const setVisible = (visible, id, display)=>{
    document.getElementById(id).style.display = visible?display:'none'
}
const submitTopic = ()=>{
    createTopic(document.getElementById("topic-title").value,
    document.getElementById("topic-description").value, 
    document.getElementById("need-code").checked
    ?document.getElementById("topic-code").value:"").then(response=>{
        openPage("topic.html", {"id":response.id})
    }).catch(error=>{
        console.log(error.message)
    })
}
const createTopic= (header, description, code)=>{
    const body = {header: header, description: description, code:code}
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body),
    }
    return sendAsync(URLS.Topics, request)
}
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
