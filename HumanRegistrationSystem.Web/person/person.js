document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('deleteItem').addEventListener('click', deleteAccount);
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
        document.getElementById('personDataDisplay').textContent = `Error fetching data: ${error.message}`;
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
    const token = getCookie('jwtToken'); // Retrieve your JWT token

    try {
        // Fetch the profile picture
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

        // Convert the response Blob into an ArrayBuffer
        const arrayBuffer = await response.arrayBuffer();

        // Convert ArrayBuffer into a Uint8Array (byte array)
        const byteArray = new Uint8Array(arrayBuffer);

        console.log('Byte Array:', byteArray);

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

    if (!fieldValue) {
        alert(`Please enter a value for ${fieldName}.`);
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

        // Optionally, show a message or alert
        alert(`${fieldName} updated successfully!`);
    } catch (error) {
        console.error("Error updating field:", error.message);
        alert(`Error updating ${fieldName}: ${error.message}`);
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
        // Create a Blob from the byte array
        const blob = new Blob([byteArray], { type: 'image/jpeg' });

        // Create an Object URL
        const imageUrl = URL.createObjectURL(blob);

        // Set it as the `src` for an image element
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

function deleteAccount() {
    const accountId = sessionStorage.getItem('AccountId');
    const token = getCookie('jwtToken');

    const response = fetch(`https://localhost:5100/api/Account/Delete/${accountId}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        credentials: 'include',
    });

    if (!response.ok) {
        const error = response.text();
        throw new Error(error);
    }

    sessionStorage.clear();
    window.location.href = '../index.html';
}

function logout() {
    document.cookie = 'jwtToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    sessionStorage.clear();
    window.location.href = '../index.html';    
}