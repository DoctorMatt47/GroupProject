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
        loadModerators();
        openMessageWindow(`Created moderator with login:'${login}' and password: '${password}'!`);
    }).catch(showError);
};

const userList = document.getElementById("user-list");
const moderatorList = document.getElementById("moderator-list");

/**
 * Creates user object for list ib page
 * @param {Object} user - data of user
 */
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

const createModeratorObject = (moderator)=>{
    const div = document.createElement("div");
    div.className = "col-sm-3";
    div.innerHTML = `<div>
                        <div class="block">
                            <div class="title moder">
                                <h4><strong>Moderator</strong></h4>
                            </div>
                            <div class="info">
                                <p title='${moderator.login}' 
                                    style='font-family: monospace; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;'>${moderator.login}</p>
                                    <p>Registration date: ${new Date(moderator.creationTime).toLocaleDateString()}</p>
                            </div>
                        </div>
                    </div>`;

    moderatorList.appendChild(div);
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
const loadModerators = ()=>{
    currentModeratorPage = 1
    moderatorList.innerHTML = "";
    const moderatorButton = document.getElementById("load-more-moderators");
    loadUserList(getModerators, currentModeratorPage++, moderatorButton, createModeratorObject);
    getModerators(1,1).then(res=>{
        getModerators(res.itemsCount, 1).then(moderators=>{
            const datalist = document.getElementById("moderator-nicknames");
            for(let i = 0; i < moderators.items.length; i++){
                const option = document.createElement("option");
                option.value = moderators.items[i].login;
                option.setAttribute("meta-data", moderators.items[i].id);
                datalist.appendChild(option);
            }
        }).catch(showError);
    }).catch(showError);
    moderatorButton.onclick = ()=>{
        loadUserList(getModerators, currentModeratorPage++, moderatorButton, createModeratorObject);
    };
};
const loadUsers = ()=>{
    currentUserPage = 1
    userList.innerHTML = "";
    const userButton = document.getElementById("load-more-users");
    loadUserList(getUsers, currentUserPage++, userButton, createUserObject);
    userButton.onclick = ()=>{
        loadUserList(getUsers, currentUserPage++, userButton, createUserObject);
    };
};
/**
 * Unblocks user with login from input
 */
const deleteSelectedModerator = ()=>{
    const input = document.getElementById("moderators");
    const datalist = document.getElementById("moderator-nicknames");
    let id = null;
    for (let i in datalist.options) {
        if (datalist.options[i].value === input.value){
            id = datalist.options[i].getAttribute("meta-data");
        }
    }
    if(id == null){
        openErrorWindow(`There isn't moderator with login: '${input.value}'`);
        return;
    }
    deleteModerator(id).then(()=>{
        loadModerators();
        openMessageWindow("Deleted!");
    }).catch(showError);
};
window.addEventListener("load", ()=>{
    loadModerators();
    loadUsers();
});