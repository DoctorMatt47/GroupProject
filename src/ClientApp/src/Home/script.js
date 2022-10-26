const cardContainer = document.getElementById("card-container");
const loadMoreButton = document.getElementById("load-more");
const cardCountElem = document.getElementById("card-count");
const cardTotalElem = document.getElementById("card-total");

const cardLimit = 60;
const cardIncrease = 10;
const pageCount = Math.ceil(cardLimit / cardIncrease);
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

const createCard = (topicId) => {
    const card = document.createElement("a");
    card.className = "card";
    getTopic(topicId).then(response=>{
        card.innerHTML = `${response.header}<br>${cutTextForTopic(response.description)}`;
        card.href = addParameters(topicPage, {id:topicId});
    });
    cardContainer.appendChild(card);
};

const addCards = (pageIndex) => {
    currentPage = pageIndex;

    handleButtonStatus();

    const endRange =
        pageIndex * cardIncrease > cardLimit ? cardLimit : pageIndex * cardIncrease;

    cardCountElem.innerHTML = endRange;
    getRecommendedTopics(cardIncrease, currentPage).then(response=>{
        for(let i in response.list){
            createCard(response.list[i].id);
        }
    }).catch(error => {
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
};
const createPopularCard = (topicId)=>{
    const column = document.createElement("div");
    column.className = "column col-sm-4";
    const card = document.createElement("div");
    card.className = "thumbnail thumbnail-topic";

    getTopic(topicId).then(response=>{
        card.innerHTML = `<p>${response.userLogin}</p><a><strong>${response.header}</strong></a>
        <p>${cutTextForTopic(response.description)}</p>`;
        column.onclick = ()=>openPage(addParameters(topicPage, {id:topicId}));
    });

    column.appendChild(card);
    popularCardContainer.appendChild(column);
};
const addPopularCards =()=>{
    getPopularTopics(popularTopicCount, 1).then(response=>{
        for(let i in response.list){
            createPopularCard(response.list[i].id)
        }
    }).catch(error=>{
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
};
window.onload = function () {
    addPopularCards();
    addCards(currentPage);
    loadMoreButton.addEventListener("click", () => {
        addCards(currentPage + 1);
    });
};

