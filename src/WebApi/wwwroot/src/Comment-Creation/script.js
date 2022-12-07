const openCommentForm = () => {
    addLanguagesToSelect('comment-code-language');
    const container = document.getElementById("comment-container");
    container.style.display = "block";
    //addBackgroundClosing(container, closeCommentForm);
};
const closeCommentForm = () => {
    document.getElementById("comment-container").style.display = "none";
};

const commentDescription = document.getElementById("comment-description");
const commentNeedCode = document.getElementById("comment-need-code");
const commentCode = document.getElementById("comment-code");
const commentLanguage = document.getElementById("comment-code-language");
const commentError = document.getElementById("comment-error");
/**
 * Gets data from from and sends http request to server.
 * If request is successful than open topic page else gets error
 */
const submitComment = ()=>{
    commentError.textContent = "";
    createComment(getValueFromCurrentUrl("id"),
        commentDescription.value, 
        commentNeedCode.checked ?commentCode.value:"", 
        commentLanguage.value)
    .then(response=>{
        if("id" in response){
            document.getElementById("comment-form").reset();
            setVisibleForComment(true);
            commentNeedCode.checked = true;
            getComment(response.id).then((comment)=>createCommentObject("User",comment, true)).catch(showError);
        }
        else openErrorWindow(response.message);
        closeCommentForm();
    })
    .catch((err)=>{
        commentError.textContent = getErrorText(err);
    });
};

const setVisibleForComment = (visible)=>{
    setVisible(visible, 'comment-code-container', 'flex');
    commentCode.required = visible;
    commentLanguage.required = visible;
};
