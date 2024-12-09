document.addEventListener('DOMContentLoaded', function () {
    fetch('/Pagos/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbPagos tbody');
            data.forEach(pago => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${pago.idPago}</td>
                    <td>${pago.monto}</td>
                    <td>${pago.metodoPago}</td>
                    <td>${pago.comprobante}</td>
                    <td>${pago.reservaId}</td>
                    <td>
                        <select class="form-select" onchange="changeEstadoPago(${pago.idPago}, this.value)">
                            <option value="10" ${pago.estadoPagoId == 10 ? 'selected' : ''}>En revisión</option>
                            <option value="11" ${pago.estadoPagoId == 11 ? 'selected' : ''}>Aprobado</option>
                            <option value=12" ${pago.estadoPagoId == 12 ? 'selected' : ''}>Rechazado</option>
                        </select>
                    </td>
                    <td>${pago.tipo}</td>
                    <td>${new Date(pago.fechaPago).toLocaleDateString()}</td>
                    <td>
                        <div class="action-buttons">
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${pago.idPago})"><img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${pago.idPago})"><img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${pago.idPago})"><img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
                        </div>
                    </td>
                `;
                tbody.appendChild(tr);
            });
            let table = new DataTable('#tbPagos', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

function openCreateModal() {
    loadReservas('#createReservaId');
    loadEstadosPago('#createEstadoPagoId');
    loadMetodosDePago('#createMetodoDePagoId');
    $('#createModal').modal('show');
}

function openEditModal(idPago) {
    fetch(`/Pagos/Obtener/${idPago}`)
        .then(response => response.json())
        .then(data => {
            $('#editIdPago').val(data.idPago);
            $('#editMonto').val(data.monto);
            $('#editMetodoPago').val(data.metodoPagoId); // Asegúrate de que el valor del método de pago se establezca correctamente
            // No establecer el valor del campo de archivo
            // $('#editComprobante').val(data.comprobante);
            $('#editTipo').val(data.tipo);
            $('#editFechaPago').val(new Date(data.fechaPago).toISOString().split('T')[0]);
            loadReservas('#editReservaId', data.reservaId); // Cargar reservas y seleccionar la actual
            loadEstadosPago('#editEstadoPagoId', data.estadoPagoId); // Cargar estados de pago y seleccionar el actual
            loadMetodosDePago('#editMetodoDePagoId', data.metodoDePagoId); // Cargar métodos de pago y seleccionar el actual
            $('#editModal').modal('show');
        });
}

function openDeleteModal(idPago) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "No podrás revertir esto",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminarlo',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Pagos/Eliminar/${idPago}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire(
                        'Eliminado',
                        'El pago ha sido eliminado.',
                        'success'
                    ).then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire(
                        'Error',
                        'Hubo un problema al eliminar el pago.',
                        'error'
                    );
                }
            });
        }
    });
}

function openDetailsModal(idPago) {
    fetch(`/Pagos/Obtener/${idPago}`)
        .then(response => response.json())
        .then(data => {
            Swal.fire({
                title: 'Detalles del Pago',
                html: `
                    <p><strong>ID Pago:</strong> ${data.idPago}</p>
                    <p><strong>Monto:</strong> ${data.monto}</p>
                    <p><strong>Método de Pago:</strong> ${data.metodoPago}</p>
                    <p><strong>Comprobante:</strong> ${data.comprobante}</p>
                    <p><strong>Reserva ID:</strong> ${data.reservaId}</p>
                    <p><strong>Estado de Pago:</strong> ${data.estadoPago}</p>
                    <p><strong>Tipo:</strong> ${data.tipo}</p>
                    <p><strong>Fecha de Pago:</strong> ${new Date(data.fechaPago).toLocaleDateString()}</p>
                `,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    fetch('/Pagos/Crear', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire(
                'Creado!',
                'El pago ha sido creado correctamente.',
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
    fetch('/Pagos/Editar', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire(
                'Editado!',
                'El pago ha sido editado correctamente.',
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
    let idPago = $(this).data('idPago');
    fetch(`/Pagos/Eliminar/${idPago}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

function loadReservas(selector, selectedReservaId = null) {
    fetch('/Reservas/Listar')  // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let select = document.querySelector(selector);
            select.innerHTML = `<option value="">Seleccione una reserva</option>`;
            data.forEach(reserva => {
                let option = document.createElement('option');
                option.value = reserva.reservaId;
                option.text = reserva.reservaId;
                if (reserva.reservaId === selectedReservaId) {
                    option.selected = true; // Seleccionar la reserva actual
                }
                select.appendChild(option);
            });
        });
}

function loadEstadosPago(selector, selectedEstadoPagoId = null) {
    fetch('/EstadoPagos/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let select = document.querySelector(selector);
            select.innerHTML = `<option value="">Seleccione un Estado de Pago</option>`;
            data.forEach(estadoPago => {
                let option = document.createElement('option');
                option.value = estadoPago.estadoPagoId;
                option.text = estadoPago.nombre;
                if (estadoPago.estadoPagoId === selectedEstadoPagoId) {
                    option.selected = true; // Seleccionar el estado de pago actual
                }
                select.appendChild(option);
            });
        });
}

function loadMetodosDePago(selector, selectedMetodoDePagoId = null) {
    fetch('/MetodoDePagos/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let select = document.querySelector(selector);
            select.innerHTML = `<option value="">Seleccione un Método de Pago</option>`;
            data.forEach(metodoDePago => {
                let option = document.createElement('option');
                option.value = metodoDePago.metodoDePagoId;
                option.text = metodoDePago.nombre;
                if (metodoDePago.metodoDePagoId === selectedMetodoDePagoId) {
                    option.selected = true; // Seleccionar el método de pago actual
                }
                select.appendChild(option);
            });
        });
}

function changeEstadoPago(idPago, estadoPagoId) {
    console.log(`Cambiando estado del pago ${idPago} a ${estadoPagoId}`);
    fetch(`/Pagos/CambiarEstado`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ idPago, estadoPagoId })
    }).then(response => {
        if (response.ok) {
            Swal.fire(
                'Actualizado!',
                'El estado del pago ha sido actualizado correctamente.',
                'success'
            );
        } else {
            response.json().then(data => {
                Swal.fire(
                    'Error',
                    data.message || 'Hubo un problema al actualizar el estado del pago.',
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
}


