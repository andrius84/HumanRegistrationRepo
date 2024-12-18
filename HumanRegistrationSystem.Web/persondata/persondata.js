document.getElementById('submitPersonalData').addEventListener('click', async () => {
    const AccountId = sessionStorage.getItem('AccountId');
    const personalData = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        personalCode: document.getElementById('personalCode').value,
        phoneNumber: document.getElementById('phoneNumber').value,
        email: document.getElementById('email').value,
    };

    let successCounter = 0;

    if (!validatePersonalData(personalData)) {
        return;
    }

    try {
        await addPersonData(AccountId, personalData);
        successCounter++;
    } catch (error) {
        showMessage("Error updating personal data. Please try again.", 'error');
        return;
    }

    const personId = sessionStorage.getItem('PersonId');
    const address = {
        city: document.getElementById('city').value,
        street: document.getElementById('street').value,
        houseNumber: document.getElementById('houseNumber').value,
        apartmentNumber: document.getElementById('apartmentNumber').value,
    };

    if (!validateAddress(address)) {
        return;
    }

    try {
        await addPersonAddress(personId, address);
        successCounter++;
    } catch (error) {
        showMessage("Error updating address. Please try again.", 'error');
        return;
    }

    const fileInput = document.getElementById('profilePhoto');
    if (fileInput.files.length > 0) {
        const file = fileInput.files[0];
        try {
            await uploadProfilePhoto(personId, file);
            successCounter++;
        } catch (error) {
            showMessage("Error uploading profile photo. Please try again.", 'error');
            return;
        }
    }

    if (successCounter === 3) {
        showMessage("Asmens informacija sėkmingai įrašyta", 'success');
        showContinueButton();
    }
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

        console.log("Personal data added successfully!");

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
        formData.append("FileName", file.name);
        formData.append("ContentType", file.type);
        formData.append("Data", file); 
        formData.append("PersonId", personId);

    try {
        const response = await fetch(`https://localhost:5100/api/Picture/upload/${personId}`, {
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

function showContinueButton() {
    const continueButton = document.getElementById('continueButton');

    continueButton.disabled = false;

    continueButton.addEventListener('click', () => {
        window.location.href = '../person/person.html';
    });

    continueButton.style.display = 'block';
}

document.getElementById('submitPersonalData').addEventListener('click', function(event) {
    event.preventDefault(); 
    showContinueButton();
});


function validatePersonalData(personalData) {
    const { firstName, lastName, phoneNumber, email, personalCode } = personalData;

    if (!firstName.trim()) {
        showMessage('Vardo laukas negali būti tuščias.', 'warning');
        return false;
    }
    if (!lastName.trim()) {
        showMessage('Pavardės laukas negali būti tuščias.', 'warning');
        return false;
    }
    if (!personalCode.match(/^\d{11}$/)) {
        showMessage('Asmens kodas turi buti sudarytas is 11 skaitmenų.', 'warning');
        return false;
    }
    if (!phoneNumber.match(/^\d{9,15}$/)) {
        showMessage('Telefono numerį turi sudaryti 9-15 skaičių.', 'warning');
        return false;
    }
    if (!email.match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)) {
        showMessage('Neteisingas el. pašto adresas.', 'warning');
        return false;
    }

    return true;
}

function validateAddress(address) {
    const { city, street, houseNumber, apartmentNumber } = address;

    if (!city.trim()) {
        showMessage('Įveskite miesto pavadinimą.', 'warning');
        return false;
    }

    if (!street.trim()) {
        showMessage('Įveskite gatvės pavadinimą.', 'warning');
        return false;
    }

    const houseNumberRegex = /^[a-zA-Z0-9\s]+$/;
    if (!houseNumberRegex.test(houseNumber)) {
        showMessage('Namų numeris turi būti sudarytas iš skaičių.', 'warning');
        return false;
    }

    return true;
}