
function openForm() {
    document.getElementById("fullscreen-container").style.display = "block";
}

function closeForm() {
    document.getElementById("fullscreen-container").style.display = "none";
}
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
function addTopicsToContainer(containerID, perPage, page){
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }
    sendAsync(URLS.TopicsOrderedByCreationTime + `?perPage=${perPage}&page=${page}`, request)
        .then(response => {
            let container = document.getElementById(containerID)
            for(let i in response.list){
                let topic = document.createElement("div")
                topic.className = "topics-style"
                let title = document.createElement("div")
                title.className = "topics-title"
                title.textContent = response.list[i].header
                title.onclick = ()=>openTopic(response.list[i].id)
                topic.appendChild(title)
                let login = document.createElement("div")
                login.className = "topics-login"
                login.textContent = response.list[i].userLogin
                topic.appendChild(login)
                container.appendChild(topic)
            }
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception)
        })
}
function openTopic(topicId){
    openPage("topic.html", {"id": topicId})
}
function addTopicToPage(topicId, titleId, descriptionId, codeId, commentsId){
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }
    sendAsync(URLS.Topics + `/${topicId}`, request)
        .then(response => {
            console.log(response)
            document.getElementById(titleId).textContent = `Title: ${response.header}`
            document.getElementById(descriptionId).textContent = `Description: ${response.description}`
            document.getElementById(codeId).textContent = `Code: ${response.code}`
            let openCode = document.createElement("button")
            openCode.textContent = "Run code"
            openCode.onclick = ()=> openPage("../code-runner.html", {"id": topicId})
            document.getElementById(codeId).appendChild(openCode)
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception)
        })
}