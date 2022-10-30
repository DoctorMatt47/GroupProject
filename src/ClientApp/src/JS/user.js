/**
 * Sends request to server to create new user. 
 * After creating gets answer from server and saves user-token and user-login to storage
 * @param {string} login - login of new user
 * @param {string} password - password of new user
 * @returns promise to response with new user data or error
 */
const register = (login, password)=>{
    const body = {login: login, password: password};

    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    };
    return sendAsync(URLS.Users, request);
};
/**
 * Sends request to server to get already created user. 
 * If user login and password are correct gets answer from server and saves user-token and user-login to storage
 * @param {string} login - login of user
 * @param {string} password - password of user
 * @returns  promise to response with user data or error
 */
const authenticate = (login, password)=>{
    const body = {login: login, password: password};

    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    };
    let res =  sendAsync(URLS.UsersAuthenticate, request);
    res.then(response => {
        putToStorage("token", response.token)
        putToStorage("login", login)
        putToStorage("password", password)
    });
    return res;
};
/**
 * 
 * @returns true if there is logged user else false
 */
const isLoggedIn = ()=>{
    const token = getFromStorage("token");
    return (token !== null && token !== undefined && token !== "");
};