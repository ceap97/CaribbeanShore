document.addEventListener('DOMContentLoaded', function () {
    fetch('/MetodoDePagos/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbMetodoDePagos tbody');
            data.forEach(metodoDePago => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${metodoDePago.metodoDePagoId}</td>
                    <td>${metodoDePago.nombre}</td>
                    <td>
                        <div class="action-buttons">
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${metodoDePago.metodoDePagoId})">
                                <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${metodoDePago.metodoDePagoId})">
                                <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${metodoDePago.metodoDePagoId})">
                                <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                        </div>
                    </td>
                `;
                tbody.appendChild(tr);
            });
            let table = new DataTable('#tbMetodoDePagos', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

function openCreateModal() {
    $('#createModal').modal('show');
}

function openEditModal(metodoDePagoId) {
    // Lógica para obtener los datos del método de pago y llenar el formulario de edición
    fetch(`/MetodoDePagos/Obtener/${metodoDePagoId}`)
        .then(response => response.json())
        .then(data => {
            $('#editMetodoDePagoId').val(data.metodoDePagoId);
            $('#editNombre').val(data.nombre);
            $('#editModal').modal('show');
        });
}

function openDeleteModal(metodoDePagoId) {
    Swal.fire({
        title: '¿Está seguro de que desea eliminar este método de pago?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cerrar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/MetodoDePagos/EliminarModal/${metodoDePagoId}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire('Eliminado!', 'El método de pago ha sido eliminado.', 'success').then(() => {
                        location.reload();
                    });
                } else {
                    response.json().then(data => {
                        Swal.fire('Error', data.message || 'No se pudo eliminar el método de pago.', 'error');
                    });
                }
            }).catch(error => {
                Swal.fire('Error', 'Ocurrió un error al eliminar el método de pago.', 'error');
            });
        }
    });
}

function openDetailsModal(metodoDePagoId) {
    fetch(`/MetodoDePagos/Obtener/${metodoDePagoId}`)
        .then(response => response.json())
        .then(data => {
            Swal.fire({
                title: 'Detalles del Método de Pago',
                html: `<p><strong>MétodoDePagoId:</strong> ${data.metodoDePagoId}</p>
                       <p><strong>Nombre:</strong> ${data.nombre}</p>`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    // Lógica para enviar los datos del formulario de creación
    let formData = $(this).serialize();
    fetch('/MetodoDePagos/CrearModal', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            $('#createModal').modal('hide');
            Swal.fire('Éxito', 'El método de pago ha sido creado exitosamente.', 'success').then(() => {
                location.reload();
            });
        } else {
            Swal.fire('Error', 'No se pudo crear el método de pago.', 'error');
        }
    }).catch(error => {
        Swal.fire('Error', 'Ocurrió un error al crear el método de pago.', 'error');
    });
});

$('#editForm').submit(function (e) {
    e.preventDefault();
    // Lógica para enviar los datos del formulario de edición
    let formData = $(this).serialize();
    fetch('/MetodoDePagos/EditarModal', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            $('#editModal').modal('hide');
            Swal.fire('Éxito', 'El método de pago ha sido editado exitosamente.', 'success').then(() => {
                location.reload();
            });
        } else {
            Swal.fire('Error', 'No se pudo editar el método de pago.', 'error');
        }
    }).catch(error => {
        Swal.fire('Error', 'Ocurrió un error al editar el método de pago.', 'error');
    });
});
