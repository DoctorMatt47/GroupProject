const openForm = () =>{
    const container = document.getElementById("fullscreen-container");
    container.style.display = "block";
    container.onclick = (event)=>{
        if(event.target.id === container.id) closeForm();
    };
};

const closeForm = () => {
    document.getElementById("fullscreen-container").style.display = "none";
};
