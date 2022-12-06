const cardContainer = document.getElementById("card-container");
const loadMoreTopicButton = document.getElementById("load-more");
const commentContainer = document.getElementById("comment-container");
const loadMoreCommentButton = document.getElementById("load-more-comments");
const cardCreated = document.getElementById("created-discussions");

const cardIncrease = 5;
let currentTopicPage = 1;
let currentCommentPage = 1;

const createCard = (data, container) => {
    const card = document.createElement("a");
    card.className = "card";
    card.innerHTML = `${data.header}<br>${cutTextForTopic(data.description)}`;
    card.href = addParameters("../Topic/topic.html", {id:data.id});
    container.appendChild(card);
};
const createTopicCards = (userId)=>{
    getUserTopics(userId, cardIncrease, currentTopicPage++).then(response=>{
        if(response.items.length < cardIncrease){
            loadMoreTopicButton.classList.add("disabled");
            loadMoreTopicButton.setAttribute("disabled", true);
        }
        for(let i in response.items){
            getTopic(response.items[i].id).then(resp =>{
                createCard(resp, cardContainer);
            }).catch(showError);
        }
    }).catch(showError);
}
const createCommentCards = (userId)=>{
    getUserComments(userId, cardIncrease, currentCommentPage++).then(response=>{
        if(response.items.length < cardIncrease){
            loadMoreCommentButton.classList.add("disabled");
            loadMoreCommentButton.setAttribute("disabled", true);
        }
        for(let i in response.items){
            let item = response.items[i];
            item.header = "Comment"
            item.id = item.topicId
            createCard(item, commentContainer);
        }
    }).catch(showError);
};
const addCards = (userId) => {
    createTopicCards(userId);
    createCommentCards(userId);
    loadMoreTopicButton.onclick = ()=>{
        createTopicCards(userId);
    };
    loadMoreCommentButton.onclick = ()=>{
        createCommentCards(userId);
    };
};

const username = document.getElementById("username");
const userCreationDate = document.getElementById("registration-date");
const userPanel = document.getElementById("user-panel");

const setUserWarnings = (response)=>{
    if(response.warningCount > 0){
        const warningPanel = document.getElementById("warning-panel");
        warningPanel.style = "display: block;";
        const warningPanelCount = document.getElementById("warning-count");
        warningPanelCount.textContent = response.warningCount;

        const warnings = new Number(getFromStorage("warnings"));
        if(response.warningCount > warnings){
            openMessageWindow("You have received a warning from a moderator because you have violated forum rules. Your content was removed.");
        }
        putToStorage("warnings", ""+response.warningCount);
    }
};
const setUserBan = (response)=>{
    const ban = new Date(response.banEndTime), now = new Date();
    ban.setHours(ban.getHours() +2);
    if(ban > now){
        const isBanned = getFromStorage("banned");
        if(isBanned == "false"){
            openMessageWindow("You have been banned by moderator because you have violated forum rules.");
            putToStorage("banned", "true");
        }
        userPanel.style = "background-color: tomato;";
        const banTime = document.createElement("p");
        banTime.textContent = "Banned until: "+ban.toLocaleDateString() + " " + ban.toLocaleTimeString();
        userPanel.appendChild(banTime);
    } else putToStorage("banned", "false");
};
const setUserData = (userId)=>{
    getUser(userId).then(response=>{
        username.textContent = response.login;
        username.style += "font-family: monospace; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;";
        username.title = response.login;
        userCreationDate.textContent += ": "+new Date(response.creationTime).toLocaleDateString();
        setUserWarnings(response);
        setUserBan(response);
        
    }).catch(showError);

    getUserTopics(userId, 1, 1).then(response=>{
        cardCreated.textContent = response.itemsCount;
    }).catch(showError);
};

window.addEventListener("load", ()=>{
    const id = getFromStorage("id");
    setUserData(id);
    addCards(id);
});
