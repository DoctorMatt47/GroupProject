const cardContainer = document.getElementById("card-container");
const loadMoreButton = document.getElementById("load-more");
const cardCountElem = document.getElementById("card-count");
const cardTotalElem = document.getElementById("card-total");

let cardLimit = 60;
let cardIncrease = 10;
let pageCount = Math.ceil(cardLimit / cardIncrease);
let currentPage = 1;

const popularCardContainer = document.getElementById("popular-topics");
const popularTopicCount = 3;

const topicPage = "../Topic/topic.html";

cardTotalElem.innerHTML = cardLimit;

const handleButtonStatus = () => {
    if (pageCount === currentPage) {
        loadMoreButton.classList.add("disabled");
        loadMoreButton.setAttribute("disabled", true);
    }
};

const createCard = (topic) => {
    const card = document.createElement("a");
    card.className = "card";
    card.innerHTML = `${topic.header}<br>${cutTextForTopic(topic.description)}`;
    card.href = addParameters(topicPage, {id:topic.id});
    cardContainer.appendChild(card);
};

const addCards = (pageIndex) => {
    currentPage = pageIndex;

    handleButtonStatus();
    getRecommendedTopics(cardIncrease, currentPage).then(response=>{
        pageCount = response.pageCount;
        cardLimit = Math.min(cardLimit, response.itemsCount);
        cardIncrease = Math.min(cardIncrease, response.itemsCount);
        const endRange =
        pageIndex * cardIncrease > cardLimit ? cardLimit : pageIndex * cardIncrease;
        cardCountElem.innerHTML = endRange;
        cardTotalElem.innerHTML = cardLimit;

        for(let i in response.items){
            getTopic(response.items[i].id).then(response=>{
                createCard(response);
            });
        }
    }).catch(showError);
};
const createPopularCard = (topic)=>{
    const column = document.createElement("div");
    column.className = "column col-sm-4";
    const card = document.createElement("div");
    card.className = "thumbnail thumbnail-topic";
    card.innerHTML = `<p title="${topic.userLogin}"
        style="font-family: monospace; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;">
            ${topic.userLogin}</p><a><strong>${topic.header}</strong></a>
    <p>${cutTextForTopic(topic.description)}</p>`;
    column.onclick = ()=>openPage(addParameters(topicPage, {id:topic.id}));
    column.appendChild(card);
    popularCardContainer.appendChild(column);
};
const addPopularCards =()=>{
    getPopularTopics(popularTopicCount, 1).then(response=>{
        for(let i in response.items){
            getTopic(response.items[i].id).then(response=>{
                createPopularCard(response);
            });
        }
    }).catch(showError);
};
/**
 * Hides join button for authorized users
 */
const hideJoinButton = ()=>{
    if(isLoggedIn()){
        const join = document.getElementById("join-button");
        join.style = "display:none";
    } 
};

window.onload = function () {
    hideJoinButton();
    addPopularCards();
    addCards(currentPage);
    loadMoreButton.addEventListener("click", () => {
        addCards(currentPage + 1);
    });
};
