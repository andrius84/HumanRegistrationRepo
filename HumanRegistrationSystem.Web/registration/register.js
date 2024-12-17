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

    if (username.length < 3) {
        showMessage("Vartotojo vardas turi būti bent 3 simbolių ilgio.", "warning");
        return;
    }
    
    if (password1 !== password2) {
        showMessage("Slaptažodžiai nesutampa.", "warning");
        return;
    }

    if (password1.length < 4 || password2.length < 4) {
        showMessage("Slaptažodis turi būti bent 4 simbolių ilgio.", "warning");
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

        } else if (response.status === 400) {
            const errorData = await response.json();
            let errorMessage = '';
            if (errorData.errors) {
                for (const field in errorData.errors) {
                    if (errorData.errors.hasOwnProperty(field)) {
                        errorMessage += errorData.errors[field].join(' ') + ' ';
                    }
                }
            } else if (errorData.message) {
                errorMessage = errorData.message;
            } else if (errorData) {
                errorMessage = errorData;
            } else {
                errorMessage = 'Nenustatyta klaida, bandykite dar kartą.';
            }
            showMessage(errorMessage.trim() || 'Klaida', 'error');
        } else {
            showMessage('Įvyko nežinoma klaida, bandykite dar kartą.', 'error');
        }
    } catch (error) {
        console.error('Network error:', error);
        showMessage('Tinklo klaida, bandykite dar kartą.', 'error');
    }
}

// async function registerUser() {
//     const username = document.getElementById("username").value.toLowerCase();
//     const password1 = document.getElementById("password1").value;
//     const password2 = document.getElementById("password2").value;

//     if (username.length < 3) {
//         showMessage("Vartotojo vardas turi būti bent 3 simbolių ilgio.", "warning");
//         return;
//     }
    
//     if (password1 !== password2) {
//         showMessage("Slaptažodžiai nesutampa.", "warning");
//         return;
//     }

//     if (password1.length < 4 || password2.length < 4) {
//         showMessage("Slaptažodis turi būti bent 4 simbolių ilgio.", "warning");
//         return;
//     }

//     const credentials = {
//         userName: username,
//         password: password1
//     };

//     try {
//         const response = await fetch('https://localhost:5100/api/Account/SignUp', {
//             method: 'POST',
//             headers: {
//                 'Content-Type': 'application/json'
//             },
//             body: JSON.stringify(credentials)
//         });

//         if (response.status === 201) {
//             const result = await response.json();
//             console.log("Registration successful, User ID:", result);

//             await getToken(username, password1);

//         } else if (response.status === 400) {
//             const errorData = await response.json();
//             let errorMessage = '';
//             if (errorData.errors) {
//                 for (const field in errorData.errors) {
//                     if (errorData.errors.hasOwnProperty(field)) {
//                         errorMessage += errorData.errors[field].join(' ') + ' ';
//                     }
//                 }
//             } else if (errorData.message) {
//                 errorMessage = errorData.message;
//             } else if (errorData) {
//                 errorMessage = errorData;
//             } else {
//                 errorMessage = 'Nenustatyta klaida, bandykite dar kartą.';
//             }
//             showMessage(errorMessage.trim() || 'Klaida', 'error');
//         } else {
//             showMessage('Įvyko nežinoma klaida, bandykite dar kartą.', 'error');
//         }
//     } catch (error) {
//         console.error('Network error:', error);
//         showMessage('Tinklo klaida, bandykite dar kartą.', 'error');
//     }

//     } catch (error) {
//         showMessage("Įvyko tinklo klaida. Bandykite dar kartą.", "error");
//         console.error('Error:', error.message || error);
//     }
// }


        // } else {
        //     const errorText = await response.text();
        //     showMessage(`Registracija nepavyko: ${errorText}`, "error");
        // }
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
            showMessage('Vartotojas nerastas, bandykite dar kartą.', 'error');
        } else {
            const error = await response.text();
            console.error("Login failed:", error);
            showMessage('Įvyko klaida. Bandykite dar kartą.', 'error');
        }
    } catch (error) {
        console.error("Network error:", error);
        showMessage('Tinklo klaida.', 'error');
    }
}

function showMessage(message, type = 'info', duration = 6000) {
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