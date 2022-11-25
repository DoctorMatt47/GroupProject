const openBlockVerificationWindow = (action) => {
    document.getElementById("block-container").style.display = "block";
    document.getElementById("block-button").onclick = ()=>{
        action();
        closeBlockVerificationWindow();
    };
};
const closeBlockVerificationWindow = () => {
    document.getElementById("block-container").style.display = "none";
};

const openLeaveVerificationWindow = () => {
    document.getElementById("leave-container").style.display = "block";
};
const closeLeaveVerificationWindow = () => {
    document.getElementById("leave-container").style.display = "none";
};

const openEndVerificationWindow = () => {
    document.getElementById("end-container").style.display = "block";
};
const closeEndVerificationWindow = () => {
    document.getElementById("end-container").style.display = "none";
};

const openDeleteVerificationWindow = () => {
    document.getElementById("delete-container").style.display = "block";
};
const closeDeleteVerificationWindow = () => {
    document.getElementById("delete-container").style.display = "none";
};
