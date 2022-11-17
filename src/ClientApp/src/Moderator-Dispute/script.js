
/**
 * Adds topic or comment data to page
 * @param {object} data - object with topic or comment data
 */
const loadComplaintObject = (data)=>{
    loadComplaintUser(data.userId);
    const sectionContainer = document.getElementById("section");
    const titleContainer = document.getElementById("topic-name");
    const descriptionContainer = document.getElementById("topic-description-complaint");
    const codeContainer = document.getElementById("all-code");
    const codeTextContainer = document.getElementById("user-code");
    const codeLanguageContainer = document.getElementById("topic-language");
    const codeButton = document.getElementById("run-code");

    titleContainer.textContent = "header" in data ? data.header: "Commentary";
    descriptionContainer.textContent = data.description;

    if("sectionHeader" in data){
        sectionContainer.textContent = data.sectionHeader;
    }else{
        sectionContainer.style = "display:none";
    }
    
    let code = data.compileOptions.code;
    if(code !== ""){
        codeLanguageContainer.textContent = LANGUAGES[data.compileOptions.language.toLowerCase()];
        codeTextContainer.textContent = code;
        codeButton.onclick = ()=> openCompiler(data.compileOptions.code, data.compileOptions.language);
    }else{
        codeContainer.style.display = "none";
    }
}
/**
 * Adds user data to page
 * @param {string} userId - id of user
 */
const loadComplaintUser = (userId)=>{
    getUser(userId).then((user)=>{
        const usernameContainer = document.getElementById("username");
        const creationTimeContainer = document.getElementById("registration-date");
        const warningCountContainer = document.getElementById("preventions");
        const createdTopicsContainer = document.getElementById("created-discussions");
        const badTimeContainer = document.getElementById("last-block");
    
        usernameContainer.textContent = user.login;
        creationTimeContainer.textContent = new Date(user.creationTime).toLocaleDateString();
        warningCountContainer.textContent = user.warningCount;
        const date = new Date(user.banEndTime).toLocaleDateString();
        if(date === "01.01.1"){
            badTimeContainer.textContent = "Last block date: No ban";
        }   
        else badTimeContainer.textContent =  "Last block date: " + date;
        getUserTopics(userId, 1, 1).then((result) => {
            createdTopicsContainer.textContent =result.itemsCount;
        }).catch(showError);
    }).catch(showError);

}
/**
 * Adds user data to page
 * @param {string} userId - id of user
 */
const loadComplaintAuthor = (complaint)=>{
    getUser(complaint.userId).then((user)=>{
        const usernameContainer = document.getElementById("complainer-username");
        const complaintText = document.getElementById("complaint-text");
        usernameContainer.textContent = user.login;
        complaintText.textContent = complaint.description;
    }).catch(showError);

}
const loadComplaintData = (complaint)=>{
    const original = document.getElementById("original-button");
    let getFunction = getTopic;
    let originalUrl = "../Topic/topic.html?id="+complaint.targetId;
    if(complaint.target === "Commentary"){
        getFunction = getComment;
    } 
    getFunction(complaint.targetId).then((response)=>{
        if(complaint.target === "Commentary"){
            originalUrl = "../Topic/topic.html?id="+response.topicId;
        }
        original.href = originalUrl;
        loadComplaintObject(response);
        loadModeratorPanel(complaint, response.userId);
        loadComplaintAuthor(complaint);
    }).catch(showError);
};
const loadModeratorPanel = (complaint, userId)=>{
    const authorBlockButton = document.getElementById("author-block-button");
    const userBlockButton = document.getElementById("open-block-button");
    const endButton = document.getElementById("end-button");
    const deleteButton = document.getElementById("delete-button");

    authorBlockButton.onclick = ()=>{
        openBlockVerificationWindow(()=>{
            blockUser(complaint.userId).then(()=>{
                openMessageWindow("Blocked");
            }).catch(showError);
        });
    };

    userBlockButton.onclick = ()=>{
        openBlockVerificationWindow(()=>{
            blockUser(userId).then(()=>{
                openMessageWindow("Blocked");
            }).catch(showError);
        });
    };

    endButton.onclick = ()=>{
        deleteComplaint(complaint.id).then(()=>{
            openPage('../Moderator-Account/moderator-account.html');
        }).catch(showError);
    };

    deleteButton.onclick = ()=>{
        let deleteFunction = deleteTopic;
        if(complaint.target === "Commentary"){
            deleteFunction = deleteComment;
        }
        deleteFunction(complaint.targetId).then(() => {//also deletes complaint
            warningUser(userId).then(()=>{
                openPage('../Moderator-Account/moderator-account.html');
            }).catch(showError);
        }).catch(showError);
    };
};
window.addEventListener("load", ()=>{
    getComplaint(getValueFromCurrentUrl("id")).then(loadComplaintData).catch(showError);
});