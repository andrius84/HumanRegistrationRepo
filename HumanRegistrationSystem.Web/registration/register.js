document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("registerButton").addEventListener("click", registerUser);
    document.getElementById("backToLogin").addEventListener("click", function () {
        window.location.href = '../index.html';
    });
});

async function registerUser() {
    const username = document.getElementById("username").value.toLowerCase();
    const password1 = document.getElementById("password1").value;
    const password2 = document.getElementById("password2").value;
    const messageDiv = document.getElementById("message");

    clearMessage();

    if (password1 !== password2) {
        showMessage("Slaptažodžiai nesutampa. Bandykite dar kartą.", "error");
        return;
    }

    const credentials = {
        userName: username,
        password: password1
    };

    try {
        const response = await fetch('https://localhost:5100/api/Account/SignUp', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(credentials)
        });

        if (response.status === 201) {
            const result = await response.json();
            console.log("Registration successful, User ID:", result);

            await getToken(username, password1);

        } else {
            const errorText = await response.text();
            showMessage(`Registracija nepavyko: ${errorText}`, "error");
            console.error("Registration failed:", errorText);
        }
    } catch (error) {
        showMessage("Įvyko tinklo klaida. Bandykite dar kartą.", "error");
        console.error('Error:', error.message || error);
    }
}

async function getToken(username, password) {
    const credentials = {
        userName: username,
        password: password
    };

    try {
        const response = await fetch('https://localhost:5100/api/Account/Login', {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials),
            credentials: 'include'
        });

        if (response.ok) {
            const data = await response.json();
            console.log("Login successful");

            if (data) {
                sessionStorage.setItem("AccountId", data);
                console.log("Account ID saved in sessionStorage:", data);
            } else {
                console.warn("Account ID not found in response.");
            }

            window.location.href = '../persondata/persondata.html';
        } else if (response.status === 404) {
            showMessage('Vartotojas nerastas, bandykite dar kartą.');
        } else {
            const error = await response.text();
            console.error("Login failed:", error);
            showMessage('Įvyko klaida. Bandykite dar kartą.');
        }
    } catch (error) {
        console.error("Network error:", error);
        showMessage('Tinklo klaida. Bandykite dar kartą.');
    }
}

function showMessage(message, type = 'error') {
    const messageDiv = document.getElementById("message");
    messageDiv.style.display = 'block';
    messageDiv.style.padding = '10px';
    messageDiv.style.borderRadius = '5px';
    messageDiv.style.textAlign = 'center';
    if (type === 'error') {
        messageDiv.style.backgroundColor = 'red';
        messageDiv.style.color = 'white';
    } else {
        messageDiv.style.backgroundColor = 'green';
        messageDiv.style.color = 'white';
    }
    messageDiv.textContent = message;
}

function togglePasswordVisibility(passwordId) {
    var passwordField = document.getElementById(passwordId);
    var toggleButton = passwordField.nextElementSibling;

    if (passwordField.type === "password") {
        passwordField.type = "text";
        toggleButton.textContent = "🔓";
    } else {
        passwordField.type = "password";
        toggleButton.textContent = "🔒"; 
    }
}

function clearMessage() {
    const messageDiv = document.getElementById("message");
    if (messageDiv) {
        messageDiv.style.display = 'none';
        messageDiv.textContent = "";
    }
}