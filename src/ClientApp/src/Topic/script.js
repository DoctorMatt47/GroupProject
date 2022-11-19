const usernameContainer = document.getElementById("topic-username");
const dateContainer = document.getElementById("creation-date");
const viewedContainer = document.getElementById("viewed");
const answersContainer = document.getElementById("answers");
const titleContainer = document.getElementById("topic-name");
const descriptionContainer = document.getElementById("topic-description");
const sectionContainer = document.getElementById("section");
const allCodeContainer = document.getElementById("all-code");
const codeContainer = document.getElementById("user-code");
const codeLanguageContainer = document.getElementById("topic-language");
const codeButton = document.getElementById("run-code");
const commentsContainer = document.getElementById("comment-group");
const closeTopicButton = document.getElementById("close-btn");
const addCommentButton = document.getElementById("comment-open-button");
const complaintButton = document.getElementById("topic-complaint-button");
const trashButton = document.getElementById("topic-admin-button");

/**
 * 
 * @param {Object} comment 
 * @returns DOM object with items to display code of comment
 */
const createCommentCode = (comment)=>{
    const panelFooter = document.createElement("div");
    panelFooter.className = "panel-footer"

    const panelFooterCode = document.createElement("div");
    panelFooterCode.id = "panel-footer-answer-code";
    panelFooter.appendChild(panelFooterCode);

    const language = document.createElement("div");
    language.id = "answer-language";
    language.className = "language";
    language.textContent = LANGUAGES[comment.compileOptions.language.toLowerCase()];
    panelFooterCode.appendChild(language);

    const code = document.createElement("div");
    code.textContent = comment.compileOptions.code;
    code.style="white-space: pre-wrap;"
    panelFooterCode.appendChild(code);

    const buttonContainer = document.createElement("div");
    buttonContainer.style = "width: 100%;";
    panelFooter.appendChild(buttonContainer);

    const run = document.createElement("button");
    run.className = "code-btn";
    run.onclick = ()=> openCompiler(comment.compileOptions.code, comment.compileOptions.language);
    run.textContent = "Run";
    buttonContainer.appendChild(run);

    return panelFooter;
};
/**
 * Creates comment object and adds comment data to comment list in page 
 * @param {Object} comment 
 */
const createCommentObject = (role, comment, isNew = false)=>{
    const com = document.createElement("div");
    com.id = "comment";
    const complaint = document.createElement("div");
    let icon = `<button data-toggle="tooltip" title="Create a complaint" onclick="openComplainForm('comment', '${comment.id}'); return false;">
                <span class="glyphicon glyphicon-comment" style="color:#d7ae54; margin-right: 15px"></span></button>`;
    if(role ==="Admin"){
        icon = `<button data-toggle="tooltip" title="Delete">
        <span class="glyphicon glyphicon-trash trash-span"></span></button>`;
    }
    complaint.innerHTML =`<div class="complaint-icon" style="margin-top: 10px;">${icon}</div>` 
    com.appendChild(complaint);

    const answer = document.createElement("p");
    answer.id = "answer";
    answer.textContent = comment.description;
    com.appendChild(answer);

    if(comment.compileOptions.code !== ""){
        com.appendChild(createCommentCode(comment));
    }

    const username = document.createElement("div");
    username.id = "answer-username";
    username.textContent = comment.userLogin;
    com.appendChild(username);

    const date = document.createElement("p");
    date.id = "answer-date";
    date.textContent = "Answered: " + new Date(comment.creationTime).toLocaleDateString();
    com.appendChild(date);

    if(isNew){
        commentsContainer.insertBefore(com, commentsContainer.firstChild);
        answersContainer.textContent = Number(answersContainer.textContent) + 1;
        return;
    }
    commentsContainer.appendChild(com);
};
const commentsPerPage = 10;
let currentPage = 1;
/**
 * Adds comments of topic to topic page
 * @param {string} topicId - id of topic
 */
const addCommentsToPage = (role, topicId) =>{
    getTopicComments(topicId, commentsPerPage, currentPage++).then(response=>{
        answersContainer.textContent = response.itemsCount;
        for(let i in response.items){
            createCommentObject(role, response.items[i]);
        }
    });
};
/**
 * Changes page elements if topic is closed
 * @param {object} topic - data of topic
 */
const topicClosed = (topic)=>{
    if(topic.isClosed){
        closeTopicButton.style = "background:gray";
        closeTopicButton.textContent = "Closed";
        addCommentButton.style = "display:none";
    }
}
/**
 * Adds data from topic to page
 * @param {Object} topic - topic data from server
 */
const addTopicToPage = (role, topic)=>{
    if(role ==="Admin"){
        complaintButton.style = "display:none;";
    }else{
        trashButton.style = "display:none;";
    }
    topicClosed(topic);
    titleContainer.textContent = topic.header;
    descriptionContainer.textContent = topic.description;
    usernameContainer.textContent = topic.userLogin;
    codeLanguageContainer.textContent = LANGUAGES[topic.compileOptions.language.toLowerCase()];
    sectionContainer.textContent = topic.sectionHeader;
    sectionContainer.onclick = ()=>{
        openPage("../Topics_in_section/topics_in_section.html", {id:topic.sectionId});
    };
    dateContainer.textContent = new Date(topic.creationTime).toLocaleDateString();
    viewedContainer.textContent = topic.viewCount;
    let code = topic.compileOptions.code;
    if(code !== ""){
        codeContainer.textContent = code;
        codeButton.onclick = ()=> openCompiler(topic.compileOptions.code, topic.compileOptions.language);
    }else{
        allCodeContainer.style.display = "none";
    }
};

const closeCurrentTopic = ()=>{
    closeTopic(getValueFromCurrentUrl('id')).then(()=>{
        openMessageWindow("Closed!");
    }).catch(showError);
};

window.addEventListener("load", ()=>{
    const role = getFromStorage("role");
    if(role !== "User"){
        addCommentButton.style = "display:none;";
    }
    getTopic(getValueFromCurrentUrl("id")).then(response => {
        addTopicToPage(role, response);
        commentsContainer.innerHTML = "";
        addCommentsToPage(role, response.id);
        document.getElementById("close-btn").style.display = response.userLogin === getFromStorage("login")?"block":"none";
    })
    .catch(showError);
});


$(".close-btn").click(function(){
    $(this).prop('disabled',true)
});
const openVerificationWindow = () => {
    document.getElementById("verificationWindow").style.display = "block";
};
const closeVerificationWindow = () => {
    document.getElementById("verificationWindow").style.display = "none";
};