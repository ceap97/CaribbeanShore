
    


function openEditClientModal(clienteId) {
    const modalTemplate = `
        <div class="modal fade" id="clientModal" tabindex="-1" aria-labelledby="clientModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="clientModalLabel">Editar Cliente</h5>
                    </div>
                    <div class="modal-body">
                        <form id="clientForm">
                            <input type="hidden" id="usuarioId" name="usuarioId" required>
                            <input type="hidden" id="clienteId" name="clienteId">
                            <div class="mb-3">
                                <label for="nombre" class="form-label">Nombre</label>
                                <input type="text" class="form-control" id="nombre" name="nombre" required>
                            </div>
                            <div class="mb-3">
                                <label for="apellido" class="form-label">Apellido</label>
                                <input type="text" class="form-control" id="apellido" name="apellido" required>
                            </div>
                            <div class="mb-3">
                                <label for="documentoIdentidad" class="form-label">Documento de Identidad</label>
                                <input type="text" class="form-control" id="documentoIdentidad" name="documentoIdentidad" required>
                            </div>
                            <div class="mb-3">
                                <label for="municipioId" class="form-label">Municipio</label>
                                <select class="form-control" id="municipioId" name="municipioId" required></select>
                            </div>
                            <div class="mb-3">
                                <label for="telefono" class="form-label">Teléfono</label>
                                <input type="tel" class="form-control" id="telefono" name="telefono" required>
                            </div>
                            <div class="mb-3">
                                <label for="correo" class="form-label">Correo</label>
                                <input type="email" class="form-control" id="correo" name="correo" required>
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
    loadMunicipios().then(() => {
        fetch(`/Clientes/Obtener/${clienteId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('clienteId').value = data.clienteId;
                document.getElementById('nombre').value = data.nombre;
                document.getElementById('apellido').value = data.apellido;
                document.getElementById('documentoIdentidad').value = data.documentoIdentidad;
                document.getElementById('municipioId').value = data.municipioId;
                document.getElementById('telefono').value = data.telefono;
                document.getElementById('correo').value = data.correo;
                document.getElementById('usuarioId').value = data.usuarioId;
            });
    });
    $('#clientModal').modal('show');

    document.getElementById('clientForm').addEventListener('submit', function (e) {
        e.preventDefault();
        let formData = new FormData(this);
        console.log('Datos enviados:', Object.fromEntries(formData.entries())); // Verificar los datos enviados
        fetch('/Clientes/Editar', {
            method: 'POST',
            body: formData
        }).then(response => {
            if (response.ok) {
                $('#clientModal').modal('hide');
                location.reload();
            } else {
                Swal.fire('Error', 'Hubo un problema al editar el cliente.', 'error');
            }
        }).catch(error => {
            Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
        });
    });
}

function openEditReservaModal(reservaId) {
    // Remove any existing modals first
    const existingModal = document.getElementById('editModal');
    if (existingModal) {
        existingModal.remove();
    }

    if (!reservaId) {
        Swal.fire('Error', 'ID de reserva no válido', 'error');
        return;
    }

    fetch(`/Reservas/Obtener/${reservaId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Reserva no encontrada');
            }
            return response.json();
        })
        .then(data => {
            const modalTemplate = `
                <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="editModalLabel">Editar Reserva</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <form id="editForm">
                                    <input type="hidden" id="editReservaId" name="reservaId" value="${data.reservaId}">
                                    <input type="hidden" id="editFechaReserva" name="fechaReserva" value="${data.fechaReserva?.split('T')[0]}">
                                    <input type="hidden" id="editClienteId" name="clienteId" value="${data.clienteId}">
                                    <input type="hidden" id="editEstadoReservaId" name="estadoReservaId" value="${data.estadoReservaId}">
                                    
                                    <div class="mb-3">
                                        <label for="editHabitacionId" class="form-label">Habitación</label>
                                        <select class="form-select" id="editHabitacionId" name="habitacionId" required>
                                        </select>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label for="editComodidadId" class="form-label">Comodidad</label>
                                        <select class="form-select" id="editComodidadId" name="comodidadId" required>
                                        </select>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label for="editServicioId" class="form-label">Servicio</label>
                                        <select class="form-select" id="editServicioId" name="servicioId" required>
                                        </select>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label for="editFechaInicio" class="form-label">Fecha Inicio</label>
                                        <input type="date" class="form-control" id="editFechaInicio" name="fechaInicio" 
                                            value="${data.fechaInicio?.split('T')[0]}" required>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label for="editFechaFin" class="form-label">Fecha Fin</label>
                                        <input type="date" class="form-control" id="editFechaFin" name="fechaFin" 
                                            value="${data.fechaFin?.split('T')[0]}" required>
                                    </div>
                                    
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                        <button type="submit" class="btn btn-primary">Guardar Cambios</button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            `;

            document.body.insertAdjacentHTML('beforeend', modalTemplate);

            // Initialize dropdowns
            Promise.all([
                loadHabitaciones(data.habitacionId),
                loadComodidades(data.comodidadId),
                loadServicios(data.servicioId),
                loadEstadosReserva(data.estadoReservaId)
            ]).then(() => {
                const modal = new bootstrap.Modal(document.getElementById('editModal'));
                modal.show();
            });

            // Set up form submission
            document.getElementById('editForm').addEventListener('submit', function(e) {
                e.preventDefault();
                const formData = new FormData(this);

                fetch('/Reservas/Editar', {
                    method: 'POST',
                    body: formData
                })
                .then(response => {
                    if (response.ok) {
                        const modal = bootstrap.Modal.getInstance(document.getElementById('editModal'));
                        modal.hide();
                        location.reload();
                    } else {
                        throw new Error('Error al editar la reserva');
                    }
                })
                .catch(error => {
                    Swal.fire('Error', error.message, 'error');
                });
            });
        })
        .catch(error => {
            Swal.fire('Error', error.message, 'error');
        });
}

function loadMunicipios() {
    return fetch('/Municipios/Listar')
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al obtener la lista de municipios');
            }
            return response.json();
        })
        .then(data => {
            let municipioSelect = document.querySelector('#municipioId');
            municipioSelect.innerHTML = '<option value="">Seleccione un municipio</option>';
            data.forEach(municipio => {
                let option = document.createElement('option');
                option.value = municipio.municipioId;
                option.textContent = municipio.nombre;
                municipioSelect.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire('Error', 'Hubo un problema al cargar la lista de municipios.', 'error');
        });
}

