const loadData = ()=>{
    const sectionId = getValueFromCurrentUrl("id");
    getSections().then((response) => {
        const section = response.find(item=>item.id === sectionId);
        console.log(section)
        
    }).catch((err) => {
        const exception = JSON.parse(err.message);
        openErrorWindow(exception.message);
    });
};
window.addEventListener("load", loadData);