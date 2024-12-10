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