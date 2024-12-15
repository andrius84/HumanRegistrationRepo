document.getElementById("loginButton").addEventListener("click", loginUser);

document.getElementById("registerButton").addEventListener("click", function() {
  window.location.href = 'registration/register.html';
});

async function loginUser() {
    const username = document.getElementById("username").value.toLowerCase();
    const password = document.getElementById("password").value;

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

            if (data) {
                sessionStorage.setItem("AccountId", data);
            } else {
                console.warn("accountId not found in response.");
            }

            window.location.href = 'person/person.html';

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

function showMessage(message) {
    let messageDiv = document.getElementById("message");
    if (!messageDiv) {
        messageDiv = document.createElement("div");
        messageDiv.id = "message";
        document.body.appendChild(messageDiv);
    }
    messageDiv.style.display = 'block';
    messageDiv.style.backgroundColor = 'red';
    messageDiv.style.color = 'white';
    messageDiv.style.padding = '5px';
    messageDiv.style.borderRadius = '5px';
    messageDiv.style.textAlign = 'center';
    
    messageDiv.textContent = message;
  }