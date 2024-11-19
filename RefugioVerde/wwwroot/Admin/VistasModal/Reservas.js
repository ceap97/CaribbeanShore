document.addEventListener('DOMContentLoaded', function () {
    fetch('/Reservas/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbReservas tbody');
            data.forEach(reserva => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${reserva.reservaId}</td>
                    <td>${new Date(reserva.fechaReserva).toLocaleDateString()}</td>
                    <td>${reserva.cliente.nombre}</td>
                    <td>${reserva.habitacion.nombreHabitacion}</td>
                    <td>${reserva.estadoReserva.nombre}</td>
                    <td>${new Date(reserva.fechaInicio).toLocaleDateString()}</td>
                    <td>${new Date(reserva.fechaFin).toLocaleDateString()}</td>
                    <td>
                        <div class="action-buttons">
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${reserva.reservaId})">
                                <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${reserva.reservaId})">
                                <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${reserva.reservaId})">
                                <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                        </div>
                    </td>
                `;
                tbody.appendChild(tr);
            });
            let table = new DataTable('#tbReservas', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

function openCreateModal() {
    loadClientes('#clienteId');
    loadHabitaciones('#habitacionId');
    loadEstadosReserva('#estadoReserva');
    $('#createModal').modal('show');
   
}

function openEditModal(reservaId) {
    fetch(`/Reservas/Obtener/${reservaId}`)
        .then(response => response.json())
        .then(data => {
            $('#editReservaId').val(data.reservaId);
            $('#editFechaReserva').val(data.fechaReserva.split('T')[0]);
            $('#editClienteId').val(data.clienteId);
            $('#editHabitacionId').val(data.habitacionId);
            $('#editEstadoReservaId').val(data.estadoReservaId);
            $('#editFechaInicio').val(data.fechaInicio.split('T')[0]);
            $('#editFechaFin').val(data.fechaFin.split('T')[0]);
            $('#editModal').modal('show');
            loadClientes(); // Cargar clientes
            loadHabitaciones(); // Cargar habitaciones
            loadEstadosReserva(); // Cargar estados
        });
}

function openDeleteModal(reservaId) {
    $('#confirmDelete').data('reservaId', reservaId);
    $('#deleteModal').modal('show');
}

function openDetailsModal(reservaId) {
    fetch(`/Reservas/Obtener/${reservaId}`)
        .then(response => response.json())
        .then(data => {
            $('#detailsReservaId').text(data.reservaId);
            $('#detailsFechaReserva').text(new Date(data.fechaReserva).toLocaleDateString());
            $('#detailsCliente').text(data.cliente.nombre);
            $('#detailsHabitacion').text(data.habitacion.nombreHabitacion);
            $('#detailsEstadoReserva').text(data.estadoReserva.nombre);
            $('#detailsFechaInicio').text(new Date(data.fechaInicio).toLocaleDateString());
            $('#detailsFechaFin').text(new Date(data.fechaFin).toLocaleDateString());
            $('#detailsModal').modal('show');
        });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    fetch('/Reservas/Crear', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

$('#editForm').submit(function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    fetch('/Reservas/Editar', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

$('#confirmDelete').click(function () {
    let reservaId = $(this).data('reservaId');
    fetch(`/Reservas/Eliminar/${reservaId}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

function loadClientes() {
    fetch('/Clientes/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let clienteSelects = document.querySelectorAll('#clienteId, #editClienteId');
            clienteSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione un Cliente</option>`;
                data.forEach(cliente => {
                    select.innerHTML += `<option value="${cliente.clienteId}">${cliente.nombre}</option>`;
                });
            });
        });
}

function loadHabitaciones() {
    fetch('/Habitaciones/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let habitacionSelects = document.querySelectorAll('#habitacionId, #editHabitacionId');
            habitacionSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione una Habitación</option>`;
                data.forEach(habitacion => {
                    select.innerHTML += `<option value="${habitacion.habitacionId}">${habitacion.nombreHabitacion}</option>`;
                });
            });
        });
}

function loadEstadosReserva() {
    fetch('/EstadoReservas/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let estadoReservaSelects = document.querySelectorAll('#estadoReservaId, #editEstadoReservaId');
            estadoReservaSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione un Estado de Reserva</option>`;
                data.forEach(estadoReserva => {
                    select.innerHTML += `<option value="${estadoReserva.estadoReservaId}">${estadoReserva.nombre}</option>`;
                });
            });
        });
}