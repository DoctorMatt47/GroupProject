function getFromStorage(key){
    return window.localStorage.getItem(key)
}
function putToStorage(key, value){
    window.localStorage.setItem(key, value)
}
function getValueFromCurrentUrl(key){
    let url = new URL(window.location)
    return url.searchParams.get(key)
}
function addParameters(url, params){
    let res = url + "?"
    for(let key in params){
        res += `${key}=${params[key]}&`
    }
    return res.slice(0, -1)
}
function openPage(url, params){
    window.location.href = addParameters(url, params)
}