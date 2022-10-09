function register(login, password){
    const body = {login: login, password: password}

    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    }
    sendAsync(URLS.Users, request)
        .then(response => {
            console.log(response);
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception);
        })
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
    sendAsync(URLS.UsersAuthenticate, request)
        .then(response => {
            putToStorage("token", response.token);
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception);
        })
}