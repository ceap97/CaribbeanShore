document.addEventListener('DOMContentLoaded', function () {
    cargarRoles();
});

function cargarRoles() {
    fetch('/Roles/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbRoles tbody');
            tbody.innerHTML = ''; // Limpiar el contenido existente
            data.forEach(rol => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${rol.rolId}</td>
                    <td>${rol.nombre}</td>
                    <td>
                        <div class="action-buttons">
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${rol.rolId})">
                                <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${rol.rolId})">
                                <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${rol.rolId})">
                                <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                        </div>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            // Destruir la instancia existente de DataTable si existe
            if ($.fn.DataTable.isDataTable('#tbRoles')) {
                $('#tbRoles').DataTable().destroy();
            }

            // Inicializar DataTable
            $('#tbRoles').DataTable({
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
}

function openCreateModal() {
    $('#createForm')[0].reset(); // Limpiar el formulario
    $('#createModal').modal('show');
}

function openEditModal(rolId) {
    fetch(`/Roles/Obtener/${rolId}`)
        .then(response => response.json())
        .then(data => {
            $('#editRolId').val(data.rolId);
            $('#editNombre').val(data.nombre);
            // Marcar los checkboxes de permisos seleccionados
            data.permisosSeleccionados.forEach(permisoId => {
                $(`#editPermiso_${permisoId}`).prop('checked', true);
            });
            $('#editModal').modal('show');
        });
}

function openDetailsModal(rolId) {
    fetch(`/Roles/Obtener/${rolId}`)
        .then(response => response.json())
        .then(data => {
            let permisosHtml = data.permisosSeleccionados.map(permisoId => `<li>${permisoId}</li>`).join('');
            Swal.fire({
                title: 'Detalles del Rol',
                html: `<p><strong>RolId:</strong> ${data.rolId}</p>
                       <p><strong>Nombre:</strong> ${data.nombre}</p>
                       <p><strong>Permisos:</strong></p>
                       <ul>${permisosHtml}</ul>`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

function openDeleteModal(rolId) {
    Swal.fire({
        title: '¿Está seguro de que desea eliminar este rol?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cerrar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Roles/Eliminar/${rolId}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire('Eliminado!', 'El rol ha sido eliminado.', 'success').then(() => {
                        cargarRoles();
                    });
                } else {
                    response.json().then(data => {
                        if (data.message && data.message.includes('administrador')) {
                            Swal.fire('Error', 'El rol de administrador no se puede eliminar.', 'error');
                        } else {
                            Swal.fire('Error', data.message || 'No se pudo eliminar el rol.', 'error');
                        }
                    });
                }
            }).catch(error => {
                Swal.fire('Error', 'Ocurrió un error al eliminar el rol.', 'error');
            });
        }
    });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Roles/Crear', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            $('#createModal').modal('hide');
            Swal.fire('Éxito', 'El rol ha sido creado exitosamente.', 'success').then(() => {
                cargarRoles();
            });
        } else {
            Swal.fire('Error', 'No se pudo crear el rol.', 'error');
        }
    }).catch(error => {
        Swal.fire('Error', 'Ocurrió un error al crear el rol.', 'error');
    });
});

$('#editForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Roles/Editar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            $('#editModal').modal('hide');
            Swal.fire('Éxito', 'El rol ha sido editado exitosamente.', 'success').then(() => {
                cargarRoles();
            });
        } else {
            Swal.fire('Error', 'No se pudo editar el rol.', 'error');
        }
    }).catch(error => {
        Swal.fire('Error', 'Ocurrió un error al editar el rol.', 'error');
    });
});

function validateForm(form) {
    let isValid = true;
    $(form).find('input, select').each(function () {
        if (!this.checkValidity()) {
            isValid = false;
            $(this).addClass('is-invalid');
        } else {
            $(this).removeClass('is-invalid');
        }
    });
    return isValid;
}
