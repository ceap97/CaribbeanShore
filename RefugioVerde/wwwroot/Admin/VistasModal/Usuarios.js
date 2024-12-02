document.addEventListener('DOMContentLoaded', function () {
    // Cargar los datos de usuarios
    fetch('/Usuarios/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbUsuarios tbody');
            data.forEach(usuario => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${usuario.usuarioId}</td>
                    <td>${usuario.nombreUsuario}</td>
                    <td>${usuario.correo}</td>
                    <td>${usuario.empleadoId}</td>
                    <td>
                        <button class="btn btn-warning btn-sm" onclick="openEditModal(${usuario.usuarioId})">
                            <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${usuario.usuarioId})">
                            <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-info btn-sm" onclick="openDetailsModal(${usuario.usuarioId})">
                            <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px;" />
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            // Inicializar DataTable para la tabla
            let table = new DataTable('#tbUsuarios', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

// Abre el modal de creación
function openCreateModal() {
    loadEmpleados('#empleadoId'); // Cargar empleados
    $('#createModal').modal('show');
}

// Abre el modal de edición
function openEditModal(usuarioId) {
    // Cargar los datos del usuario
    fetch(`/Usuarios/Obtener/${usuarioId}`)
        .then(response => response.json())
        .then(usuario => {
            $('#editUsuarioId').val(usuario.usuarioId);
            $('#editNombreUsuario').val(usuario.nombreUsuario);
            $('#editCorreo').val(usuario.correo);
            $('#editClave').val(''); // La clave puede ser opcional en edición
            loadEmpleados('#editEmpleadoId'); // Cargar empleados
            $('#editModal').modal('show');
        });
}

// Abre el modal de detalles con SweetAlert2
function openDetailsModal(usuarioId) {
    fetch(`/Usuarios/Obtener/${usuarioId}`)
        .then(response => response.json())
        .then(usuario => {
            Swal.fire({
                title: `Detalles de ${usuario.nombreUsuario}`,
                html: `
                    <p><strong>UsuarioId:</strong> ${usuario.usuarioId}</p>
                    <p><strong>Nombre de Usuario:</strong> ${usuario.nombreUsuario}</p>
                    <p><strong>Correo:</strong> ${usuario.correo}</p>
                    <p><strong>Empleado:</strong> ${usuario.empleadoId}</p>
                `,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

// Abre el modal de eliminación con SweetAlert2
function openDeleteModal(usuarioId) {
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
            fetch(`/Usuarios/Eliminar/${usuarioId}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Eliminado!',
                            'El usuario ha sido eliminado correctamente.',
                            'success'
                        ).then(() => location.reload());
                    } else {
                        Swal.fire(
                            'Error',
                            'Hubo un problema al intentar eliminar el usuario.',
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

// Función para cargar los empleados en los selects
function loadEmpleados(selectId) {
    fetch('/Empleados/Listar')
        .then(response => response.json())
        .then(data => {
            let empleadoSelect = document.querySelector(selectId);
            empleadoSelect.innerHTML = '<option value="">Seleccione un Empleado</option>';
            data.forEach(empleado => {
                let option = document.createElement('option');
                option.value = empleado.empleadoId;
                option.textContent = `${empleado.nombre} ${empleado.apellido}`;
                empleadoSelect.appendChild(option);
            });
        });
}
$('#createForm').submit(function (e) {
    e.preventDefault();

    // Obtener los datos del formulario
    let formData = new FormData(this);

    // Obtener la imagen como base64
    const imageInput = document.getElementById('imagen');
    if (imageInput.files.length > 0) {
        const file = imageInput.files[0];
        const reader = new FileReader();
        reader.onload = function (event) {
            formData.append('imagen', event.target.result); // Agregar imagen como base64 al formulario
            submitFormData(formData);
        };
        reader.readAsDataURL(file);
    } else {
        submitFormData(formData); // Enviar formulario sin imagen si no se ha seleccionado una
    }
});

function submitFormData(formData) {
    fetch('/Usuarios/Crear', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload(); // Recargar la página si se ha creado con éxito
        } else {
            return response.json().then(data => {
                if (data.message) {
                    Swal.fire('Error', data.message, 'error');
                } else {
                    Swal.fire('Error', 'Ocurrió un error al procesar la solicitud.', 'error');
                }
            });
        }
    }).catch(error => {
        Swal.fire('Error', 'Ocurrió un error al procesar la solicitud.', 'error');
        console.error('Error:', error);
    });
}






// Enviar formulario de Editar Comodidad
$('#editForm').submit(function (e) {
    e.preventDefault();

    // Obtener los datos del formulario
    let formData = new FormData(this);

    // Obtener la imagen como base64
    const imageInput = document.getElementById('editImagen');
    const imagePreview = document.getElementById('editImagePreview');

    if (imageInput.files.length > 0) {
        const file = imageInput.files[0];
        const reader = new FileReader();
        reader.onload = function (event) {
            formData.append('imagen', event.target.result); // Agregar imagen como base64 al formulario
            submitEditFormData(formData);
        };
        reader.readAsDataURL(file);
    } else {
        // Si no se selecciona una nueva imagen, dejamos la imagen previa como está
        if (imagePreview.src) {
            formData.append('imagen', imagePreview.src.replace("data:image/jpeg;base64,", ""));
        }
        submitEditFormData(formData);
    }
});

function submitEditFormData(formData) {
    fetch('/Usuarios/Editar', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload(); // Recargar la página si se ha editado correctamente
        } else {
            return response.json().then(data => {
                if (data.message) {
                    Swal.fire('Error', data.message, 'error');
                }
            });
        }
    });
}
function previewImage(event) {
    const file = event.target.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = function (e) {
            // Mostrar la vista previa
            const imagePreview = document.getElementById('imagePreview');
            imagePreview.src = e.target.result;
            imagePreview.style.display = 'block';
        };
        reader.readAsDataURL(file); // Convierte la imagen a base64
    }
}
function previewEditImage(event) {
    const file = event.target.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = function (e) {
            // Mostrar la vista previa
            const imagePreview = document.getElementById('editImagePreview');
            imagePreview.src = e.target.result;
            imagePreview.style.display = 'block';
        };
        reader.readAsDataURL(file); // Convierte la imagen a base64
    }
}