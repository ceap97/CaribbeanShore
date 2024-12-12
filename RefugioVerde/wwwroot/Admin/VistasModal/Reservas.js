document.addEventListener('DOMContentLoaded', function () {
    fetch('/Reservas/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbReservas tbody');
            data.forEach(reserva => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${reserva.reserva}</td>
                    <td>${new Date(reserva.fechaReserva).toLocaleDateString()}</td>
                    <td>${reserva.cliente}</td>
                    <td>${reserva.estadoReserva}</td>
                    <td>${new Date(reserva.fechaInicio).toLocaleDateString()}</td>
                    <td>${new Date(reserva.fechaFin).toLocaleDateString()}</td>
                    <td>${reserva.montoTotal.toFixed(2)}</td>
                    <td>${reserva.confirmacion}</td>
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
    loadComodidades('#comodidadId');
    loadServicios('#servicioId');
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
            $('#editComodidadId').val(data.comodidadId);
            $('#editServicioId').val(data.servicioId);
            $('#editEstadoReservaId').val(data.estadoReservaId);
            $('#editFechaInicio').val(data.fechaInicio.split('T')[0]);
            $('#editFechaFin').val(data.fechaFin.split('T')[0]);
            $('#editMontoTotal').val(data.montoTotal.toFixed(2));
            $('#editConfirmacion').val(data.confirmacion);
            $('#editModal').modal('show');
            loadClientes(); // Cargar clientes
            loadHabitaciones(); // Cargar habitaciones
            loadComodidades(); // Cargar comodidades
            loadServicios(); // Cargar servicios
            loadEstadosReserva(); // Cargar estados
        });
}

function openDeleteModal(reservaId) {
    Swal.fire({
        title: '¿Está seguro de que desea eliminar esta reserva?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cerrar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Reservas/Eliminar/${reservaId}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire('Eliminado!', 'La reserva ha sido eliminada.', 'success').then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire('Error', 'Hubo un problema al eliminar la reserva.', 'error');
                }
            }).catch(error => {
                Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
            });
        }
    });
}

function openDetailsModal(reservaId) {
    fetch(`/Reservas/Obtener/${reservaId}`)
        .then(response => response.json())
        .then(data => {
            Swal.fire({
                title: 'Detalles de la Reserva',
                html: `<p><strong>Reserva ID:</strong> ${data.reservaId}</p>
                       <p><strong>Fecha de Reserva:</strong> ${new Date(data.fechaReserva).toLocaleDateString()}</p>
                       <p><strong>Cliente:</strong> ${data.clienteId}</p>
                       <p><strong>Habitación:</strong> ${data.habitacionId}</p>
                       <p><strong>Comodidad:</strong> ${data.comodidadId}</p>
                       <p><strong>Servicio:</strong> ${data.servicioId}</p>
                       <p><strong>Estado:</strong> ${data.estadoReservaId}</p>
                       <p><strong>Fecha de Inicio:</strong> ${new Date(data.fechaInicio).toLocaleDateString()}</p>
                       <p><strong>Fecha de Fin:</strong> ${new Date(data.fechaFin).toLocaleDateString()}</p>
                       <p><strong>Monto Total:</strong> ${data.montoTotal.toFixed(2)}</p>
                       <p><strong>Confirmación:</strong> ${data.confirmacion}</p>`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
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
            Swal.fire(
                'Creado!',
                'La reserva ha sido creada correctamente.',
                'success'
            ).then(() => location.reload());
        } else {
            return response.json().then(data => {
                let errors = Object.values(data.errors).flat().join('<br>');
                Swal.fire(
                    'Error',
                    errors,
                    'error'
                );
            });
        }
    }).catch(error => {
        Swal.fire(
            'Error',
            'Hubo un problema con la solicitud.',
            'error'
        );
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
            Swal.fire(
                'Editado!',
                'La reserva ha sido editada correctamente.',
                'success'
            ).then(() => location.reload());
        } else {
            return response.json().then(data => {
                let errors = Object.values(data.errors).flat().join('<br>');
                Swal.fire(
                    'Error',
                    errors,
                    'error'
                );
            });
        }
    }).catch(error => {
        Swal.fire(
            'Error',
            'Hubo un problema con la solicitud.',
            'error'
        );
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

function loadComodidades() {
    fetch('/Comodidades/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let comodidadSelects = document.querySelectorAll('#comodidadId, #editComodidadId');
            comodidadSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione una Comodidad</option>`;
                data.forEach(comodidad => {
                    select.innerHTML += `<option value="${comodidad.comodidadId}">${comodidad.nombre}</option>`;
                });
            });
        });
}

function loadServicios() {
    fetch('/Servicios/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let servicioSelects = document.querySelectorAll('#servicioId, #editServicioId');
            servicioSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione un Servicio</option>`;
                data.forEach(servicio => {
                    select.innerHTML += `<option value="${servicio.servicioId}">${servicio.nombre}</option>`;
                });
            });
        });
}
