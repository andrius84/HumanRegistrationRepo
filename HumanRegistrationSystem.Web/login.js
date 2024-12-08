document.getElementById("loginButton").addEventListener("click", loginUser);

document.getElementById("registerButton").addEventListener("click", function() {
  window.location.href = 'registration/register.html';
});

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

function loginUser() {
  const username = document.getElementById("username").value.toLowerCase();
  const password = document.getElementById("password").value;

  const url = `https://localhost:5100/api/Auth?username=${username}&password=${password}`;

  fetch(url, {
      method: 'GET',
      headers: {
          'Content-Type': 'application/json' 
      }
    })
  .then(response => {
        if (response.ok) {
            return response.json();
        } else if (response.status === 404) {
            showMessage('Vartotojas nerastas, bandykite dar kartą.');
        } else {
            showMessage('Įvyko klaida. Bandykite dar kartą.');
        }
    })
  .then(data => {
      if (data.userName === username) {
          console.log('Login successful:', data);

          sessionStorage.setItem('userId', JSON.stringify(data.id));
          sessionStorage.setItem('userName', JSON.stringify(data.userName));
          sessionStorage.setItem('email', JSON.stringify(data.email));

          window.location.href = 'toDoApp/toDoApp.html'; 
      } else {
          console.log('Login failed: ' + (data.message));
      }
    })
  .catch(error => {
      console.error('Error:', error);
    });
}

//////////////////////////////


async function loginUser() {
    const credentials = {
        userName: "johndoe",
        password: "SecurePassword123"
    };

    try {
        const response = await fetch("/api/auth/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials)
        });

        if (response.status === 200) {
            const token = await response.text(); // JWT is returned as plain text.
            console.log("Login successful, JWT:", token);
            localStorage.setItem("jwtToken", token); // Store JWT for future use.
        } else {
            const error = await response.text();
            console.error("Login failed:", error);
        }
    } catch (error) {
        console.error("Network error:", error);
    }
}

async function loginUser() {
    const credentials = {
        userName: "johndoe",
        password: "SecurePassword123"
    };

    try {
        const response = await fetch("/api/auth/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials),
            credentials: "include"  // This ensures cookies are sent with the request
        });

        if (response.status === 200) {
            console.log("Login successful");
            // No need to handle JWT manually, it's stored in the cookie
        } else {
            const error = await response.text();
            console.error("Login failed:", error);
        }
    } catch (error) {
        console.error("Network error:", error);
    }
}