/**
 *
 * @param {boolean} visible - true to make visible else false
 * @param {string} id - id of html tag
 * @param {string} display - type of display if it will be visible
 */
const setVisible = (visible, id, display) => {
    document.getElementById(id).style.display = visible?display:'none';
};

/**
 * Adds options (languages) from LANGUAGES to select by id
 * @param {string} selectId - id of `select` html tag to add to it options
 */
const addLanguagesToSelect = (selectId) => {
    let select = document.getElementById(selectId);
    for(let type in LANGUAGES){
        let option = document.createElement("option");
        option.value = type;
        option.textContent = LANGUAGES[type];
        select.appendChild(option);
    }
};