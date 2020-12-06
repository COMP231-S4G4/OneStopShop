function registerBtn1() {
    validateEmail();
}
function validateEmail() {
    var emailInput = document.getElementById("email");
    var errorDiv = document.getElementById("emailError");
    try {
        if (emailInput.value.search("@") === -1 || emailInput.value.lastIndexOf(".") === -1) {
            throw "Please provide a valid email address.";
        }

    }
    catch (msg) {
        errorDiv.innerHTML = msg;
        errorDiv.style.display = "block";
        emailInput.style.background = "rgb(255,233,233)";
    }
}

function createEventListener() {
    var registerBtn = document.getElementById("btn1");

    if (registerBtn.addEventListener) {
        registerBtn.addEventListener("click", registerBtn1, false);
    }
    else if (registerBtn.attachEvent) {
        registerBtn.attachEvent("onclick", registerBtn1);
    }
}

function setUpPage() {
    createEventListener();
    //customValidity();
}

if (window.addEventListener) {
    window.addEventListener("load", setUpPage, false);
} else if (window.attachEvent) {
    window.attachEvent("onload", setUpPage);
}