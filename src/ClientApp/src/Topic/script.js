const usernameContainer = document.getElementById("topic-username");
const dateContainer = document.getElementById("creation-date");
const viewedContainer = document.getElementById("viewed");
const answersContainer = document.getElementById("answers");
const titleContainer = document.getElementById("topic-name");
const descriptionContainer = document.getElementById("topic-description");
const codeContainer = document.getElementById("panel-footer-code");
const commentsContainer = document.getElementById("comment-group");

/**
 * Adds data from topic to page
 * @param {Object} topic - topic data from server
 */
const addTopicToPage = (topic)=>{
    titleContainer.textContent = topic.header;
    descriptionContainer.textContent = topic.description;
    codeContainer.innerHTML = topic.code.replaceAll('\n','<br>');
    usernameContainer.textContent = topic.userLogin;
    /*let openCode = document.createElement("button");
    openCode.textContent = "Run code";
    openCode.onclick = ()=> openPage("../Html/code-runner.html", {"id": topicId});
    document.getElementById(codeId).appendChild(openCode);*/    
};

window.addEventListener("load", ()=>{
    getTopic(getValueFromCurrentUrl("id")).then(response => {
        addTopicToPage(response);
    })
    .catch(error => {
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
});