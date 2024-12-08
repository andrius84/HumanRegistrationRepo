document.addEventListener("DOMContentLoaded", function() {
    document.getElementById("submitPersonalData").addEventListener("click", addPersonalData);
    document.getElementById("backToLogin").addEventListener("click", function() {
        window.location.href = '../index.html';
    });
});

// Function to add personal data
async function addPersonalData(userId, personalData) {
    const token = getCookie('authToken'); // Retrieve the token from cookies
    if (!token) {
        console.error("No token found. Please log in.");
        return;
    }

    try {
        const response = await fetch(`https://localhost:5100/api/Person`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(personalData),
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(`Failed to add personal data: ${error}`);
        }

        console.log("Personal data added successfully!");
    } catch (error) {
        console.error("Error adding personal data:", error.message);
    }
}

// Function to upload a profile photo
async function uploadProfilePhoto(userId, file) {
    const token = getCookie('authToken'); // Retrieve the token from cookies
    if (!token) {
        console.error("No token found. Please log in.");
        return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
        const response = await fetch(`https://localhost:5100/api/Users/${userId}/ProfilePhoto`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
            body: formData,
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(`Failed to upload profile photo: ${error}`);
        }

        console.log("Profile photo uploaded successfully!");
    } catch (error) {
        console.error("Error uploading profile photo:", error.message);
    }
}

// Example usage
document.getElementById('submitPersonalData').addEventListener('click', async () => {
    const userId = "1234-5678-91011"; // Replace with actual user ID after registration
    const personalData = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        email: document.getElementById('email').value,
        address: document.getElementById('address').value,
    };

    await addPersonalData(userId, personalData);

    const fileInput = document.getElementById('profilePhoto');
    if (fileInput.files.length > 0) {
        const file = fileInput.files[0];
        await uploadProfilePhoto(userId, file);
    }
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
