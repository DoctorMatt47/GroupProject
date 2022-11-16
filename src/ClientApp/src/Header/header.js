const userButton = ()=>{
    if(isLoggedIn()){
        window.location.href = "../User_account/account.html";
        return;
    }
};
const guestButton = ()=>{
    window.location.href = "../Login/login.html";
};
const signOutButton = ()=>{
    localStorage.clear();
    window.location.href = "../Home/home.html";
};

const openErrorWindow = (error)=>{
    document.getElementById("error-message").textContent = error;
    document.getElementById("error-container").style = "display: block;";
};
const closeErrorWindow = ()=>{
    document.getElementById("error-container").style = "display: none;";
};
const searchTopics = ()=>{
    const input = document.getElementById("search-input");
    if(input.value === "") return;
    openPage("../Topic-Search/topic-search.html", {"pattern":input.value});
}
const moderatorButton = ()=>{
    if(isLoggedIn()){
        window.location.href = "../Moderator-Account/moderator-account.html";
        return;
    }
};
window.addEventListener("load", ()=>{
    document.getElementById("search-input").value = getValueFromCurrentUrl("pattern");
    addBackgroundClosing(document.getElementById("error-container"), closeErrorWindow);
    if(isLoggedIn()){
        authenticate(getFromStorage("login"), getFromStorage("password"))
        .then((result) => {
            if(result.role === "Moderator"){
                document.getElementById("moderator-login-button").textContent = getFromStorage("login");
                document.getElementById("moderator-menu-item").style.display = "block";
                return;
            }
            document.getElementById("user-login-button").textContent = getFromStorage("login");
            document.getElementById("user-menu-item").style.display = "block";
        }).catch(error=>{
            localStorage.clear();
        });
        return;
    }
    //TODO: add for admin and moderator moderator-account.html
    document.getElementById("guest-menu-item").style.display = "block";
});