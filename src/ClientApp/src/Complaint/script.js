/**
 * 
 * @param {string} type - topic or comment
 * @param {string} objectId - id of topic or comment
 */
const openForm = (type, objectId) =>{
    const container = document.getElementById("fullscreen-container");
    container.style.display = "block";
    container.onclick = (event)=>{
        if(event.target.id === container.id) closeForm();
    };

    container.setAttribute("object-type", type);
    container.setAttribute("object-id", objectId);
};

const closeForm = () => {
    document.getElementById("fullscreen-container").style.display = "none";
};
/**
 * Creates complain for topic or comment with description from form
 */
const submitComplaint = () =>{
    const container = document.getElementById("fullscreen-container");
    const type = container.getAttribute("object-type");
    const id = container.getAttribute("object-id");
    const create = type === "topic"?createTopicComplaint:createCommentComplaint;
    create(id, document.getElementById("message").value)
        .then(response=>{
            console.log("Complaint was created!");
        }).catch(error=>{
            console.log(error);
        });
};