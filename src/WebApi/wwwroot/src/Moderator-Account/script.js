const openUserActionWindow = () => {
    document.getElementById("Unblock").style.display = "block";
};
const closeUserActionWindow = () => {
    document.getElementById("Unblock").style.display = "none";
};
const openModeratorActionWindow = () => {
    document.getElementById("Unblock-moderator").style.display = "block";
};
const closeModeratorActionWindow = () => {
    document.getElementById("Unblock-moderator").style.display = "none";
};
/**
 * Creates complaint html object for page
 * @param {object} complaint - object with complaint data
 */
const createComplaintObject = (complaint)=>{
    const div = document.createElement("div");
    div.innerHTML = `<a href="${addParameters('../Moderator-Dispute/moderator-dispute.html', {id:complaint.id, type:"Complaint"})}">
                        <div class="block">
                            <div class="title ${complaint.target === "Topic"?"topic":"comment"}">
                                <h4><strong>${textCutter(complaint.target, 7)}</strong></h4>
                            </div>
                            <div class="info">
                                <p>${textCutter(complaint.description, 20) + "..."}</p>
                                <p>Creation date:${new Date(complaint.creationTime).toLocaleDateString()}</p>
                            </div>
                        </div>
                    </a>`
    div.className = "col-sm-3";
    return div;
}
/**
 * Creates topic or comment html object for page
 * @param {string} type - 'topic' or 'comment'
 * @param {Object} obj - topic or comment data
 * @param {Array} words - array with bad words
 * @returns html object
 */
const createVerifyObject = (type, obj, words)=>{
    let wordsStr = "Found phrases: ";
    words.forEach(item=>wordsStr += item + ", ");
    wordsStr = wordsStr.slice(0, -2);
    const div = document.createElement("div");
    div.innerHTML = `<a href="${addParameters('../Moderator-Dispute/moderator-dispute.html', {id:obj.id, type:"Verify"+type})}">
                        <div class="block">
                            <div class="title ${type === "Topic"?"topic":"comment"}">
                                <h4><strong>${type}</strong></h4>
                            </div>
                            <div class="info">
                                <p style="color:red; font-weight:bold;">${textCutter(wordsStr, 20) + "..."}</p>
                                <p>Creation date:${new Date(obj.creationTime).toLocaleDateString()}</p>
                            </div>
                        </div>
                    </a>`
    div.className = "col-sm-3";
    return div;
}

const perPage = 4;

const loadMoreComplaints = document.getElementById("load-more-complaints");
let complaintPage = 1;

/**
 * Adds complaint topics and comments to page
 * @param {Object} container - container for complaints
 */
const loadComplains = (container)=>{
    getComplaints(perPage, complaintPage++).then((response) => {
        if(response.items.length < perPage){
            loadMoreComplaints.style = "display: none";
        }
        for(let i in response.items){
            container.appendChild(createComplaintObject(response.items[i]));
        }
    }).catch(showError);
}

const loadMoreVerifies = document.getElementById("load-more-verifies");
let verifyPage = 1;

/**
 * Adds topics and comments to page
 * @param {Object} container - container for topics and comments
 * @param {Array} words - array with bad words
 */
const loadVerifies = (container, words)=>{
    let loadMoreTopics = true, loadMoreComments = true;

    getVerifyTopics(perPage, verifyPage).then((response) => {
        if(response.items.length < perPage){
            loadMoreTopics = false;
            if(!loadMoreComments && !loadMoreTopics){
                loadMoreVerifies.style="display:none;";
            }
        }
        for(let i in response.items){
            getTopic(response.items[i].id).then(topic=>{
                let titleWords = findWords(words, topic.header + topic.description);
                container.appendChild(createVerifyObject("Topic", topic, titleWords));
            }).catch(showError);
        }
    }).catch(showError);

    getVerifyComments(perPage, verifyPage).then((response) => {
        if(response.items.length < perPage){
            loadMoreComments = false;
            if(!loadMoreComments && !loadMoreTopics){
                loadMoreVerifies.style="display:none;";
            }
        }
        for(let i in response.items){
            let comment = response.items[i];
            let titleWords = findWords(words, comment.description);
            container.appendChild(createVerifyObject("Comment", comment, titleWords));
        }
    }).catch(showError);

    verifyPage++;
}
/**
 * Adds banned users to input
 */
const loadBannedUsers = ()=>{
    const nicknames = document.getElementById("nicknames");
    nicknames.innerHTML = "";
    getBlockedUsers().then(response=>{
        for(let i in response){
            const option = document.createElement("option");
            option.value = response[i].login;
            option.setAttribute("meta-data", response[i].id);
            nicknames.appendChild(option);
        }
    }).catch(showError);
}
/**
 * Unblocks user with login from input
 */
const unblockSelectedUser = ()=>{
    const input = document.getElementById("users");
    const datalist = document.getElementById("nicknames");
    let id = null;
    for (let i in datalist.options) {
        if (datalist.options[i].value === input.value){
            id = datalist.options[i].getAttribute("meta-data");
        }
    }
    if(id == null){
        openErrorWindow(`There isn't user with login: '${input.value}'; Try other one; Use id only from list`);
        return;
    }
    unBlockUser(id).then(()=>{
        input.value = "";
        loadBannedUsers();
        openMessageWindow("Unblocked!");
    }).catch(showError);
};

const loadModeratorData = ()=>{
    const username = document.getElementById("username");
    const date = document.getElementById("registration-date");
    getUser(getFromStorage("id")).then(response=>{
        username.textContent = response.login;
        username.style += "font-family: monospace; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;";
        username.title = response.login;
        date.textContent += ": "+new Date(response.creationTime).toLocaleDateString();
    }).catch(showError);
}
window.addEventListener("load", ()=>{
    //Moderator data
    loadModeratorData();
    
    //Complaints
    const complaints = document.getElementById("complaint-list");
    complaints.innerHTML = "";
    loadComplains(complaints);
    loadMoreComplaints.onclick = ()=>{
        loadComplains(complaints);
    };

    //Topics and comments with found words
    const verifies = document.getElementById("verify-list");
    verifies.innerHTML = "";
    getVerifyPhrases().then(response=>{
        const words = response.map(item=>item.phrase);
        loadVerifies(verifies, words);
        loadMoreVerifies.onclick = ()=>{
            loadVerifies(verifies, words);
        };
    }).catch(showError);
    
    //Banned users list
    loadBannedUsers();
});