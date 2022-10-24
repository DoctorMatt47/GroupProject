const loginButton = ()=>{
    if(isLoggedIn()){
        window.location.href = "../User_account/account.html"
        return
    }
    window.location.href = "../Login/login.html"
}
window.onload = ()=>{
    document.getElementById("login-button").textContent =  isLoggedIn()?"My Account":"LOGIN/JOIN"
}