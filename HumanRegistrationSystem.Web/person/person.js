document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('deleteItem').addEventListener('click', deleteAccount);
    document.getElementById("logout").addEventListener("click", logout);
});

document.addEventListener("DOMContentLoaded", () => {
    const accountId = sessionStorage.getItem('AccountId');
    if (accountId) {
        fetchPersonData(accountId);
    } else {
        showMessage("Account ID nerastas arba neprisijungta.", 'error');
    }
});

async function fetchPersonData(accountId) {
    try {
        const personalData = await getPersonData(accountId);
        sessionStorage.setItem('PersonId', personalData.id);
        displayPersonData(personalData);
        displayEmail(personalData.email);
        const personId = personalData.id;
        const addressData = await getAddressData(personId);
        displayAddressData(addressData);

        await displayProfilePicture(personId);
    } catch (error) {
        console.error("Error fetching person data:", error.message);
    }
}

async function getPersonData(accountId) {
    const token = getCookie('jwtToken');
    if (!token) {
        throw new Error("Tokenas nerastas. Prisijunkite.");
    }

    const response = await fetch(`https://localhost:5100/api/Person/PersonByAccountId?accountId=${accountId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        credentials: 'include',
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error);
    }

    return await response.json();
}

async function getAddressData(personId) {
    const token = getCookie('jwtToken');

    const response = await fetch(`https://localhost:5100/api/Address/AddressByPersonId?personId=${personId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        credentials: 'include',
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error);
    }

    return await response.json();
}

async function getProfilePicture(personId) {
    const token = getCookie('jwtToken');

    try {
        const response = await fetch(`https://localhost:5100/api/Picture/${personId}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
            credentials: 'include',
        });

        if (!response.ok) {
            throw new Error(`Failed to fetch profile picture: ${response.status}`);
        }

        const arrayBuffer = await response.arrayBuffer();
        const byteArray = new Uint8Array(arrayBuffer);

        return byteArray;
    } catch (error) {
        console.error('Error fetching profile picture as byte array:', error);
        return null;
    }
}

function toggleEdit(fieldName) {
    const input = document.getElementById(`field-${fieldName}`);
    const saveIcon = input.nextElementSibling.nextElementSibling;
    var toggleButton = input.nextElementSibling;

    if (input.disabled) {
        input.disabled = false;
        saveIcon.disabled = false;
        saveIcon.style.opacity = '1';
        toggleButton.textContent = "🔓";
    } else {
        input.disabled = true;
        saveIcon.disabled = true;
        saveIcon.style.opacity = '0.5';
        toggleButton.textContent = "🔒"; 
    }
}

async function saveField(fieldName) {
    const input = document.getElementById(`field-${fieldName}`);
    const saveIcon = input.nextElementSibling.nextElementSibling;
    const fieldValue = input.value;

    if (!validatePersonalData(fieldName, fieldValue)) {
        return;
    }

    try {
        if (fieldName === 'firstName' || fieldName === 'lastName' || fieldName === 'personalCode' || fieldName === 'phoneNumber' || fieldName === 'email') {
            await updatePersonField(fieldName, fieldValue);
        }
        else {
            await updateAddressField(fieldName, fieldValue);
        }

        input.disabled = true;
        saveIcon.disabled = true;
        saveIcon.style.opacity = '0.5'; 

        showMessage(`${fieldName} sėkmingai atnaujintas.`, 'success');
    } catch (error) {
        console.error("Error updating field:", error.message);
        showMessage(`Klaida atnaujinant ${fieldName}. Bandykite dar kartą.`, 'error');
    }
}

async function updatePersonField(fieldKey, fieldValue) {

    const token = getCookie('jwtToken');
    const accountId = sessionStorage.getItem('AccountId');
    const response = await fetch(`https://localhost:5100/api/Person/${accountId}/${fieldKey}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(fieldValue),
        credentials: 'include'
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error);
    }
}

async function updateAddressField(fieldKey, fieldValue) {

    const token = getCookie('jwtToken');
    const personId = sessionStorage.getItem('PersonId');

    const response = await fetch(`https://localhost:5100/api/Address/${personId}/${fieldKey}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(fieldValue),
        credentials: 'include'
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error);
    }
}

function displayPersonData(personData) {
    document.getElementById('field-firstName').value = personData.firstName;
    document.getElementById('field-lastName').value = personData.lastName;
    document.getElementById('field-personalCode').value = personData.personalCode;
    document.getElementById('field-phoneNumber').value = personData.phoneNumber;
    document.getElementById('field-email').value = personData.email;
}

function displayAddressData(addressData) {
    document.getElementById('field-city').value = addressData.city;
    document.getElementById('field-street').value = addressData.street;
    document.getElementById('field-houseNumber').value = addressData.houseNumber;
    document.getElementById('field-apartmentNumber').value = addressData.apartmentNumber;
}

function displayEmail(email) {
    document.getElementById('showEmail').textContent = email;
}

async function displayProfilePicture(personId) {
    const byteArray = await getProfilePicture(personId);

    if (byteArray) {
        const blob = new Blob([byteArray], { type: 'image/jpeg' });
        const imageUrl = URL.createObjectURL(blob);
        const imgElement = document.getElementById('profilePictureContainer');
        imgElement.src = imageUrl;
    }
}

function getCookie(cookieName) {
    const cookies = document.cookie.split(';');
    for (const cookie of cookies) {
        const trimmedCookie = cookie.trim();
        if (trimmedCookie.startsWith(`${cookieName}=`)) {
            return trimmedCookie.substring(cookieName.length + 1);
        }
    }
    return null;
}

async function deleteAccount() {
    const accountId = sessionStorage.getItem('AccountId');
    const token = getCookie('jwtToken');

    try {
        const response = await fetch(`https://localhost:5100/api/Account/Delete/${accountId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
            credentials: 'include',
        });

        if (response.status === 403) {
            showMessage("Neturite teisių ištrinti paskyrą", 'warning');
            return;
        }

        if (!response.ok) {
            showMessage("Klaida ištrinant paskyrą. Bandykite dar kartą.", 'error');
            return;
        }

        if (response.ok)
        {
            showMessage("Vartotojas sėkmingai ištrintas", 'success');
            setTimeout(() => {
                logout();
            }, 4000);
        }

    } catch (error) {
        console.error("Error deleting account:", error.message);
    }
}

function showMessage(message, type = 'error', duration = 5000) {
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

function validatePersonalData(fieldKey, fieldValue) {

    if (fieldKey == 'firstName' && !fieldValue.trim()) {
        showMessage('Vardo laukas negali būti tuščias.', 'warning');
        return false;
    }
    if (fieldKey == 'lastName' && !fieldValue.trim()) {
        showMessage('Pavardės laukas negali būti tuščias.', 'warning');
        return false;
    }
    if (fieldKey == 'personalCode' && !fieldValue.replace(/\s+/g, '').trim()) {
        showMessage('Asmens kodo laukas negali būti tuščias.', 'warning');
        return false;
    }
    if (fieldKey == 'email' && !fieldValue.replace(/\s+/g, '').trim()) {
        showMessage('El. pašto laukas negali būti tuščias.', 'warning');
        return
    }
    if (fieldKey == 'phoneNumber' && !fieldValue.replace(/\s+/g, '').trim()) {
        showMessage('Telefono numerio laukas negali būti tuščias.', 'warning');
        return false;
    }
    
    return true;
}

function logout() {
    document.cookie = 'jwtToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    sessionStorage.clear();
    window.location.href = '../index.html';    
}