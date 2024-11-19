
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
            $('#editReservaId').val(data.reservaId);
            $('#editEstadoPagoId').val(data.estadoPagoId);
            $('#editModal').modal('show');
        });
}

function openDeleteModal(idPago) {
    $('#confirmDelete').data('idPago', idPago);
    $('#deleteModal').modal('show');
}

function openDetailsModal(idPago) {
    fetch(`/Pagos/Obtener/${idPago}`)
        .then(response => response.json())
        .then(data => {
            $('#detailsIdPago').text(data.idPago);
            $('#detailsMonto').text(data.monto);
            $('#detailsMetodoPago').text(data.metodoPago);
            $('#detailsComprobante').text(data.comprobante);
            $('#detailsReservaId').text(data.reservaId);
            $('#detailsEstadoPago').text(data.estadoPago);
            $('#detailsModal').modal('show');
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
