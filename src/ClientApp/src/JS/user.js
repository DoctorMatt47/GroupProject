function register(login, password){
    const body = {login: login, password: password}

    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    }
    let res = sendAsync(URLS.Users, request)
    res.then(response => {
        putToStorage("token", response.token)
        putToStorage("login", login)
    })
    return res
}

function authenticate(login, password){
    const body = {login: login, password: password}

    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    }
    let res =  sendAsync(URLS.UsersAuthenticate, request)
    res.then(response => {
        putToStorage("token", response.token)
        putToStorage("login", login)
    })
    return res
}
function isLoggedIn(){
    const token = getFromStorage("token")
    return (token !== null && token !== undefined && token !== "")
}