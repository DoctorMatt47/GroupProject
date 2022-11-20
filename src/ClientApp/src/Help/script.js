/**
 * 
 * Shows buttons for admin
 */
const loadForAdmin = ()=>{
    const role = getFromStorage("role");
    if(role !== "Admin") return;
    const rules = document.getElementById("rules-button");
    rules.style = "";
    const words = document.getElementById("words-button");
    words.style = "";
    const warnings = document.getElementById("warnings-button");
    warnings.style = "";
    const verify = document.getElementById("verification-duration");
    verify.style = "";
};
/**
 * Adds words to page
 * @param {Array} words - array with words
 * @param {string} id - id of html object
 */
const loadWords = (words, id, content, hide = false) => {
    const div = document.getElementById(id);
    div.textContent = content;
    for(let i in words){
        const word = hide?words[i].slice(0, 1) + "*" + words[i].slice(2, words[i].length):words[i];
        div.textContent += word + ", ";
    }
    div.textContent = div.textContent.slice(0, -2) + ".";
};
/**
 * Loads forum configuration
 */
const loadConfiguration = ()=>{
    getConfiguration().then((response) => {
        const rules = document.getElementById("rules");
        rules.textContent = response.rules;
        document.getElementById("rules-text").textContent = response.rules;

        const warnings = document.getElementById("warning-count");
        warnings.textContent = `Users will be blocked after getting ${response.warningCountForBan} warnings.`;

        const ban = document.getElementById("ban-duration");
        ban.textContent = `Users will be blocked for ${response.banDuration}.`;
        
        const verify = document.getElementById("verification-duration");
        verify.textContent = `Complaints will be removed from system after ${response.verificationDuration}.`;
    }).catch(showError);
};
const loadPhrases = ()=>{
    getForbiddenPhrases().then(response=>{
        const words = response.map(item=>item.phrase);
        loadWords(words, "forbidden-words", "Forbidden phrases: ", true);
        const input = document.getElementById("forbidden");
        input.value = "";
        words.forEach(item=>input.value += item + ", ");
        input.value = input.value.slice(0, -2);
    }).catch(showError);

    getVerifyPhrases().then(response=>{
        const words = response.map(item=>item.phrase);
        loadWords(words, "verification-words", "Phrases that need verification: ");
        const input = document.getElementById("verification");
        input.value = "";
        words.forEach(item=>input.value += item + ", ");
        input.value = input.value.slice(0, -2);
    }).catch(showError);
};
const submitRules = ()=>{
    const rules = document.getElementById("rules-text");
    updateConfiguration({rules:rules.value}).then(()=>{
        openMessageWindow("Updated!");
        loadConfiguration();
    }).catch(showError);
};
const submitWords = ()=>{
    const forbidden = document.getElementById("forbidden");
    const verification = document.getElementById("verification");
    const fWords = forbidden.value.split(/[\s,]+/).filter(item=>item.length > 1);
    const vWords = verification.value.split(/[\s,]+/).filter(item=>item.length > 1);
    updateForbiddenPhrases(fWords).then(()=>{
        openMessageWindow("Updated!");
        loadPhrases();
    }).catch(showError);
    updateVerifyPhrases(vWords).then(()=>{
        loadPhrases();
    }).catch(showError);
};

window.addEventListener("load", ()=>{
    loadForAdmin();
    loadConfiguration();
    loadPhrases();
});
const openUpdateForm = () => {
    document.getElementById("update-container").style.display = "block";
};
const closeUpdateForm = () => {
    document.getElementById("update-container").style.display = "none";
};
const openUpdateWordsForm = () => {
    document.getElementById("update-words-container").style.display = "block";
};
const closeUpdateWordsForm = () => {
    document.getElementById("update-words-container").style.display = "none";
};
const openUpdateWarningForm = () => {
    document.getElementById("update-warning-container").style.display = "block";
};
const closeUpdateWarningForm = () => {
    document.getElementById("update-warning-container").style.display = "none";
};