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
        console.log(response)
        putToStorage("token", response.token);
        putToStorage("login", login);
        putToStorage("id", response.id);
        putToStorage("role", response.role);
    });
    return res;
};
/**
 * 
 * @returns true if there is logged user else false
 */
const isLoggedIn = ()=>{
    const token = getFromStorage("login");
    return (token !== null && token !== undefined && token !== "");
};
/**
 * 
 * @param {string} userId - id of user
 * @returns promise to response with user data or error
 */
const getUser = (userId)=>{
    const request = {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
        },
    };
    return sendAsync(URLS.Users + `/${userId}`, request);
};
/**
 * Blocks user for ban duration time(from configuration)
 * @param {string} userId - id of user 
 */
const blockUser = (userId)=>{
    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            "Authorization":"Bearer "+getFromStorage("token")
        },
    };
    return sendAsync(URLS.Users + `/Ban/${userId}`, request);
};
/**
 * Unblocks user
 * @param {string} userId - id of user 
 */
const unBlockUser = (userId)=>{
    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            "Authorization":"Bearer "+getFromStorage("token")
        },
    };
    return sendAsync(URLS.Users + `/${userId}`, request);
};
/**
 * Adds one warning to user
 * @param {string} userId - id of user 
 */
const warningUser = (userId)=>{
    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            "Authorization":"Bearer "+getFromStorage("token")
        },
    };
    return sendAsync(URLS.Users + `/${userId}/Warning`, request);
};
/**
 * @returns promise to list of users
 */
const getUsers = (perPage, page)=>{
    const request = {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            "Authorization":"Bearer "+getFromStorage("token")
        },
    };
    return sendAsync(URLS.Users+`?Number=${page}&Size=${perPage}`, request);
};
/**
 * @returns promise to list of users
 */
const getModerators = (perPage, page)=>{
    const request = {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            "Authorization":"Bearer "+getFromStorage("token")
        },
    };
    return sendAsync(URLS.Users+`/Moderators?Number=${page}&Size=${perPage}`, request);
};
/**
 * @returns promise to list of blocked users
 */
const getBlockedUsers = ()=>{
    const request = {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            "Authorization":"Bearer "+getFromStorage("token")
        },
    };
    return sendAsync(URLS.Users + `/Banned`, request);
};