const openCompiler = (code, language) => {
    const container = document.getElementById("compiler-container");
    const form = document.getElementById("compiler-form");
    container.style.display = "block";
    createCode(form, code, language.toLowerCase());
    container.onclick = (event)=> {
        if (event.target.id === container.id) closeCompiler();
    };
};
const closeCompiler = () => {
    document.getElementById("compiler-form").innerHTML = "";
    document.getElementById("compiler-container").style.display = "none";
};
const createCode = (container, code, language) =>{
    let div = document.createElement("div");
    div.setAttribute("data-pym-src", "https://www.jdoodle.com/plugin");
    div.setAttribute("data-language", language);
    div.textContent = code;
    container.appendChild(div);
    let script = document.createElement("script");
    script.src = "https://www.jdoodle.com/assets/jdoodle-pym.min.js";
    script.type = "text/javascript";
    container.appendChild(script);
};