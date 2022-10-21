const cardContainer = document.getElementById("card-container");
const loadMoreButton = document.getElementById("load-more");
const cardCountElem = document.getElementById("card-count");
const cardTotalElem = document.getElementById("card-total");

const cardLimit = 60;
const cardIncrease = 10;
const pageCount = Math.ceil(cardLimit / cardIncrease);
let currentPage = 1;

cardTotalElem.innerHTML = cardLimit;

const handleButtonStatus = () => {
    if (pageCount === currentPage) {
        loadMoreButton.classList.add("disabled");
        loadMoreButton.setAttribute("disabled", true);
    }
};

const createCard = () => {
    const card = document.createElement("a");
    card.className = "card";
    card.innerHTML = "Topic name<br>Topic description";
    card.href = "#";
    cardContainer.appendChild(card);
};

const addCards = (pageIndex) => {
    currentPage = pageIndex;

    handleButtonStatus();

    const startRange = (pageIndex - 1) * cardIncrease;
    const endRange =
        pageIndex * cardIncrease > cardLimit ? cardLimit : pageIndex * cardIncrease;

    cardCountElem.innerHTML = endRange;

    for (let i = startRange + 1; i <= endRange; i++) {
        createCard();
    }
};

window.onload = function () {
    addCards(currentPage);
    loadMoreButton.addEventListener("click", () => {
        addCards(currentPage + 1);
    });
};

