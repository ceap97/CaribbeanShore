﻿document.addEventListener('DOMContentLoaded', function () {
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
                    <td>${pago.estadoPago}</td>
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
    $('#createModal').modal('show');
}

function openEditModal(idPago) {
    fetch(`/Pagos/Obtener/${idPago}`)
        .then(response => response.json())
        .then(data => {
            $('#editIdPago').val(data.idPago);
            $('#editMonto').val(data.monto);
            $('#editMetodoPago').val(data.metodoPago);
            $('#editComprobante').val(data.comprobante);
            loadReservas('#editReservaId', data.reservaId); // Cargar reservas y seleccionar la actual
            loadEstadosPago('#editEstadoPagoId', data.estadoPagoId); // Cargar estados de pago y seleccionar el actual
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
                `,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Pagos/Crear', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

$('#editForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Pagos/Editar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
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
function loadEstadosPago() {
    fetch('/EstadoPago/Listar') // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            let estadosPagoSelects = document.querySelectorAll('#estadoPagoId, #editEstadoPagoId');
            estadosPagoSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione un Estado de Reserva</option>`;
                data.forEach(estadosPago => {
                    select.innerHTML += `<option value="${estadosPago.estadosPagoId}">${estadosPago.nombre}</option>`;
                });
            });
        });
}

