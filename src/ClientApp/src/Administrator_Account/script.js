const openCreateModeratorWindow = () => {
    document.getElementById("createModerator").style.display = "block";
};
const closeCreateModeratorWindow = () => {
    document.getElementById("createModerator").style.display = "none";
};

const moderatorNickname = document.getElementById("moderator-nickname");
const moderatorPassword = document.getElementById("moderator-password");

/**
 * Creates moderator with data from form
 */
const submitModerator = ()=>{
    const login = moderatorNickname.value;
    const password = moderatorPassword.value;
    createModerator(login, password).then(() => {
        openMessageWindow(`Created moderator with login:'${login}' and password: '${password}'!`);
    }).catch(showError);
};

const userList = document.getElementById("user-list");
const moderatorList = document.getElementById("moderator-list");

const createUserObject = (user)=>{
    const div = document.createElement("div");
    div.className = "col-sm-3";
    getUserTopics(user.id, 1, 1).then(response=>{
        div.innerHTML = `<div>
                            <div class="block" style='${isBanned(user)?"background-color:#FF4500":""}'>
                                <div class="title user">
                                    <h4 title='${user.login}'><strong>${user.login}</strong></h4>
                                </div>
                                <div class="info">
                                    <p>Registration date: ${new Date(user.creationTime).toLocaleDateString()}</p>
                                    <p>Number of topics: ${response.itemsCount}</p>
                                    <p>Number of warnings: ${user.warningCount}</p>
                                </div>
                            </div>
                        </div>`
    }).catch(showError);

    userList.appendChild(div);
};

const perUserPage = 4;
let currentUserPage = 1, currentModeratorPage = 1;

const loadUserList = (getter, page, button, creator)=>{
    getter(perUserPage, page).then(response=>{
        if(response.items.length < perUserPage){
            button.style = "display: none;";
        }
        for(let i in response.items){
            creator(response.items[i]);
        }
    }).catch(showError);
};
window.addEventListener("load", ()=>{
    userList.innerHTML = "";
    const userButton = document.getElementById("load-more-users");
    loadUserList(getUsers, currentUserPage, userButton, createUserObject);
});