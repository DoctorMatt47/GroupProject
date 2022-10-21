const info_btn = document.getElementsByClassName("info-btn")
for (let i = 0; i < info_btn.length; i++) {
    info_btn[i].onclick = () => {
        document.querySelector(".container").classList.toggle("log-in");
    };
}
//Validation
class FormValidator {
    constructor(form, fields) {
        this.form = form
        this.fields = fields
    }

    initialize() {
        this.validateOnEntry()
        this.validateOnSubmit()
    }

    validateOnSubmit() {
        let self = this

        this.form.addEventListener('submit', event => {
            event.preventDefault()
            self.fields.forEach(field => {
                const input = document.querySelector(`#${field}`)
                self.validateFields(input)
            })
        })
    }

    validateOnEntry() {
        let self = this
        this.fields.forEach(field => {
            const input = document.querySelector(`#${field}`)

            input.addEventListener('input', () => {
                self.validateFields(input)
            })
        })
    }

    validateFields(field) {

        // Check presence of values
        if (field.value.trim() === "") {
            this.setStatus(field, null, "error")
        } else {
            this.setStatus(field, null, "success")
        }

        // Password confirmation edge case
        if (field.id === "password_confirmation") {
            const passwordField = this.form.querySelector('#registration-password')

            if (field.value.trim() == "") {
                this.setStatus(field, "Password confirmation required", "error")
            } else if (field.value != passwordField.value) {
                this.setStatus(field, "Password does not match", "error")
            } else {
                this.setStatus(field, null, "success")
            }
        }
    }

    setStatus(field, message, status) {
        const successIcon = field.parentElement.querySelector('.icon-success')
        const errorIcon = field.parentElement.querySelector('.icon-error')
        const errorMessage = field.parentElement.querySelector('.error-message')

        if (status === "success") {
            if (errorIcon) {
                errorIcon.classList.add('hidden')
            }

            successIcon.classList.remove('hidden')
            field.classList.remove('input-error')
        }

        if (status === "error") {
            if (successIcon) {
                successIcon.classList.add('hidden')
            }

            errorIcon.classList.remove('hidden')
            field.classList.add('input-error')
        }
    }
}

const form = document.querySelector('.form')
const fields = ["registration-username", "registration-password", "password_confirmation"]
const validator = new FormValidator(form, fields)
validator.initialize()

//Registration-Authentifiation
const registrationLoginInput = document.getElementById("registration-username");
const registrationPasswordInput = document.getElementById("registration-password");

const authenticationLoginInput = document.getElementById("username");
const authenticationPasswordInput = document.getElementById("password");

const logElement = document.getElementById("log");

// TODO: Move to common js file
const baseUrl = "http://localhost:5000/api/";
const sendAsync = async (endpoint, request) => {
    const response = await fetch(endpoint, request);
    if (!response.ok) {
        const text = await response.text()
        throw new Error(text);
    }
    return response.json();
}

const register = () => {
    const login = registrationLoginInput.value;
    const password = registrationPasswordInput.value;

    const body = {login: login, password: password}

    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    }
    sendAsync(baseUrl + "Users", request)
        .then(response => {
            console.log(response);
            logElement.innerText = `Registered! ${response.id}`;
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception);
            logElement.innerText = `Error! ${exception.message}`;
        })
}

const authenticate = () => {
    const login = authenticationLoginInput.value;
    const password = authenticationPasswordInput.value;

    const body = {login: login, password: password}

    const request = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    }
    sendAsync(baseUrl + "Users/Authenticate", request)
        .then(response => {
            console.log(response)
            logElement.innerText = `Authenticated! ${response.id}`;

            // Push auth token to local storage
            window.localStorage.setItem("token", response.token);
        })
        .catch(error => {
            const exception = JSON.parse(error.message)
            console.log(exception);
            logElement.innerText = `Error! ${exception.message}`;
        })
}