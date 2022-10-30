const cardContainer = document.getElementById("card-container");
const loadMoreButton = document.getElementById("load-more");
const cardCountElem = document.getElementById("card-count");
const cardTotalElem = document.getElementById("card-total");

const cardLimit = 10; //should be specified
const cardIncrease = 5;
const pageCount = Math.ceil(cardLimit / cardIncrease);
let currentPage = 1;

const handleButtonStatus = () => {
    if (pageCount === currentPage) {
        loadMoreButton.classList.add("disabled");
        loadMoreButton.setAttribute("disabled", true);
    }
};

const createCard = (data) => {
    const card = document.createElement("a");
    card.className = "card";
    card.innerHTML = `${data.header}<br>${cutTextForTopic(data.description)}`;
    card.href = addParameters("../Topic/topic.html", {id:data.id});
    cardContainer.appendChild(card);
};

const addCards = (userId, pageIndex) => {
    currentPage = pageIndex;

    /*handleButtonStatus();

    const startRange = (pageIndex - 1) * cardIncrease;
    const endRange =
        pageIndex * cardIncrease > cardLimit ? cardLimit : pageIndex * cardIncrease;

    for (let i = startRange + 1; i <= endRange; i++) {
        createCard();
    }*/
    const err = (error)=>{
        console.log(error);
    };
    getUserTopics(userId).then(response=>{
        for(let i in response){
            getTopic(response[i].id).then(resp =>{
                createCard(resp);
            }).catch(err);
        }
    }).catch(err);
};

const username = document.getElementById("username");

const setUserData = (data)=>{
    username.textContent = getFromStorage("login");
};

window.addEventListener("load", ()=>{
    authenticate(getFromStorage("login"), getFromStorage("password")).then(response=>{
        setUserData(response);
        addCards(response.id, currentPage);
    }).catch(error=>{
        console.log(error);
    });
    loadMoreButton.addEventListener("click", () => {
        addCards(currentPage + 1);
    });
});

