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
window.addEventListener("load", ()=>{
    if(isLoggedIn()){
        document.getElementById("user-menu-item").style.display = "block";
        document.getElementById("login-button").textContent =  getFromStorage("login");
        return;
    }
    //TODO: add for admin and moderator
    document.getElementById("guest-menu-item").style.display = "block";
});