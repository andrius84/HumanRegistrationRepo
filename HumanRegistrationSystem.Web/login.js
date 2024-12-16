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

        } else {
            const errorData = await response.json(); 

            if (response.status === 400) {
                let errorMessage = '';
                for (const field in errorData.errors) {
                    if (errorData.errors.hasOwnProperty(field)) {
                        errorMessage += errorData.errors[field].join(' ') + ' ';
                    }
                }
                showMessage(errorMessage.trim(), 'error');
            } else {
                showMessage(errorData.title || 'Unknown error occurred.', 'error');
            }
        }
    }
    catch (error) {
        showMessage('Nenustatyta klaida, bandykite dar kartą.');
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

function showMessage(message, type = 'info', duration = 5000) {
    const container = document.querySelector('.container');
    let messageDiv = document.getElementById("message");

    if (!messageDiv) {
        messageDiv = document.createElement("div");
        messageDiv.id = "message";
        document.body.appendChild(messageDiv);
    }

    messageDiv.className = ''; 
    messageDiv.classList.add(`message-${type}`);
    messageDiv.textContent = message;
    messageDiv.style.display = 'block';

    setTimeout(() => {
        messageDiv.style.display = 'none';
    }, duration);
}