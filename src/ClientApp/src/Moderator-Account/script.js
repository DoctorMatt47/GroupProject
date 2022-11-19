const openUserActionWindow = () => {
    document.getElementById("Unblock").style.display = "block";
};
const closeUserActionWindow = () => {
    document.getElementById("Unblock").style.display = "none";
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
 * Creates complaint html object for page
 * @param {object} complaint - object with complaint data
 */
const createVerifyObject = (type, obj, words)=>{
    const div = document.createElement("div");
    div.innerHTML = `<a href="${addParameters('../Moderator-Dispute/moderator-dispute.html', {id:obj.id, type:"Verify"+type})}">
                        <div class="block">
                            <div class="title ${type === "Topic"?"topic":"comment"}">
                                <h4><strong>${type}</strong></h4>
                            </div>
                            <div class="info">
                                <p>${textCutter(words, 20) + "..."}</p>
                                <p>Creation date:${new Date(obj.creationTime).toLocaleDateString()}</p>
                            </div>
                        </div>
                    </a>`
    div.className = "col-sm-3";
    return div;
}

const perPage = 8;

const loadMoreComplaints = document.getElementById("load-more-complaints");
let complaintPage = 1;

const loadComplains = (container)=>{
    getComplaints(perPage, complaintPage++).then((response) => {
        if(response.itemsCount == 0){
            loadMoreComplaints.style = "display: none";
            return;
        }
        for(let i in response.items){
            container.appendChild(createComplaintObject(response.items[i]));
        }
    }).catch(showError);
}

const loadMoreVerifies = document.getElementById("load-more-verifies");
let verifyPage = 1;

const loadVerifies = (container, words)=>{
    let loadMoreTopics = true, loadMoreComments = true;
    getVerifyTopics(perPage, verifyPage).then((response) => {
        if(response.itemsCount == 0){
            loadMoreTopics = false;
            return;
        }
        for(let i in response.items){
            getTopic(response.items[i].id).then(topic=>{
                console.log(topic);
                let titleWords = findWords(words, topic.header + topic.description);
                container.appendChild(createVerifyObject("Topic", topic, titleWords));
            }).catch(showError);
        }
    }).catch(showError);
    return;
    getVerifyComments(perPage, verifyPage).then((response) => {
        if(response.itemsCount == 0){
            loadMoreComments = false;
            return;
        }
        for(let i in response.items){
            let comment = response.items[i];
            let titleWords = findWords(words, comment.description);
            container.appendChild(createVerifyObject("Comment", comment, titleWords));
        }
    }).catch(showError);
    if(!loadMoreComments && !loadMoreTopics){
        loadMoreVerifies.style="display:none;";
        return;
    }
    verifyPage++;
}
const loadBannedUsers = ()=>{
    const nicknames = document.getElementById("nicknames");
    nicknames.innerHTML = "";
    getBlockedUsers().then(response=>{
        console.log(response);
        for(let i in response.items){
            const option = document.createElement("option");
            option.value = response.items[i].login;
            option.textContent = response.items[i].id;
            nicknames.appendChild(option);
        }
    }).catch(showError);
}
window.addEventListener("load", ()=>{
    const username = document.getElementById("username");
    const date = document.getElementById("registration-date");
    getUser(getFromStorage("id")).then(response=>{
        username.textContent = response.login;
        date.textContent += ": "+new Date(response.creationTime).toLocaleDateString();
    }).catch(showError);
    

    const complaints = document.getElementById("complaint-list");
    complaints.innerHTML = "";
    loadComplains(complaints);
    loadMoreComplaints.onclick = ()=>{
        loadComplains(complaints);
    };

    const verifies = document.getElementById("verify-list");
    verifies.innerHTML = "";
    getVerifyPhrases().then(response=>{
        loadVerifies(verifies, response.map(item=>item.phrase));
    }).catch(showError);
    
    loadBannedUsers();
});