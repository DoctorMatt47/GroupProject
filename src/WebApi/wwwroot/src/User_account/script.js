const cardContainer = document.getElementById("card-container");
const loadMoreButton = document.getElementById("load-more");
const cardCreated = document.getElementById("created-discussions");

const cardIncrease = 5;
let currentPage = 1;

const createCard = (data) => {
    const card = document.createElement("a");
    card.className = "card";
    card.innerHTML = `${data.header}<br>${cutTextForTopic(data.description)}`;
    card.href = addParameters("../Topic/topic.html", {id:data.id});
    cardContainer.appendChild(card);
};
const createCards = (userId)=>{
    getUserTopics(userId, cardIncrease, currentPage++).then(response=>{
        if(response.items.length == 0){
            loadMoreButton.classList.add("disabled");
            loadMoreButton.setAttribute("disabled", true);
        }
        for(let i in response.items){
            getTopic(response.items[i].id).then(resp =>{
                createCard(resp);
            }).catch(showError);
        }
    }).catch(showError);
}

const addCards = (userId) => {
    createCards(userId);
    loadMoreButton.onclick = ()=>{
        createCards(userId);
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