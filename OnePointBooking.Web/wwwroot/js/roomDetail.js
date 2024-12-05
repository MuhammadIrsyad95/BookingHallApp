function togglePackages(roomId) {
    var roomPackages = document.getElementById("roomPackages-" + roomId);
    var chevronIcon = document.getElementById("chevronIcon-" + roomId);

    if (roomPackages.style.display === "none") {
        roomPackages.style.display = "block";
        chevronIcon.classList.remove("bi-chevron-double-down");
        chevronIcon.classList.add("bi-chevron-double-up");
    } else {
        roomPackages.style.display = "none";
        chevronIcon.classList.remove("bi-chevron-double-up");
        chevronIcon.classList.add("bi-chevron-double-down");
    }
}
