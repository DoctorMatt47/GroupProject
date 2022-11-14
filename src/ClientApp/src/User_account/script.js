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
        if(response.list.length == 0){
            loadMoreButton.classList.add("disabled");
            loadMoreButton.setAttribute("disabled", true);
        }
        for(let i in response.list){
            getTopic(response.list[i].id).then(resp =>{
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

const setUserData = (data)=>{
    username.textContent = getFromStorage("login");
    getUserTopics(data.id, 1, 1).then(response=>{
        cardCreated.textContent = response.pageCount;
    }).catch(showError);
};

window.addEventListener("load", ()=>{
    authenticate(getFromStorage("login"), getFromStorage("password")).then(response=>{
        setUserData(response);
        addCards(response.id);
    }).catch(error=>{
        console.log(error);
    });
});

