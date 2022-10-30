function addCode(topicId, codePlaceId){
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }
    sendAsync(URLS.Topics + `/${topicId}`, request)
    .then(response => {
        console.log(response)
        let place = document.getElementById(codePlaceId)
        place.appendChild(createCode(response.compileOptions.code, response.compileOptions.language.toLowerCase()))
    })
    .catch(error => {
        console.log(error)
        const exception = JSON.parse(error.message)
        console.log(exception)
    })
}
function createCode(code, language){
    let container = document.createElement("div")
    let div = document.createElement("div")
    div.setAttribute("data-pym-src", "https://www.jdoodle.com/plugin")
    div.setAttribute("data-language", language)
    div.textContent = code
    container.appendChild(div)
    let script = document.createElement("script")
    script.src = "https://www.jdoodle.com/assets/jdoodle-pym.min.js"
    script.type = "text/javascript"
    container.appendChild(script)
    return container
}