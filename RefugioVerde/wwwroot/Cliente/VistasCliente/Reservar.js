function openCreateModal() {
    const modalTemplate = `
        <div class="modal fade" id="createModal" tabindex="-1" aria-labelledby="createModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="createModalLabel">Agregar Reserva</h5>
                    </div>
                    <div class="modal-body">
                        <form id="createForm">
                            <input type="hidden" id="fechaReserva" name="fechaReserva" required>
                            <input type="hidden" id="clienteId" name="clienteId" required>
                            <input type="hidden" id="estadoReservaId" name="estadoReservaId" required>
                            <div class="mb-3">
                                <label for="habitacionId" class="form-label">Habitación</label>
                                <select class="form-select" id="habitacionId" name="habitacionId" required>
                                    <!-- Opciones de habitaciones se cargarán aquí -->
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="comodidadId" class="form-label">Comodidad</label>
                                <select class="form-select" id="comodidadId" name="comodidadId" required>
                                    <!-- Opciones de comodidades se cargarán aquí -->
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="servicioId" class="form-label">Servicio</label>
                                <select class="form-select" id="servicioId" name="servicioId" required>
                                    <!-- Opciones de servicios se cargarán aquí -->
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="fechaInicio" class="form-label">Fecha Inicio</label>
                                <input type="date" class="form-control" id="fechaInicio" name="fechaInicio" required>
                            </div>
                            <div class="mb-3">
                                <label for="fechaFin" class="form-label">Fecha Fin</label>
                                <input type="date" class="form-control" id="fechaFin" name="fechaFin" required>
                            </div>
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            <button type="submit" class="btn btn-primary">Guardar</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalTemplate);

    // Establecer la fecha de la reserva por defecto como la fecha actual
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('fechaReserva').value = today;

    // Cargar datos y establecer valores por defecto
    loadClientes().then(() => {
        // Establecer el cliente por defecto como el cliente asociado al usuario que inició sesión
        fetch('/Clientes/ObtenerClienteActual')
            .then(response => response.json())
            .then(data => {
                document.getElementById('clienteId').value = data.clienteId;
            });
    });

    loadHabitaciones();
    loadComodidades();
    loadServicios();
    loadEstadosReserva().then(() => {
        document.getElementById('estadoReservaId').value = 2; 
    });

    $('#createModal').modal('show');

    document.getElementById('createForm').addEventListener('submit', function (e) {
        e.preventDefault();
        let formData = new FormData(this);
        fetch('/Reservas/Crear', {
            method: 'POST',
            body: formData
        }).then(response => {
            if (response.ok) {
                $('#createModal').modal('hide');
                location.reload();
            } else {
                Swal.fire('Error', 'Hubo un problema al crear la reserva.', 'error');
            }
        }).catch(error => {
            Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
        });
    });
}

function loadEstadosReserva() {
    return fetch('/EstadoReservas/Listar') // Asegúrate de que la ruta sea correcta
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

function loadClientes() {
    return fetch('/Clientes/Listar') // Asegúrate de que la ruta sea correcta
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
    return fetch('/Habitaciones/Listar') // Asegúrate de que la ruta sea correcta
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

function loadComodidades() {
    return fetch('/Comodidades/Listar') // Asegúrate de que la ruta sea correcta
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
    return fetch('/Servicios/Listar') // Asegúrate de que la ruta sea correcta
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
