document.addEventListener('DOMContentLoaded', function () {
    // Cargar los datos de empleados
    fetch('/Empleados/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbEmpleados tbody');
            data.forEach(empleado => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${empleado.empleadoId}</td>
                    <td>${empleado.nombre}</td>
                    <td>${empleado.apellido}</td>
                    <td>${empleado.documentoIdentidad}</td>
                    <td>${empleado.telefono}</td>
                    <td>${empleado.correo}</td>
                    <td>${empleado.rolId}</td>
                    <td>
                        <button class="btn btn-warning btn-sm" onclick="openEditModal(${empleado.empleadoId})">
                            <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${empleado.empleadoId})">
                            <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-info btn-sm" onclick="openDetailsModal(${empleado.empleadoId})">
                            <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px;" />
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            // Inicializar DataTable para la tabla
            let table = new DataTable('#tbEmpleados', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

// Abre el modal de creación
function openCreateModal() {
    loadMunicipiosAndRoles('#municipioId', '#rolId'); // Cargar municipios y roles
    $('#createModal').modal('show');
}

// Abre el modal de edición
function openEditModal(empleadoId) {
    // Cargar los datos del empleado
    fetch(`/Empleados/Obtener/${empleadoId}`)
        .then(response => response.json())
        .then(empleado => {
            $('#editEmpleadoId').val(empleado.empleadoId);
            $('#editNombre').val(empleado.nombre);
            $('#editApellido').val(empleado.apellido);
            $('#editDocumentoIdentidad').val(empleado.documentoIdentidad);
            $('#editTelefono').val(empleado.telefono);
            $('#editCorreo').val(empleado.correo);
            loadMunicipiosAndRoles('#editMunicipioId', '#editRolId'); // Cargar municipios y roles
            $('#editModal').modal('show');
        });
}

// Abre el modal de detalles con SweetAlert2
function openDetailsModal(empleadoId) {
    fetch(`/Empleados/Obtener/${empleadoId}`)
        .then(response => response.json())
        .then(empleado => {
            Swal.fire({
                title: `Detalles del Empleado ${empleado.nombre} ${empleado.apellido}`,
                html: `
                    <p><strong>EmpleadoId:</strong> ${empleado.empleadoId}</p>
                    <p><strong>Nombre:</strong> ${empleado.nombre}</p>
                    <p><strong>Apellido:</strong> ${empleado.apellido}</p>
                    <p><strong>Documento Identidad:</strong> ${empleado.documentoIdentidad}</p>
                    <p><strong>Teléfono:</strong> ${empleado.telefono}</p>
                    <p><strong>Correo:</strong> ${empleado.correo}</p>
                    <p><strong>Municipio:</strong> ${empleado.municipioId}</p>
                    <p><strong>Rol:</strong> ${empleado.rolId}</p>
                `,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

// Abre el modal de eliminación con SweetAlert2
function openDeleteModal(empleadoId) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¡Esta acción no se puede deshacer!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Empleados/Eliminar/${empleadoId}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Eliminado!',
                            'El empleado ha sido eliminado correctamente.',
                            'success'
                        ).then(() => location.reload());
                    } else {
                        Swal.fire(
                            'Error',
                            'Hubo un problema al intentar eliminar el empleado.',
                            'error'
                        );
                    }
                }).catch(error => {
                    Swal.fire(
                        'Error',
                        'Hubo un problema con la solicitud.',
                        'error'
                    );
                });
        }
    });
}

// Función para cargar los municipios y roles en los selects
function loadMunicipiosAndRoles(municipioSelectId, rolSelectId) {
    fetch('/Municipios/Listar')
        .then(response => response.json())
        .then(data => {
            let municipioSelect = document.querySelector(municipioSelectId);
            municipioSelect.innerHTML = '<option value="">Seleccione un municipio</option>';
            data.forEach(municipio => {
                let option = document.createElement('option');
                option.value = municipio.municipioId;
                option.textContent = municipio.nombre;
                municipioSelect.appendChild(option);
            });
        });

    fetch('/Roles/Listar')
        .then(response => response.json())
        .then(data => {
            let rolSelect = document.querySelector(rolSelectId);
            rolSelect.innerHTML = '<option value="">Seleccione un rol</option>';
            data.forEach(rol => {
                let option = document.createElement('option');
                option.value = rol.rolId;
                option.textContent = rol.nombre;
                rolSelect.appendChild(option);
            });
        });
}

// Formulario de creación
$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Empleados/Crear', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire({
                title: 'Empleado creado',
                text: 'El empleado ha sido creado correctamente.',
                icon: 'success',
                confirmButtonText: 'Cerrar'
            }).then(() => location.reload());
        } else {
            return response.json().then(data => {
                Swal.fire({
                    title: 'Error',
                    text: data.message || 'Hubo un problema al crear el empleado.',
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

// Formulario de edición
$('#editForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Empleados/Editar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire({
                title: 'Empleado editado',
                text: 'El empleado ha sido editado correctamente.',
                icon: 'success',
                confirmButtonText: 'Cerrar'
            }).then(() => location.reload());
        } else {
            return response.json().then(data => {
                Swal.fire({
                    title: 'Error',
                    text: data.message || 'Hubo un problema al editar el empleado.',
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
