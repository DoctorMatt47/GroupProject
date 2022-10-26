const usernameContainer = document.getElementById("topic-username");
const dateContainer = document.getElementById("creation-date");
const viewedContainer = document.getElementById("viewed");
const answersContainer = document.getElementById("answers");
const titleContainer = document.getElementById("topic-name");
const descriptionContainer = document.getElementById("topic-description");
const codeContainer = document.getElementById("panel-footer-code");
const commentsContainer = document.getElementById("comment-group");

const addTopicToPage = (topicId)=>{
    getTopic(topicId).then(response => {
        titleContainer.textContent = response.header;
        descriptionContainer.textContent = response.description;
        codeContainer.innerHTML = response.code.replaceAll('\n','<br>');
        usernameContainer.textContent = response.userLogin;
        /*let openCode = document.createElement("button");
        openCode.textContent = "Run code";
        openCode.onclick = ()=> openPage("../Html/code-runner.html", {"id": topicId});
        document.getElementById(codeId).appendChild(openCode);*/
    })
    .catch(error => {
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
};

window.addEventListener("load", ()=>{
    addTopicToPage(getValueFromCurrentUrl("id"))
})