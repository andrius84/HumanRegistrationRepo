document.addEventListener('DOMContentLoaded', function () {

    document.getElementById('deleteItem').addEventListener('click', deleteItem);
    document.getElementById("logout").addEventListener("click", logout);
});

document.addEventListener("DOMContentLoaded", () => {
    const accountId = sessionStorage.getItem('AccountId');
    if (accountId) {
        fetchPersonData(accountId);
    } else {
        alert("Account ID not found in session storage. Please log in.");
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const personId = sessionStorage.getItem('PersonId');
    if (personId) {
        fetchAddressData(personId);
    } else {
        alert("Person ID not found in session storage. Please log in.");
    }
});

async function fetchPersonData(accountId) {
    try {
        const personalData = await getPersonData(accountId);
        sessionStorage.setItem('PersonId', personalData.id);
        displayPersonData(personalData);
    } catch (error) {
        console.error("Error fetching person data:", error.message);
        document.getElementById('personDataDisplay').textContent = `Error fetching data: ${error.message}`;
    }
}

async function fetchAddressData(personId) {
    try {
        const addressData = await getAddressData(personId);
        displayAddressData(addressData);
    } catch (error) {
        console.error("Error fetching address data:", error.message);
        document.getElementById('addressDataDisplay').textContent = `Error fetching data: ${error.message}`;
    }
}

async function getPersonData(accountId) {
    const token = getCookie('jwtToken');
    if (!token) {
        throw new Error("No token found. Please log in.");
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
    if (!token) {
        throw new Error("No token found. Please log in.");
    }

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

function toggleEdit(fieldName) {
    const input = document.getElementById(`field-${fieldName}`);
    const saveIcon = input.nextElementSibling.nextElementSibling;

    if (input.disabled) {
        input.disabled = false;
        saveIcon.disabled = false;
        saveIcon.style.opacity = '1'; 
    } else {
        input.disabled = true;
        saveIcon.disabled = true;
        saveIcon.style.opacity = '0.5'; 
    }
}

async function saveField(fieldName) {
    const input = document.getElementById(`field-${fieldName}`);
    const saveIcon = input.nextElementSibling.nextElementSibling;
    const fieldValue = input.value;

    if (!fieldValue) {
        alert(`Please enter a value for ${fieldName}.`);
        return;
    }

    const accountId = sessionStorage.getItem('AccountId');
    try {
        if (fieldName === 'firstName' || 'lastName' || 'personalCode' || 'phoneNumber' || 'email') {
            await updatePersonField(accountId, fieldName, fieldValue);
        }
        else {
            await updateAddressField(personId, fieldName, fieldValue);
        }

        input.disabled = true;
        saveIcon.disabled = true;
        saveIcon.style.opacity = '0.5'; 

        // Optionally, show a message or alert
        alert(`${fieldName} updated successfully!`);
    } catch (error) {
        console.error("Error updating field:", error.message);
        alert(`Error updating ${fieldName}: ${error.message}`);
    }
}

async function updatePersonField(accountId, fieldKey, fieldValue) {
    const token = getCookie('jwtToken');
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

async function updateAddressField(personId, fieldKey, fieldValue) {
    const token = getCookie('jwtToken');
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