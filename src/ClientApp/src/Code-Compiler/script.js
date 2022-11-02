const openCompiler= () => {
    const container = document.getElementById("compiler-container");
    container.style.display = "block";
    container.onclick = (event)=> {
        if (event.target.id === container.id) closeCompiler();
    };
};
const closeCompiler = () => {
    document.getElementById("compiler-container").style.display = "none";
};
