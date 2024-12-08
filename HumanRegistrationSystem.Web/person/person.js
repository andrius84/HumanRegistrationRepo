document.addEventListener('DOMContentLoaded', function () {

    document.getElementById('createItem').addEventListener('click', createItem);
    document.getElementById("mainView").addEventListener("click", getItems);
});

function getItems() {
    const url = `https://localhost:7171/api/ToDo`; 

    fetch(url, {
        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error fetching items: ' + response.status);
        }
        return response.json();
    })
    .then(data => {
        console.log('Fetched items:', data);
        displayItems(data); 
    })
    .catch(error => {
        console.error('Error:', error);
    });
}

function displayItems(items) {
    const itemsContainer = document.getElementById('itemsContainer');
    const userId = sessionStorage.getItem('userId').slice(1, -1);
    console.log('Displaying items for userId:', userId);

    itemsContainer.innerHTML = '';

    const filteredItems = items.filter(item => item.userId === userId);

    if (filteredItems.length === 0) {
        itemsContainer.innerHTML = '<p>Nerastas nei vienas šio vartotojo įrašas</p>';
        return;
    }

    filteredItems.forEach(item => {
        const itemDiv = document.createElement('div');
        itemDiv.className = 'item';

        itemDiv.innerHTML = `
            <p><strong>Įrašo numeris:</strong> ${item.id}</p>
            <p><strong>Pavadinimas:</strong> ${item.type}</p>
            <p><strong>Aprašymas:</strong> ${item.content}</p>
            <p><strong>Galiojimo data iki:</strong> ${new Date(item.endDate).toLocaleString()}</p>
        `;

        itemsContainer.appendChild(itemDiv);
    });
}

function createItem() {
    const formContainer = document.getElementById('itemsContainer');
    
    formContainer.innerHTML = '';

    const userId = sessionStorage.getItem('userId').slice(1, -1);

    const typeField = createInputField('type', 'Pavadinimas');
    const contentField = createInputField('content', 'Aprašymas');
    const endDateField = createInputField('endDate', 'Data');

    formContainer.appendChild(typeField);
    formContainer.appendChild(contentField);
    formContainer.appendChild(endDateField);

    const saveButton = document.createElement('button');
    saveButton.id = 'saveButton';
    saveButton.innerText = 'Save';

    formContainer.appendChild(saveButton);

    saveButton.addEventListener('click', function () {

        const requestBody = {
            userId: userId, 
            type: document.getElementById('type').value,
            content: document.getElementById('content').value,
            endDate: new Date(document.getElementById('endDate').value).toISOString() 
        };

        fetch('https://localhost:7171/api/ToDo', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
    });
}

function deleteItem(itemId) {
    const url = `https://localhost:7171/api/ToDo/${itemId}`;

    fetch(url, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error deleting item: ' + response.status);
        }
    })
    .then(data => {
        console.log('Success:', data);
        getItems();
    })
    .catch(error => {
        console.error('Error:', error);
    });
}

