document.addEventListener('DOMContentLoaded', function () {
    const contactButton = document.querySelector('a[data-bs-target="#offcanvasContact"]');
    const offcanvasContact = new bootstrap.Offcanvas(document.getElementById('offcanvasContact'));

    contactButton.addEventListener('click', function () {
        offcanvasContact.show();
    });
});