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

const setUserData = (userId)=>{
    getUser(userId).then(response=>{
        username.textContent = response.login;
        userCreationDate.textContent += ": "+new Date(response.creationTime).toLocaleDateString();
    }).catch(showError);
    getUserTopics(userId, 1, 1).then(response=>{
        cardCreated.textContent = response.itemsCount;
    }).catch(showError);
};

window.addEventListener("load", ()=>{
    authenticate(getFromStorage("login"), getFromStorage("password")).then(response=>{
        setUserData(response.id);
        addCards(response.id);
    }).catch(showError);
});

