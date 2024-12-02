document.addEventListener('DOMContentLoaded', function () {
    fetch('/Permisos/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbPermisos tbody');
            data.forEach(permiso => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                                    <td>${permiso.permisoId}</td>
                                    <td>${permiso.nombre}</td>
                                    <td>${permiso.descripcion}</td>
                                    <td>
                                        <div class="action-buttons">
        <button class="btn btn-warning btn-sm" onclick="openEditModal(${permiso.permisoId})"><img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
        <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${permiso.permisoId})"><img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
        <button class="btn btn-info btn-sm" onclick="openDetailsModal(${permiso.permisoId})"><img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" /></button>
                                        </div>
                                    </td>
                                `;
                tbody.appendChild(tr);
            });
            let table = new DataTable('#tbPermisos', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

function openCreateModal() {
    $('#createModal').modal('show');
}

function openEditModal(permisoId) {
    // Lógica para obtener los datos del permiso y llenar el formulario de edición
    fetch(`/Permisos/Obtener/${permisoId}`)
        .then(response => response.json())
        .then(data => {
            $('#editPermisoId').val(data.permisoId);
            $('#editNombre').val(data.nombre);
            $('#editDescripcion').val(data.descripcion);
            $('#editModal').modal('show');
        });
}

function openDeleteModal(permisoId) {
    Swal.fire({
        title: '¿Está seguro de que desea eliminar este permiso?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cerrar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Permisos/Eliminar/${permisoId}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire('Eliminado!', 'El permiso ha sido eliminado.', 'success').then(() => {
                        location.reload();
                    });
                } else {
                    response.json().then(data => {
                        Swal.fire('Error', data.message || 'No se pudo eliminar el permiso.', 'error');
                    });
                }
            }).catch(error => {
                Swal.fire('Error', 'Ocurrió un error al eliminar el permiso.', 'error');
            });
        }
    });
}

function openDetailsModal(permisoId) {
    fetch(`/Permisos/Obtener/${permisoId}`)
        .then(response => response.json())
        .then(data => {
            Swal.fire({
                title: 'Detalles del Permiso',
                html: `<p><strong>PermisoId:</strong> ${data.permisoId}</p>
                       <p><strong>Nombre:</strong> ${data.nombre}</p>
                       <p><strong>Descripción:</strong> ${data.descripcion}</p>`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    // Lógica para enviar los datos del formulario de creación
    let formData = $(this).serialize();
    fetch('/Permisos/Crear', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            $('#createModal').modal('hide');
            Swal.fire('Éxito', 'El permiso ha sido creado exitosamente.', 'success').then(() => {
                location.reload();
            });
        } else {
            Swal.fire('Error', 'No se pudo crear el permiso.', 'error');
        }
    }).catch(error => {
        Swal.fire('Error', 'Ocurrió un error al crear el permiso.', 'error');
    });
});

$('#editForm').submit(function (e) {
    e.preventDefault();
    // Lógica para enviar los datos del formulario de edición
    let formData = $(this).serialize();
    fetch('/Permisos/Editar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            $('#editModal').modal('hide');
            Swal.fire('Éxito', 'El permiso ha sido editado exitosamente.', 'success').then(() => {
                location.reload();
            });
        } else {
            Swal.fire('Error', 'No se pudo editar el permiso.', 'error');
        }
    }).catch(error => {
        Swal.fire('Error', 'Ocurrió un error al editar el permiso.', 'error');
    });
});
