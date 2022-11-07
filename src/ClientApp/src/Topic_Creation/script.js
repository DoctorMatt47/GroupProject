/**
 * Makes topic creation form visible
 */
const openForm = () => {
    document.getElementById("topic-container").style.display = "flex";
};
/**
 * Makes topic creation form invisible
 */
const closeForm = () => {
    document.getElementById("topic-container").style.display = "none";
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
 * Gets data from from and sends http request to server.
 * If request is successful than open topic page else gets error
 */
const submitTopic = () => {
    const sectionCreate = document.getElementById("topic-section");
    const titleCreate = document.getElementById("topic-title");
    const descriptionCreate = document.getElementById("topic-description");
    const needCodeCreate = document.getElementById("need-code");
    const codeCreate = document.getElementById("topic-code");
    const languageCreate = document.getElementById("topic-code-language");

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
window.addEventListener("load", ()=>{
    addLanguagesToSelect('topic-code-language');
    addSectionToSelect('topic-section');
});