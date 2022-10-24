//Replace all functions to topic/script.js

function addLanguagesToSelect(selectId){
    let select = document.getElementById(selectId)
    for(let type in LANGUAGES){
        let option = document.createElement("option")
        option.value = type
        option.textContent = LANGUAGES[type]
        select.appendChild(option)
    }
}
function setVisible(visible, id, display){
    document.getElementById(id).style.display = visible?display:'none'
}
function submitTopic(){
    createTopic(document.getElementById("topic-title").value,
    document.getElementById("topic-description").value, 
    document.getElementById("need-code").checked
    ?document.getElementById("topic-code").value:"")
}
function createTopic(header, description, code){
    const body = {header: header, description: description, code:code}
    const request = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body),
    }
    sendAsync(URLS.Topics, request)
        .then(response => {
            openTopic(response.id)
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception)
        })
}
function getTopics(perPage, page){
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }
    return sendAsync(URLS.TopicsOrderedByCreationTime + `?perPage=${perPage}&page=${page}`, request)
}
function getRecommendedTopics(perPage, page){
    return getTopics(perPage, page)
}
function getPopularTopics(perPage, page){
    return getTopics(perPage, page)
}
function getTopic(topicId){
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }
    return sendAsync(URLS.Topics + `/${topicId}`, request)
}
function addTopicToPage(topicId, titleId, descriptionId, codeId, commentsId){
    getTopic(topicId).then(response => {
        console.log(response)
        document.getElementById(titleId).textContent = `Title: ${response.header}`
        document.getElementById(descriptionId).textContent = `Description: ${response.description}`
        document.getElementById(codeId).textContent = `Code: ${response.code}`
        let openCode = document.createElement("button")
        openCode.textContent = "Run code"
        openCode.onclick = ()=> openPage("../Html/code-runner.html", {"id": topicId})
        document.getElementById(codeId).appendChild(openCode)
    })
    .catch(error => {
        const exception = JSON.parse(error.message)
        console.log(exception)
    })
}