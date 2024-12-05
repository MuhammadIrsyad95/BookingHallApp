function updateTotalCost() {
    const days = parseInt(document.querySelector('input[name="Days"]').value) || 1;
    const packageSelect = document.querySelector('#RoomPackage');
    const packagePrice = parseFloat(packageSelect.options[packageSelect.selectedIndex].dataset.price) || 0;
    const totalCost = packagePrice * days;

    document.getElementById('BookingTotal').innerText = new Intl.NumberFormat('id-ID', { style: 'currency', currency: 'IDR' }).format(totalCost);
    document.getElementById('TotalCost').value = totalCost;
    document.getElementById('RoomPackageId').value = packageSelect.value;
    document.getElementById('RoomSetupId').value = document.querySelector('#RoomSetup').value;

    // Show the selected room package details
    const selectedPackageId = packageSelect.value;

    // Hide all package cards first
    const allCards = document.querySelectorAll('.room-package-card');
    allCards.forEach(card => {
        card.style.display = 'none'; // Hide all cards
    });

    // Show the selected package card
    const selectedCard = document.querySelector(`.room-package-card[data-package-id="${selectedPackageId}"]`);
    if (selectedCard) {
        selectedCard.style.display = 'block'; // Show the selected package card
    }

    // Get selected package details
    const selectedPackage = Array.from(packageSelect.options).find(option => option.value == selectedPackageId);
    const amenitiesList = selectedPackage ? JSON.parse(selectedPackage.dataset.amenities) : [];

    // Update amenities list (if necessary)
    const amenitiesContainer = document.getElementById('amenitiesList');
    amenitiesContainer.innerHTML = '';
    amenitiesList.forEach(amenity => {
        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.textContent = amenity;
        amenitiesContainer.appendChild(li);
    });
}
