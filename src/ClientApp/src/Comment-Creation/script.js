const openCommentForm = () => {
    addLanguagesToSelect('comment-code-language');
    const container = document.getElementById("comment-container");
    container.style.display = "block";
    addBackgroundClosing(container, closeCommentForm);
};
const closeCommentForm = () => {
    document.getElementById("comment-container").style.display = "none";
};

const commentDescription = document.getElementById("comment-description");
const commentNeedCode = document.getElementById("comment-need-code");
const commentCode = document.getElementById("comment-code");
const commentLanguage = document.getElementById("comment-code-language");
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
        if("id" in response){
            getComment(response.id).then((comment)=>createCommentObject("User",comment, true)).catch(showError);
        }
        else openErrorWindow(response.message);
    })
    .catch(showError);
};
