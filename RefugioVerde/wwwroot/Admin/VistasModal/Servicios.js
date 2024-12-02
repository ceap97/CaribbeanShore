document.addEventListener('DOMContentLoaded', function () {
    fetch('/Servicios/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbServicios tbody');
            data.forEach(servicio => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${servicio.servicioId}</td>
                    <td>${servicio.nombre}</td>
                    <td>${servicio.descripcion}</td>
                    <td>${servicio.precio}</td>
                    <td>
                        <div class="action-buttons">
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${servicio.servicioId})"><img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${servicio.servicioId})"><img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${servicio.servicioId})"><img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
                        </div>
                    </td>
                `;
                tbody.appendChild(tr);
            });
            let table = new DataTable('#tbServicios', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

function openCreateModal() {
    $('#createModal').modal('show');
}

function openEditModal(servicioId) {
    fetch(`/Servicios/Obtener/${servicioId}`)
        .then(response => response.json())
        .then(data => {
            $('#editServicioId').val(data.servicioId);
            $('#editNombre').val(data.nombre);
            $('#editDescripcion').val(data.descripcion);
            $('#editPrecio').val(data.precio);
            $('#editModal').modal('show');
        });
}

function openDeleteModal(servicioId) {
    Swal.fire({
        title: '¿Está seguro de que desea eliminar este servicio?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cerrar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Servicios/Eliminar/${servicioId}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire('Eliminado!', 'El servicio ha sido eliminado.', 'success').then(() => {
                        location.reload();
                    });
                }
            });
        }
    });
}

function openDetailsModal(servicioId) {
    fetch(`/Servicios/Obtener/${servicioId}`)
        .then(response => response.json())
        .then(data => {
            Swal.fire({
                title: 'Detalles del Servicio',
                html: `<p><strong>ServicioId:</strong> ${data.servicioId}</p>
                       <p><strong>Nombre:</strong> ${data.nombre}</p>
                       <p><strong>Descripción:</strong> ${data.descripcion}</p>
                       <p><strong>Precio:</strong> ${data.precio}</p>`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Servicios/Crear', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire({
                title: 'Servicio creado',
                text: 'El servicio ha sido creado correctamente.',
                icon: 'success',
                confirmButtonText: 'Cerrar'
            }).then(() => location.reload());
        } else {
            return response.json().then(data => {
                Swal.fire({
                    title: 'Error',
                    text: data.message || 'Hubo un problema al crear el servicio.',
                    icon: 'error',
                    confirmButtonText: 'Cerrar'
                });
            });
        }
    }).catch(error => {
        Swal.fire({
            title: 'Error',
            text: 'Hubo un problema con la solicitud.',
            icon: 'error',
            confirmButtonText: 'Cerrar'
        });
    });
});

$('#editForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Servicios/Editar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire({
                title: 'Servicio editado',
                text: 'El servicio ha sido editado correctamente.',
                icon: 'success',
                confirmButtonText: 'Cerrar'
            }).then(() => location.reload());
        } else {
            return response.json().then(data => {
                Swal.fire({
                    title: 'Error',
                    text: data.message || 'Hubo un problema al editar el servicio.',
                    icon: 'error',
                    confirmButtonText: 'Cerrar'
                });
            });
        }
    }).catch(error => {
        Swal.fire({
            title: 'Error',
            text: 'Hubo un problema con la solicitud.',
            icon: 'error',
            confirmButtonText: 'Cerrar'
        });
    });
});
