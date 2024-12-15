document.getElementById('submitPersonalData').addEventListener('click', async () => {
    const AccountId = sessionStorage.getItem('AccountId');
    const personalData = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        personalCode: document.getElementById('personalCode').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        email: document.getElementById('email').value,
    };

    await addPersonData(AccountId, personalData);

    const personId = sessionStorage.getItem('PersonId');

    const address = {
        city: document.getElementById('city').value,
        street: document.getElementById('street').value,
        houseNumber: document.getElementById('houseNumber').value,
        apartmentNumber: document.getElementById('apartmentNumber').value,
    };

    await addPersonAddress(personId, address);

    const fileInput = document.getElementById('profilePhoto');
    if (fileInput.files.length > 0) {
        const file = fileInput.files[0];
        await uploadProfilePhoto(personId, file);
    }

    window.location.href = '../person/person.html';
});

async function addPersonData(AccountId, personalData) {
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
            credentials: 'include'
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(`Failed to add personal data: ${error}`);
        }

        const result = await response.json();
        sessionStorage.setItem('PersonId', result.personId);

    } catch (error) {
        console.error("Error adding personal data:", error.message);
    }
}

async function addPersonAddress(personId, address) {
    const token = getCookie('jwtToken');

    const requestData = { personId, ...address };

    try {
        const response = await fetch(`https://localhost:5100/api/Address`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData),
            credentials: 'include'
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(`Failed to add address: ${error}`);
        }

        console.log("Address added successfully!");

    } catch (error) {
        console.error("Error adding address:", error.message);
    }
}

async function uploadProfilePhoto(personId, file) {
    const token = getCookie('jwtToken');

    const formData = new FormData();
    formData.append('file', file);

    try {
        const response = await fetch(`https://localhost:5100/api/Picture/upload?personId=${personId}`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
            body: formData,
            credentials: 'include'
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
