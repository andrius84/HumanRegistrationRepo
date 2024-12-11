document.addEventListener('DOMContentLoaded', function () {

    document.getElementById('createItem').addEventListener('click', createItem);
    document.getElementById("mainView").addEventListener("click", getItems);
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
        displayPersonData(personalData);
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

    const response = await fetch(`https://localhost:5100/api/Person/PersonById?accountId=${accountId}`, {
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

function displayPersonData(personData) {
    document.getElementById('field-firstName').value = personData.firstName;
    document.getElementById('field-lastName').value = personData.lastName;
    document.getElementById('field-personalCode').value = personData.personalCode;
    document.getElementById('field-phoneNumber').value = personData.phoneNumber;
    document.getElementById('field-email').value = personData.email;
}

function toggleEdit(fieldName) {
    const input = document.getElementById(`field-${fieldName}`);
    const lockIcon = input.nextElementSibling;
    const saveIcon = lockIcon.nextElementSibling;

    if (input.disabled) {
        input.disabled = false; 
        lockIcon.innerHTML = '🔓'; 
        lockIcon.onclick = () => toggleEdit(fieldName); 
        saveIcon.disabled = false; 
        saveIcon.style.opacity = '1'; 
    } else {
        input.disabled = true; 
        lockIcon.innerHTML = '🔒'; 
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
        await updatePersonField(accountId, fieldName, fieldValue);
        input.disabled = true; 
        saveIcon.disabled = true; 
        saveIcon.style.opacity = '0.5'; 
        document.getElementById('updateStatus').textContent = `Field "${fieldName}" updated successfully.`;
    } catch (error) {
        console.error("Error updating field:", error.message);
        document.getElementById('updateStatus').textContent = `Error updating field "${fieldName}": ${error.message}`;
    }
}

async function updatePersonField(accountId, fieldKey, fieldValue) {
    const token = getCookie('jwtToken');
    if (!token) {
        throw new Error("No token found. Please log in.");
    }

    const response = await fetch(`https://localhost:5100/api/Person/${accountId}/${fieldKey}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ value: fieldValue }),
        credentials: 'include',
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error);
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