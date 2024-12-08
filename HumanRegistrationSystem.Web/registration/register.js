document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("registerButton").addEventListener("click", GetUser);
    document.getElementById("backToLogin").addEventListener("click", function() {
        window.location.href = '../index.html';
    });
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

function GetUser(username, password) {
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
      } else {
        registerUser();
      }
    })
    .then(data => {
            console.log('User exist', data);
            showMessage('Toks vartotojas jau egzistuoja!');
    })
    .catch(error => {
        console.error('Error:', error);
    });
}

function registerUser() {
    const name = document.getElementById("username").value;
    const username = name.toLowerCase();
    const password = document.getElementById("password").value;
    const email = document.getElementById("email").value;  // Retrieve email value

    const credentials = {
        userName: username,
        password: password,
        email: email
    };

    // Call the registration API
    fetch('https://localhost:5100/api/Auth/SignUp', {  // Make sure the endpoint matches your API
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(credentials)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Registration failed with status: ' + response.status);
        }
        return response.json(); // Parse the response if successful
    })
    .then(data => {
        console.log('User registered successfully:', data);  // Log the response data (user details or ID)
        window.location.href = '../toDoApp/toDoApp.html';  // Redirect after successful registration
    })
    .catch(error => {
        console.error('Error:', error);  // Handle any errors
    });
}
//////////////////////////////
async function registerUser() {
    const credentials = {
        userName: "johndoe",
        password: "SecurePassword123"
    };

    try {
        const response = await fetch("/api/auth/SignUp", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials)
        });

        if (response.status === 201) {
            const result = await response.json();
            console.log("Registration successful, User ID:", result.id);
            window.location.href = '../toDoApp/toDoApp.html'
        } else {
            const error = await response.text();
            console.error("Registration failed:", error);
        }
    } catch (error) {
        console.error("Network error:", error);
    }
}