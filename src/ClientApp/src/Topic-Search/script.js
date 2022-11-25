/**
 * Creates html object with topic data
 * @param {Object} topic - object with topic data
 */
const createSearchTopic = (topic)=>{
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
const perPage = 10;
let currentPage = 1;

const loadTopicsSearch = (container, pattern, loadMore)=>{
    getSearchTopics(pattern, perPage, currentPage++).then((response) => {
        if(currentPage - 1 >= response.pageCount){
            loadMore.classList.add("disabled");
            loadMore.setAttribute("disabled", true);
        }
        for(let i in response.items){
            getTopic(response.items[i].id).then(topic=>{
                container.appendChild(createSearchTopic(topic));
            }).catch(showError);
        }
    }).catch(showError);
}
window.addEventListener("load", ()=>{
    const request = document.getElementById("your-request");
    const pattern = getValueFromCurrentUrl("pattern");
    request.textContent += " " + pattern;
    const container = document.getElementById("topics-container");
    container.innerHTML = "";
    const loadMore = document.getElementById("load-more");
    loadTopicsSearch(container, pattern, loadMore);
    loadMore.onclick = ()=>{
        loadTopicsSearch(container, pattern, loadMore);
    };
});