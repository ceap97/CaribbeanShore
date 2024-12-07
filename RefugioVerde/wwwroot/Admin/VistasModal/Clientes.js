document.addEventListener('DOMContentLoaded', function () {
    // Cargar los datos de clientes
    fetch('/Clientes/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbClientes tbody');
            data.forEach(cliente => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${cliente.clienteId}</td>
                    <td>${cliente.nombre}</td>
                    <td>${cliente.apellido}</td>
                    <td>${cliente.correo}</td>
                    <td>${cliente.genero}</td>
                    <td>
                        <button class="btn btn-warning btn-sm" onclick="openEditModal(${cliente.clienteId})">
                            <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${cliente.clienteId})">
                            <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-info btn-sm" onclick="openDetailsModal(${cliente.clienteId})">
                            <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px;" />
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            // Inicializar DataTable para la tabla
            let table = new DataTable('#tbClientes', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

// Abre el modal de creación
function openCreateModal() {
    // Cargar usuarios en el select
    loadUsuarios('#usuarioId');
    $('#createModal').modal('show');
}

// Abre el modal de edición
function openEditModal(clienteId) {
    // Cargar los datos del cliente
    fetch(`/Clientes/Obtener/${clienteId}`)
        .then(response => response.json())
        .then(cliente => {
            $('#editClienteId').val(cliente.clienteId);
            $('#editNombre').val(cliente.nombre);
            $('#editApellido').val(cliente.apellido);
            $('#editDocumentoIdentidad').val(cliente.documentoIdentidad);
            $('#editTelefono').val(cliente.telefono);
            $('#editCorreo').val(cliente.correo);
            $('#editDireccion').val(cliente.direccion);
            $('#editGenero').val(cliente.genero);
            loadUsuarios('#editUsuarioId');
            $('#editModal').modal('show');
        });
}

// Abre el modal de detalles
function openDetailsModal(clienteId) {
    // Obtener los detalles del cliente
    fetch(`/Clientes/Obtener/${clienteId}`)
        .then(response => response.json())
        .then(cliente => {
            // Obtener el nombre del usuario
            fetch(`/Usuarios/Obtener/${cliente.usuarioId}`)
                .then(res => res.json())
                .then(usuario => {
                    // Mostrar los detalles en un SweetAlert2 modal
                    Swal.fire({
                        title: 'Detalles del Cliente',
                        html: `
                            <div style="text-align: left;">
                                <p><strong>ID:</strong> ${cliente.clienteId}</p>
                                <p><strong>Nombre:</strong> ${cliente.nombre}</p>
                                <p><strong>Apellido:</strong> ${cliente.apellido}</p>
                                <p><strong>Documento:</strong> ${cliente.documentoIdentidad}</p>
                                <p><strong>Teléfono:</strong> ${cliente.telefono}</p>
                                <p><strong>Correo:</strong> ${cliente.correo}</p>
                                <p><strong>Dirección:</strong> ${cliente.direccion}</p>
                                <p><strong>Género:</strong> ${cliente.genero}</p>
                                <p><strong>Usuario:</strong> ${usuario.nombreUsuario}</p>
                            </div>
                        `,
                        icon: 'info'
                    });
                }).catch(error => {
                    Swal.fire(
                        'Error',
                        'Hubo un problema al cargar el usuario.',
                        'error'
                    );
                });
        }).catch(error => {
            Swal.fire(
                'Error',
                'Hubo un problema al cargar los detalles del cliente.',
                'error'
            );
        });
}

// Abre el modal de eliminación con SweetAlert2
function openDeleteModal(clienteId) {
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
            fetch(`/Clientes/Eliminar/${clienteId}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Eliminado!',
                            'El cliente ha sido eliminado correctamente.',
                            'success'
                        ).then(() => location.reload());
                    } else {
                        Swal.fire(
                            'Error',
                            'Hubo un problema al intentar eliminar el cliente.',
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

// Función para cargar los usuarios en el select
function loadUsuarios(usuarioSelectId) {
    fetch('/Usuarios/Listar')
        .then(response => response.json())
        .then(data => {
            let usuarioSelect = document.querySelector(usuarioSelectId);
            usuarioSelect.innerHTML = '<option value="">Seleccione un usuario</option>';
            data.forEach(usuario => {
                let option = document.createElement('option');
                option.value = usuario.usuarioId;
                option.textContent = usuario.nombreUsuario;
                usuarioSelect.appendChild(option);
            });
        });
}

// Formulario de creación
$('#createForm').submit(function (e) {
    e.preventDefault();
    if (!validateForm(this)) return;

    Swal.fire({
        title: '¿Está seguro?',
        text: "Se guardarán los datos del cliente.",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, guardar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            let formData = $(this).serialize();
            fetch('/Clientes/Crear', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: formData
            }).then(response => {
                if (response.ok) {
                    Swal.fire(
                        'Creado!',
                        'El cliente ha sido creado correctamente.',
                        'success'
                    ).then(() => location.reload());
                } else {
                    Swal.fire(
                        'Error',
                        'Hubo un problema al intentar crear el cliente.',
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
});

// Formulario de edición
$('#editForm').submit(function (e) {
    e.preventDefault();
    if (!validateForm(this)) return;

    Swal.fire({
        title: '¿Está seguro?',
        text: "Se actualizarán los datos del cliente.",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, actualizar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            let formData = $(this).serialize();
            fetch('/Clientes/Editar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: formData
            }).then(response => {
                if (response.ok) {
                    Swal.fire(
                        'Actualizado!',
                        'El cliente ha sido actualizado correctamente.',
                        'success'
                    ).then(() => location.reload());
                } else {
                    Swal.fire(
                        'Error',
                        'Hubo un problema al intentar actualizar el cliente.',
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
});

// Función de validación
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

