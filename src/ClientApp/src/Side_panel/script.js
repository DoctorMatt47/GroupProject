/**
 * Creates section html object for panel in page
 * @param {Object} sectionData - data of section: header, topics count, id
 * @returns html object with section data
 */
const createSectionForPanel = (sectionData)=>{
    const section = document.createElement("a");
    section.innerHTML = `<li class="list-group-item d-flex justify-content-between align-items-center">
                            ${sectionData.header}
                            <span class="badge badge-primary badge-pill">${sectionData.topicCount}</span>
                        </li>`;
    section.href = addParameters("../Topics_in_section/topics_in_section.html", {id:sectionData.id});
    return section;
};
/**
 * Adds sections to page
 * @param {String} containerId - id of container to add to id sections
 */
const addSectionsToPanel = (containerId)=>{
    const container  = document.getElementById(containerId);
    container.innerHTML = "";
    getSections().then((response) => {
        const sections = response.slice(0, 6);
        for(let i in sections){
            container.appendChild(createSectionForPanel(sections[i]));
        }
    }).catch((err) => {
        console.log(err);
    });
};
/**
 * Creates section as programing language html object for panel in page
 * @param {Object} sectionData - data of section: header, topics count, id
 * @returns html object with section data
 */
const createLanguageForPanel = (sectionData)=>{
    const section = document.createElement("a");
    section.innerHTML = `<a>
                            <li class="list-group-item language-list-item">${sectionData.header}</li>
                        </a>`;
    section.href = addParameters("../Topics_in_section/topics_in_section.html", {id:sectionData.id});

    return section;
};
/**
 * Adds languages to page
 * @param {String} containerId - id of container to add to id sections
 */
const addLanguagesToPanel = (containerId)=>{
    const container  = document.getElementById(containerId);
    container.innerHTML = "";
    getSections().then((response) => {
        const languages = Object.values(LANGUAGES);
        const sections = response.filter(item=>languages.includes(item.header)).slice(0, 6);
        for(let i in sections){
            container.appendChild(createLanguageForPanel(sections[i]));
        }
    }).catch((err) => {
        console.log(err);
    });
};
window.addEventListener("load", ()=>{
    addSectionsToPanel("sections-list-group");
    addLanguagesToPanel("languages-list-group");
});