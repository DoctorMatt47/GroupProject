const usernameContainer = document.getElementById("topic-username");
const dateContainer = document.getElementById("creation-date");
const viewedContainer = document.getElementById("viewed");
const answersContainer = document.getElementById("answers");
const titleContainer = document.getElementById("topic-name");
const descriptionContainer = document.getElementById("topic-description");
const codeContainer = document.getElementById("panel-footer-code");
const commentsContainer = document.getElementById("comment-group");
const sectionContainer = document.getElementById("section");
const codeButton = document.getElementById("run-code");

/**
 * Adds data from topic to page
 * @param {Object} topic - topic data from server
 */
const addTopicToPage = (topic)=>{
    console.log(topic)
    titleContainer.textContent = topic.header;
    descriptionContainer.textContent = topic.description;
    codeContainer.innerHTML = topic.compileOptions.code.replaceAll('\n','<br>');
    usernameContainer.textContent = topic.userLogin;
    //sectionContainer.textContent = LANGUAGES[topic.compileOptions.language.toLowerCase()];
    sectionContainer.textContent = topic.sectionHeader;
    dateContainer.textContent = new Date(topic.creationTime).toLocaleDateString();
    codeButton.onclick = ()=> openPage("../Html/code-runner.html", {"id": topic.id});  
};

window.addEventListener("load", ()=>{
    getTopic(getValueFromCurrentUrl("id")).then(response => {
        addTopicToPage(response);
    })
    .catch(error => {
        console.log(error);
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
});


$(".close-btn").click(function(){
    $(this).prop('disabled',true)
});