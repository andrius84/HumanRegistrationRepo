document.getElementById('submitPersonalData').addEventListener('click', async () => {
    const AccountId = sessionStorage.getItem('AccountId');
    const personalData = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        personalCode: document.getElementById('personalCode').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        email: document.getElementById('email').value,  
    };

    await addPersonalData(AccountId, personalData);

    const PersonId = sessionStorage.getItem('PersonId');

    const fileInput = document.getElementById('profilePhoto');
    if (fileInput.files.length > 0) {
        const file = fileInput.files[0];
        await uploadProfilePhoto(PersonId, file);
    }
});

async function addPersonalData(AccountId, personalData) {
    const token = getCookie('jwtToken'); 
    if (!token) {
        console.error("No token found. Please log in.");
        return;
    }

    const requestData = { AccountId, ...personalData };

    try {
        const response = await fetch(`https://localhost:5100/api/Person/`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData),
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(`Failed to add personal data: ${error}`);
        }

        const result = await response.json();
        console.log("Personal data added successfully! Created ID:", result.personId);

        sessionStorage.setItem('personId', result.personId);
        console.log("Person ID saved to sessionStorage:", result.personId);

    } catch (error) {
        console.error("Error adding personal data:", error.message);
    }
}

async function uploadProfilePhoto(PersonId, file) {
    const token = getCookie('authToken');
    if (!token) {
        console.error("No token found. Please log in.");
        return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
        const response = await fetch(`https://localhost:5100/api/Picture/upload?personId=${PersonId}`, {
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

function getCookie(cookieName) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        if (cookie.startsWith(`${cookieName}=`)) {
            return cookie.substring(cookieName.length + 1);
        }
    }
    return null;
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
