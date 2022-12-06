const userButton = ()=>{
    if(isLoggedIn()){
        window.location.href = "../User-account/account.html";
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

const openMessageWindow = (message) => {
    document.getElementById("message-container").style.display = "block";
    document.getElementById("message-message").textContent = message;
};
const closeMessageWindow = () => {
    document.getElementById("message-container").style.display = "none";
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
const adminButton = ()=>{
    if(isLoggedIn()){
        window.location.href = "../Administrator-Account/admin-account.html";
        return;
    }
};

const openContacts = ()=>{
    openMessageWindow(`"HI-FI CODE" FORUM WAS CREATED BY STUDENTS OF EPS-41 GROUP.IF YOU HAVE ANY QUESTIONS SEND IT ON THIS EMAIL ADDRESS: RUHGUWGHIWHDOHFU@GMAIL.COM`);
};
window.addEventListener("load", ()=>{
    document.getElementById("search-input").value = getValueFromCurrentUrl("pattern");
    addBackgroundClosing(document.getElementById("error-container"), closeErrorWindow);
    if(isLoggedIn()){
        if(getFromStorage("role") === "Moderator"){
            document.getElementById("moderator-login-button").textContent = getFromStorage("login");
            document.getElementById("moderator-menu-item").style.display = "block";
            return;
        }
        if(getFromStorage("role") === "Admin"){
            document.getElementById("admin-login-button").textContent = getFromStorage("login");
            document.getElementById("admin-menu-item").style.display = "block";
            return;
        }
        document.getElementById("user-login-button").textContent = getFromStorage("login");
        document.getElementById("user-menu-item").style.display = "block";
        return;
    }
    document.getElementById("guest-menu-item").style.display = "block";
});