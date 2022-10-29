const openForm = () => {
    document.getElementById("fullscreen-container").style.display = "block";
};

const closeForm = () => {
    document.getElementById("fullscreen-container").style.display = "none";
};

const addLanguagesToSelect = (selectId) => {
    let select = document.getElementById(selectId);
    console.log(select)
    for(let type in LANGUAGES){
        let option = document.createElement("option");
        option.value = type;
        option.textContent = LANGUAGES[type];
        select.appendChild(option);
    }
};
const setVisible = (visible, id, display) => {
    document.getElementById(id).style.display = visible?display:'none';
};

const sectionCreate = document.getElementById("topic-section");
const titleCreate = document.getElementById("topic-title");
const descriptionCreate = document.getElementById("topic-description");
const needCodeCreate = document.getElementById("need-code");
const codeCreate = document.getElementById("topic-code");
const languageCreate = document.getElementById("topic-code-language");

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