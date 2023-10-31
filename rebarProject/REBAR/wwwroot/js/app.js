function loadMenu() {
    console.log("loadMenu function was called");
    fetch('https://localhost:7202/menu')
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to load menu");
            }
            return response.json();
        })
        .then(data => {
            let output = '<ul>';
            for (let shake of data.shakes) {
                output += `<li>${shake.name} - ${shake.description}</li>`;
            }
            output += '</ul>';
            document.getElementById('menu').innerHTML = output;
        })
        .catch(error => {
            console.error("There was an error fetching the menu:", error);
        });
}
document.addEventListener('DOMContentLoaded', function () {
    fetchShakes();

    const orderForm = document.getElementById('orderForm');
    orderForm.addEventListener('submit', placeOrder);
});

function fetchShakes() {
    fetch('https://localhost:7202/menu')
        .then(response => response.json())
        .then(data => {
            const shakesSelect = document.getElementById('shakes');
            data.shakes.forEach(shake => {
                let option = document.createElement('option');
                option.value = shake.id;
                option.text = shake.name;
                shakesSelect.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error fetching shakes:", error);
        });
}

function placeOrder(event) {
    event.preventDefault();

    const customerName = document.getElementById('customerName').value;
    const selectedShakes = Array.from(document.getElementById('shakes').selectedOptions).map(option => option.value);

    const order = {
        customerName: customerName,
        shakes: selectedShakes
    };

    fetch('https://localhost:7202/orders', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(order)
    })
        .then(response => response.json())
        .then(data => {
            alert("Order placed successfully!");
        })
        .catch(error => {
            console.error("Error placing order:", error);
        });
}

