
    // Función para establecer la fecha mínima en el campo de "Check In" y "Check Out"
    function setMinDates() {
        const today = new Date().toISOString().split('T')[0];
        document.getElementById('checkin').min = today;
        document.getElementById('checkout').min = today;
    }

    // Establece la fecha mínima al cargar la página
    setMinDates();

    // Evento para actualizar la fecha de "Check Out" cuando se selecciona una fecha de "Check In"
    document.getElementById('checkin').addEventListener('change', function () {
        let checkinDate = new Date(this.value);
        checkinDate.setDate(checkinDate.getDate() + 1); // Añade 1 día
        document.getElementById('checkout').valueAsDate = checkinDate;
        document.getElementById('checkout').min = this.value; // Establece como mínimo la fecha de Check In
    });

