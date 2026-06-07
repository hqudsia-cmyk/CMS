// CMS JavaScript utilities

document.addEventListener('DOMContentLoaded', function() {
    // Add any global interactions here
    console.log('CMS loaded');
});

// Function to move menu items
function moveMenuItem(itemId, direction) {
    fetch(`/api/menu/${itemId}/move?direction=${direction}`, {
        method: 'POST'
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            location.reload();
        }
    });
}

// Function to delete item with confirmation
function deleteItem(itemId, itemName) {
    if (confirm(`Are you sure you want to delete "${itemName}"?`)) {
        window.location.href = `/Admin/Delete/${itemId}`;
    }
}
