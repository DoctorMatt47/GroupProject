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
        <a href="#" id= "username">${topic.userLogin}</a>
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
        if(section == undefined) return;
        loadSection(section);
        const container = document.getElementById("topics-container");
        container.innerHTML = '';
        const loadMore = document.getElementById("load-more");
        loadSectionTopics(container, section, loadMore);
        loadMore.onclick = ()=>{
            loadSectionTopics(container, section, loadMore);
        };
    }).catch(showError);
};
window.addEventListener("load", loadData);

const openUpdateForm = () => {
    document.getElementById("updateWindow").style.display = "block";

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