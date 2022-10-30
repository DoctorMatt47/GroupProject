/**
 * Makes topic creation form visible
 */
const openForm = () => {
    document.getElementById("fullscreen-container").style.display = "block";
};
/**
 * Makes topic creation form invisible
 */
const closeForm = () => {
    document.getElementById("fullscreen-container").style.display = "none";
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
 * Adds options (sections) from server to select by id
 * @param {string} selectId - id of `select` html tag to add to it options
 */
const addSectionToSelect = (selectId) =>{
    let select = document.getElementById(selectId);
    getSections().then(response=>{
        for(let type in response){
            let option = document.createElement("option");
            option.value = response[type].id;
            option.textContent = response[type].header;
            select.appendChild(option);
        }
    }).catch(error=>{
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
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

const sectionCreate = document.getElementById("topic-section");
const titleCreate = document.getElementById("topic-title");
const descriptionCreate = document.getElementById("topic-description");
const needCodeCreate = document.getElementById("need-code");
const codeCreate = document.getElementById("topic-code");
const languageCreate = document.getElementById("topic-code-language");

/**
 * Gets data from from and sends http request to server.
 * If request is successful than open topic page else gets error
 */
const submitTopic = () => {
    createTopic(sectionCreate.value,
        titleCreate.value, 
        descriptionCreate.value, 
        needCodeCreate.checked ?codeCreate.value:"", 
        languageCreate.value)
    .then(response=>{
        openPage(addParameters("../Topic/topic.html", {id:response.id}));
    })
    .catch(error=>{
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
};