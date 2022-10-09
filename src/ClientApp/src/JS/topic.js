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
    document.getElementById("topic-body").value, 
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
            console.log(response)
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception)
        })
}
function getTopics(containerID, perPage, page){
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }
    sendAsync(URLS.TopicsOrderedByCreationTime + `?perPage=${perPage}&page=${page}`, request)
        .then(response => {
            console.log(response)
            let container = document.getElementById(containerID)
            for(let i in response.list){
                let title = document.createElement("h1")
                title.textContent = response.list[i].header
                container.appendChild(title)
                let body = document.createElement("p")
                body.textContent = response.list[i].description
                container.appendChild(body)
            }
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception)
        })
}