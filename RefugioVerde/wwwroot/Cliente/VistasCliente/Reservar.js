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
                                <input type="hidden" id="precioHabitacion" name="precioHabitacion">
                            </div>
                            <div class="mb-3">
                                <label for="comodidades" class="form-label">Comodidades</label>
                                <div id="comodidades">
                                    <!-- Checkboxes de comodidades se cargarán aquí -->
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="servicios" class="form-label">Servicios</label>
                                <div id="servicios">
                                    <!-- Checkboxes de servicios se cargarán aquí -->
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="fechaInicio" class="form-label">Fecha Inicio</label>
                                <input type="date" class="form-control" id="fechaInicio" name="fechaInicio" required>
                            </div>
                            <div class="mb-3">
                                <label for="fechaFin" class="form-label">Fecha Fin</label>
                                <input type="date" class="form-control" id="fechaFin" name="fechaFin" required>
                            </div>
                            <div class="mb-3">
                                <label for="total" class="form-label">Total</label>
                                <input type="text" class="form-control" id="total" name="total" readonly>
                            </div>
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            <button type="submit" class="btn btn-primary" id="guardarBtn">Guardar</button>
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

    // Establecer el valor mínimo para las fechas de inicio y fin
    document.getElementById('fechaInicio').setAttribute('min', today);
    document.getElementById('fechaFin').setAttribute('min', today);

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

        // Agregar comodidades seleccionadas al formData
        document.querySelectorAll('#comodidades input[type="checkbox"]:checked').forEach(checkbox => {
            formData.append('comodidades', checkbox.value);
        });

        // Agregar servicios seleccionados al formData
        document.querySelectorAll('#servicios input[type="checkbox"]:checked').forEach(checkbox => {
            formData.append('servicios', checkbox.value);
        });

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

    // Agregar eventos para calcular el total y validar fechas
    document.getElementById('habitacionId').addEventListener('change', calculateTotal);
    document.getElementById('comodidades').addEventListener('change', calculateTotal);
    document.getElementById('servicios').addEventListener('change', calculateTotal);
    document.getElementById('fechaInicio').addEventListener('change', validateDates);
    document.getElementById('fechaFin').addEventListener('change', validateDates);
}

function calculateTotal() {
    const precioHabitacion = parseFloat(document.getElementById('precioHabitacion').value) || 0;

    let precioComodidades = 0;
    document.querySelectorAll('#comodidades input[type="checkbox"]:checked').forEach(checkbox => {
        precioComodidades += parseFloat(checkbox.getAttribute('data-precio')) || 0;
    });

    let precioServicios = 0;
    document.querySelectorAll('#servicios input[type="checkbox"]:checked').forEach(checkbox => {
        precioServicios += parseFloat(checkbox.getAttribute('data-precio')) || 0;
    });

    const fechaInicio = new Date(document.getElementById('fechaInicio').value);
    const fechaFin = new Date(document.getElementById('fechaFin').value);

    const diffTime = Math.abs(fechaFin - fechaInicio);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 0; // +1 para incluir el día de inicio

    const total = (precioHabitacion + precioComodidades + precioServicios) * diffDays;
    document.getElementById('total').value = total.toFixed(2);
}

function validateDates() {
    const fechaInicio = new Date(document.getElementById('fechaInicio').value);
    const fechaFin = new Date(document.getElementById('fechaFin').value);
    const guardarBtn = document.getElementById('guardarBtn');

    if (fechaFin <= fechaInicio) {
        Swal.fire('Error', 'La fecha de fin debe ser mayor que la fecha de inicio.', 'error');
        guardarBtn.disabled = true;
    } else {
        guardarBtn.disabled = false;
        calculateTotal();
    }
}

function loadHabitaciones() {
    return fetch('/Habitaciones/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let habitacionSelects = document.querySelectorAll('#habitacionId, #editHabitacionId');
            habitacionSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione una Habitación</option>`;
                data.forEach(habitacion => {
                    select.innerHTML += `<option value="${habitacion.habitacionId}" data-precio="${habitacion.precio}">${habitacion.nombreHabitacion}</option>`;
                });
            });

            document.getElementById('habitacionId').addEventListener('change', function () {
                const selectedOption = this.options[this.selectedIndex];
                document.getElementById('precioHabitacion').value = selectedOption.getAttribute('data-precio');
                calculateTotal();
            });
        });
}

function loadComodidades() {
    return fetch('/Comodidades/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let comodidadesDiv = document.getElementById('comodidades');
            comodidadesDiv.innerHTML = '';
            data.forEach(comodidad => {
                comodidadesDiv.innerHTML += `
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" value="${comodidad.comodidadId}" data-precio="${comodidad.precio}" id="comodidad-${comodidad.comodidadId}">
                        <label class="form-check-label" for="comodidad-${comodidad.comodidadId}">
                            ${comodidad.nombre} - ${comodidad.precio.toFixed(2)} €
                        </label>
                    </div>
                `;
            });
        });
}

function loadServicios() {
    return fetch('/Servicios/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let serviciosDiv = document.getElementById('servicios');
            serviciosDiv.innerHTML = '';
            data.forEach(servicio => {
                serviciosDiv.innerHTML += `
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" value="${servicio.servicioId}" data-precio="${servicio.precio}" id="servicio-${servicio.servicioId}">
                        <label class="form-check-label" for="servicio-${servicio.servicioId}">
                            ${servicio.nombre} - ${servicio.precio.toFixed(2)} €
                        </label>
                    </div>
                `;
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
