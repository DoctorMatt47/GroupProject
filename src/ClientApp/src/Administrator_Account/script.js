const openCreateModeratorWindow = () => {
    document.getElementById("createModerator").style.display = "block";
};
const closeCreateModeratorWindow = () => {
    document.getElementById("createModerator").style.display = "none";
};

const moderatorNickname = document.getElementById("moderator-nickname");
const moderatorPassword = document.getElementById("moderator-password");

const submitModerator = ()=>{
    const login = moderatorNickname.value;
    const password = moderatorPassword.value;
    createModerator(login, password).then(() => {
        openMessageWindow(`Created moderator with login:'${login}' and password: '${password}'!`);
    }).catch(showError);
};