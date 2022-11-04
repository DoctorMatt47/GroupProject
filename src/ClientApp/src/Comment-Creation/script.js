const openCommentForm = () => {
    const container = document.getElementById("comment-container");
    container.style.display = "block";
    container.onclick = (event)=> {
        if (event.target.id === container.id) closeCommentForm();
    };
};
const closeCommentForm = () => {
    document.getElementById("comment-container").style.display = "none";
};

const commentDescription = document.getElementById("comment-description");
const commentNeedCode = document.getElementById("need-code");
const commentCode = document.getElementById("comment-code");
const commentLanguage = document.getElementById("topic-code-language");
/**
 * Gets data from from and sends http request to server.
 * If request is successful than open topic page else gets error
 */
const submitComment = ()=>{

    createComment(getValueFromCurrentUrl("id"),
        commentDescription.value, 
        commentNeedCode.checked ?commentCode.value:"", 
        commentLanguage.value)
    .then(response=>{
        
    })
    .catch(error=>{
        const exception = JSON.parse(error.message);
        console.log(exception);
    });
};
