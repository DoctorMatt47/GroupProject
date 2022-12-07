/**
 * Adds section data to page
 * @param {Object} section - object with section data
 */
const loadSection = (section)=>{
    const header = document.getElementById("section-name");
    const description = document.getElementById("section-description");
    header.textContent = section.header;
    description.textContent = section.description;
};
/**
 * Creates html object with topic data
 * @param {Object} topic - object with topic data
 */
const createSectionTopic = (topic)=>{
    const container = document.createElement("div");
    container.className = "topic";
    container.innerHTML = `            
    <div class="col-sm-2 user-info">
        <a href="#">
            <div class="language">${topic.compileOptions.language}</div>
        </a>
        <a href="#" id= "username" title="${topic.userLogin}"
            style="font-family: monospace; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;">
            ${topic.userLogin}</a>
        <p class="topic-p">${new Date(topic.creationTime).toLocaleDateString()}</p>
    </div>
    <div class="vertical-line"></div>
    <a href="${addParameters("../Topic/topic.html", {id:topic.id})}">
        <div class="col-sm-12">
            <div class="topic-name">${topic.header}</div>
            <div class="topic-description">
            ${topic.description}
            </div>
            <div class="topic-info">
                <p class="topic-p">views:${topic.viewCount}</p>
            </div>
        </div>
    </a>
    `
    return container;
};
const perSectionPage = 10;
let currentPage = 1;
/**
 * Adds topics of section to page
 * @param {Object} section - object with section data
 */
const loadSectionTopics = (container, section, loadMore)=>{
    getSectionTopics(section.id, perSectionPage, currentPage++).then((response) => {
        if(currentPage - 1 >= response.pageCount){
            loadMore.classList.add("disabled");
            loadMore.setAttribute("disabled", true);
        }
        for(let i in response.items){
            getTopic(response.items[i].id).then((topic) => {
                container.appendChild(createSectionTopic(topic));
            }).catch(showError);
        }
    }).catch(showError);
};
const loadData = ()=>{
    const sectionId = getValueFromCurrentUrl("id");
    getSections().then((response) => {
        const section = response.find(item=>item.id == sectionId);
        const container = document.getElementById("topics-container");
        container.innerHTML = '';
        if(section == undefined){
            loadSection({header:"", description:""});
            openErrorWindow("Section not found!");
            return;
        } 
        loadSection(section);
        const loadMore = document.getElementById("load-more");
        loadSectionTopics(container, section, loadMore);
        loadMore.onclick = ()=>{
            loadSectionTopics(container, section, loadMore);
        };
    }).catch(showError);
    if(getFromStorage("role") === "Admin"){
        document.getElementById("edit-button").style = "display:block";
        document.getElementById("delete-button").style = "display:block";
    }
};
window.addEventListener("load", loadData);


const title = document.getElementById("new-section-name");
const description = document.getElementById("new-section-description");

const openUpdateForm = () => {
    const sectionId = getValueFromCurrentUrl("id");
    getSections().then((response) => {
        const section = response.find(item=>item.id == sectionId);
        if(section == undefined) return;
        title.value = section.header;
        description.value = section.description;
        document.getElementById("updateWindow").style.display = "block";
    }).catch(showError);

};
const closeUpdateForm = () => {
    document.getElementById("updateWindow").style.display = "none";
};
const openDeleteForm = () => {
    document.getElementById("deleteWindow").style.display = "block";

};
const closeDeleteForm = () => {
    document.getElementById("deleteWindow").style.display = "none";
};
/**
 * Gets data from form and sends request to update current section
 */
const submitUpdateSection = ()=>{
    const sectionId = getValueFromCurrentUrl("id");
    const error = document.getElementById("section-error");
    error.textContent = "";

    updateSection(sectionId, title.value, description.value).then(()=>{
        closeUpdateForm();
        openMessageWindow("Updated!");
        loadSection({"id":sectionId, "header":title.value, "description":description.value});
    }).catch(err=>{
        error.textContent = getErrorText(err);
    });
}
/**
 * Sends request for section removing
 */
const deleteCurrentSection = ()=>{
    deleteSection(getValueFromCurrentUrl("id")).then(()=>{
        openMessageWindow("Deleted!");
        openPage("../Sections/sections.html");
    }).catch(showError);
};