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

        const warnings = document.getElementById("warning-count");
        warnings.textContent = `Users will be blocked after getting ${response.warningCountForBan} warnings.`;

        const ban = document.getElementById("ban-duration");
        ban.textContent = `Users will be blocked for ${response.banDuration}.`;
        
        const verify = document.getElementById("verification-duration");
        verify.textContent = `Complaints will be removed from system after ${response.verificationDuration}.`;
    }).catch(showError);
};
window.addEventListener("load", ()=>{
    loadForAdmin();
    loadConfiguration();
    getForbiddenPhrases().then(response=>{
        loadWords(response.map(item=>item.phrase), "forbidden-words", "Forbidden phrases: ", true);
    }).catch(showError);
    getVerifyPhrases().then(response=>{
        loadWords(response.map(item=>item.phrase), "verification-words", "Phrases that need verification: ");
    }).catch(showError);
});