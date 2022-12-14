/**
 * 
 * @param {string} key - key of data in storage
 * @returns data from storage with key
 */
const getFromStorage = (key)=>{
    return window.localStorage.getItem(key);
};
/**
 * 
 * @param {string} key - key of data in storage 
 * @param {any} value - data to put it to storage
 */
const putToStorage = (key, value)=>{
    window.localStorage.setItem(key, value);
};
/**
 * 
 * @param {string} key - of data in url 
 * @returns data from current url
 */
const getValueFromCurrentUrl=(key)=>{
    let url = new URL(window.location);
    return url.searchParams.get(key);
};
/**
 * 
 * @param {string} url - url to add to it parameters
 * @param {object} params - parameters to add it to url
 * @returns url with parameters
 */
const addParameters = (url, params)=>{
    let res = url + "?";
    for(let key in params){
        res += `${key}=${params[key]}&`;
    }
    return res.slice(0, -1);
};
/**
 * Opens page by url and add to url parameters
 * @param {string} url - url to add to it parameters
 * @param {Object} params - parameters to add it to url
 */
const openPage = (url, params)=>{
    window.location.href = addParameters(url, params);
};
/**
 * 
 * @param {string} text - text to cut
 * @param {number} length - length of resulted text
 * @returns string with new length
 */
const textCutter = (text, length)=>{
    if(text === null || text === undefined) return "";
    return text.slice(0, length);
};
/**
 * 
 * @param {string} text - text from topic
 * @returns cutted string 
 */
const cutTextForTopic = (text)=>{
    return textCutter(text, 30) + "...";
};
/**
 * Adds options (languages) from LANGUAGES to select by id
 * @param {string} selectId - id of `select` html tag to add to it options
 */
const addLanguagesToSelect = (selectId) => {
    let select = document.getElementById(selectId);
    for(let type in LANGUAGES){
        let option = document.createElement("option");
        option.value = type;
        option.textContent = LANGUAGES[type];
        select.appendChild(option);
    }
};
/**
 * 
 * @param {boolean} visible - true to make visible else false
 * @param {string} id - id of html tag
 * @param {string} display - type of display if it will be visible
 */
const setVisible = (visible, id, display) => {
    document.getElementById(id).style.display = visible?display:'none';
};
/**
 * 
 * @param {Object} container - html object to hide it
 * @param {Function} closeFunction - function of closing
 */
const addBackgroundClosing = (container, closeFunction)=>{
    container.addEventListener("click",  (event)=> {
        if (event.target.id === container.id) closeFunction();
    });
}
const getErrorText = (err)=>{
    let exception = err;
    if("message" in err) exception = JSON.parse(err.message);
    let text = exception;
    if("message" in exception) text = exception.message + ";\n";
    if("howToFix" in exception) text += exception.howToFix + ";\n";
    if("howToPrevent" in exception) text += exception.howToPrevent + ";\n";
    return text;
};
const showError = (err) => {
    console.log(err)
    openErrorWindow(getErrorText(err));
};
/**
 * 
 * @param {Array} words 
 * @param {String} text 
 * @returns array with found words
 */
const findWords = (words, text)=>{
    let res = []
    let lower = text.toLowerCase();
    for(let i in words){
        if(lower.search(words[i].toLowerCase()) != -1) res.push(words[i]);
    }
    return res;
};
const viewTopicForCurrentUser = (topicId)=>{
    let saved = getFromStorage("viewedTopics");
    let topics = new Array();
    if(saved != null){
        topics = JSON.parse(saved);
    }
    if(topics.includes(topicId)) return;
    viewTopic(topicId).then(() => {
        topics.push(topicId);
        putToStorage("viewedTopics", JSON.stringify(topics));
    }).catch((err) => {
        console.log(err);
    });
};

const reg = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-5][0-9a-f]{3}-[089ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
/**
 * Checks id is uuid
 */
const isUUID = (id)=>{
    return reg.test(id);
};