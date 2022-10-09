function getFromStorage(key){
    return window.localStorage.getItem(key)
}
function putToStorage(key, value){
    window.localStorage.setItem(key, value)
}