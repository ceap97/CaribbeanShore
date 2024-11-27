function openEditReservaModal(reservaId) {
    fetch(`/Reservas/Obtener/${reservaId}`)
        .then(response => response.json())
        .then(data => {
            const modalTemplate = `
                <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="editModalLabel">Editar Reserva</h5>
                            </div>
                            <div class="modal-body">
                                <form id="editForm">
                                    <input type="hidden" id="editReservaId" name="reservaId" value="${data.reservaId}" required>
                                    <input type="hidden" id="editFechaReserva" name="fechaReserva" value="${data.fechaReserva.split('T')[0]}" required>
                                    <input type="hidden" id="editClienteId" name="clienteId" value="${data.clienteId}" required>
                                    <input type="hidden" id="editEstadoReservaId" name="estadoReservaId" value="${data.estadoReservaId}" required>
                                    <div class="mb-3">
                                        <label for="editHabitacionId" class="form-label">Habitación</label>
                                        <select class="form-select" id="editHabitacionId" name="habitacionId" required>
                                            <!-- Opciones de habitaciones se cargarán aquí -->
                                        </select>
                                    </div>
                                    <div class="mb-3">
                                        <label for="editComodidadId" class="form-label">Comodidad</label>
                                        <select class="form-select" id="editComodidadId" name="comodidadId" required>
                                            <!-- Opciones de comodidades se cargarán aquí -->
                                        </select>
                                    </div>
                                    <div class="mb-3">
                                        <label for="editServicioId" class="form-label">Servicio</label>
                                        <select class="form-select" id="editServicioId" name="servicioId" required>
                                            <!-- Opciones de servicios se cargarán aquí -->
                                        </select>
                                    </div>
                                    <div class="mb-3">
                                        <label for="editFechaInicio" class="form-label">Fecha Inicio</label>
                                        <input type="date" class="form-control" id="editFechaInicio" name="fechaInicio" value="${data.fechaInicio.split('T')[0]}" required>
                                    </div>
                                    <div class="mb-3">
                                        <label for="editFechaFin" class="form-label">Fecha Fin</label>
                                        <input type="date" class="form-control" id="editFechaFin" name="fechaFin" value="${data.fechaFin.split('T')[0]}" required>
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

            loadHabitaciones(data.habitacionId);
            loadComodidades(data.comodidadId);
            loadServicios(data.servicioId);
            loadEstadosReserva(data.estadoReservaId);

            $('#editModal').modal('show');

            document.getElementById('editForm').addEventListener('submit', function (e) {
                e.preventDefault();
                let formData = new FormData(this);
                fetch('/Reservas/Editar', {
                    method: 'POST',
                    body: formData
                }).then(response => {
                    if (response.ok) {
                        $('#editModal').modal('hide');
                        location.reload();
                    } else {
                        Swal.fire('Error', 'Hubo un problema al editar la reserva.', 'error');
                    }
                }).catch(error => {
                    Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
                });
            });
        });
}

function loadEstadosReserva(selectedId) {
    return fetch('/EstadoReservas/Listar')
        .then(response => response.json())
        .then(data => {
            let estadoReservaSelect = document.getElementById('editEstadoReservaId');
            estadoReservaSelect.innerHTML = `<option value="">Seleccione un Estado de Reserva</option>`;
            data.forEach(estadoReserva => {
                estadoReservaSelect.innerHTML += `<option value="${estadoReserva.estadoReservaId}" ${estadoReserva.estadoReservaId === selectedId ? 'selected' : ''}>${estadoReserva.nombre}</option>`;
            });
        });
}

function loadHabitaciones(selectedId) {
    return fetch('/Habitaciones/Listar')
        .then(response => response.json())
        .then(data => {
            let habitacionSelect = document.getElementById('editHabitacionId');
            habitacionSelect.innerHTML = `<option value="">Seleccione una Habitación</option>`;
            data.forEach(habitacion => {
                habitacionSelect.innerHTML += `<option value="${habitacion.habitacionId}" ${habitacion.habitacionId === selectedId ? 'selected' : ''}>${habitacion.nombreHabitacion}</option>`;
            });
        });
}

function loadComodidades(selectedId) {
    return fetch('/Comodidades/Listar')
        .then(response => response.json())
        .then(data => {
            let comodidadSelect = document.getElementById('editComodidadId');
            comodidadSelect.innerHTML = `<option value="">Seleccione una Comodidad</option>`;
            data.forEach(comodidad => {
                comodidadSelect.innerHTML += `<option value="${comodidad.comodidadId}" ${comodidad.comodidadId === selectedId ? 'selected' : ''}>${comodidad.nombre}</option>`;
            });
        });
}

function loadServicios(selectedId) {
    return fetch('/Servicios/Listar')
        .then(response => response.json())
        .then(data => {
            let servicioSelect = document.getElementById('editServicioId');
            servicioSelect.innerHTML = `<option value="">Seleccione un Servicio</option>`;
            data.forEach(servicio => {
                servicioSelect.innerHTML += `<option value="${servicio.servicioId}" ${servicio.servicioId === selectedId ? 'selected' : ''}>${servicio.nombre}</option>`;
            });
        });
}
