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
window.addEventListener("load", ()=>{
    addBackgroundClosing(document.getElementById("error-container"), closeErrorWindow);
    if(isLoggedIn()){
        document.getElementById("user-menu-item").style.display = "block";
        document.getElementById("login-button").textContent =  getFromStorage("login");
        return;
    }
    //TODO: add for admin and moderator
    document.getElementById("guest-menu-item").style.display = "block";
});