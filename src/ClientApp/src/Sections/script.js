const sectionNameInput = document.getElementById("section-search");
const sectionSearchButton = document.getElementById("section-search-button");

/**
 * Creates section block to add to page
 * @param {Array} section - list of section data
 * @returns html object with section data
 */
const createSectionObject = (section) =>{
    const column = document.createElement("div");
    column.className = "col-sm-3";

    const a = document.createElement("a");
    a.href = addParameters("../Topics_in_section/topics_in_section.html", {id:section.id});

    const block = document.createElement("div");
    block.className = "block";

    const title = document.createElement("div");
    title.className = "section-title";
    title.innerHTML = `<h4><strong>${section.header}</strong></h4>`;
    block.appendChild(title);

    const info = document.createElement("div");
    info.className = "section-info";
    info.innerHTML = `<p>Number of topics:${section.topicCount}</p><p>${section.description}</p>`;
    block.appendChild(info);

    a.appendChild(block);
    column.appendChild(a);
    
    return column;
};

const perPage = 8;
let currentPage = 1;
let pattern = "";
/**
 * 
 * @param {Object} container - html container for section blocks
 * @param {Array} sections - list of section data
 * @param {Number} page - page number of sections to add it to html page
 */
const addSectionsPage = (container, sections) =>{
    container.innerHTML = "";
    const objects = sections.slice((currentPage - 1)*perPage, Math.min(sections.length, currentPage*perPage));
    for(let i in objects){
        container.appendChild(createSectionObject(objects[i]));
    }
};
/**
 * Adds page buttons to page nav bar
 * @param {Object} navBar - html object with page buttons
 * @param {Number} pagesNumber - count of buttons
 * @param {Object} container - html container for section blocks
 * @param {Array} sections - list of section data
 */
const createPageButtons = (navBar, pagesNumber, container, sections) =>{
    for(let i = 0; i < pagesNumber; i++){
        const pageButton = document.createElement("a");
        pageButton.textContent = i + 1;
        pageButton.id = `page-button${i + 1}`;
        if(i == 0) pageButton.classList.add("active");
        pageButton.onclick = ()=>{
            document.getElementById(`page-button${currentPage}`).classList.remove("active");
            pageButton.classList.add("active");
            currentPage = i + 1;
            addSectionsPage(container, sections);
        }
        navBar.appendChild(pageButton);
    }
};
/**
 * Creates page nav bar for sections and adds its to html page
 * @param {Object} container - html container for section blocks
 * @param {Array} sections - list of section data
 */
const addPagesBar = (container, sections)=>{
    const pages = document.getElementById("pages");
    pages.innerHTML = '';
    //Creates button LEFT
    const prev = document.createElement("a");
    prev.innerHTML = `&laquo;`;
    prev.onclick = ()=>{
        console.log(currentPage)
        if(currentPage - 1 >= 1){
            document.getElementById(`page-button${currentPage}`).classList.remove("active");
            currentPage--;
            document.getElementById(`page-button${currentPage}`).classList.add("active");
            addSectionsPage(container, sections);
        }
    }
    pages.appendChild(prev);
    
    const pagesNumber = Math.ceil(sections.length / perPage);
    createPageButtons(pages, pagesNumber, container, sections);

    //Creates button RIGHT
    const next = document.createElement("a");
    next.innerHTML = `&raquo;`;
    next.onclick = ()=>{
        if(currentPage + 1 <= pagesNumber){
            document.getElementById(`page-button${currentPage}`).classList.remove("active");
            currentPage++;
            document.getElementById(`page-button${currentPage}`).classList.add("active");
            addSectionsPage(container, sections);
        }
    }
    pages.appendChild(next);
}
/**
 * Adds sections to page
 */
const addSections = ()=>{
    getSections().then((response) => {
        const container = document.getElementById("page-sections");
        container.innerHTML = "";
        response = response.filter(item=>item.header.toLowerCase().includes(pattern)||item.description.toLowerCase().includes(pattern));
        addPagesBar(container, response);
        addSectionsPage(container, response, currentPage);
    }).catch((err) => {
        console.log(err);
    });
};

window.addEventListener("load", ()=>{
    addSections();
    sectionSearchButton.onclick = ()=>{
        if(pattern != sectionNameInput.value){
            pattern = sectionNameInput.value.toLowerCase();
            addSections();
        }
    }
});