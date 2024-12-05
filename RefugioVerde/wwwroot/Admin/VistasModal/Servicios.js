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
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${servicio.servicioId})">
                                <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${servicio.servicioId})">
                                <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${servicio.servicioId})">
                                <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
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

    // Agregar event listeners para los formularios
    document.getElementById('createForm').addEventListener('submit', handleCreateFormSubmit);
    document.getElementById('editForm').addEventListener('submit', handleEditFormSubmit);
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
            $('#editImagePreview').attr('src', `data:image/jpeg;base64,${data.imagen}`).show();
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
                } else {
                    Swal.fire('Error', 'Hubo un problema al eliminar el servicio.', 'error');
                }
            }).catch(error => {
                Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
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
                html: `<p><strong>Servicio ID:</strong> ${data.servicioId}</p>
                       <p><strong>Nombre:</strong> ${data.nombre}</p>
                       <p><strong>Descripción:</strong> ${data.descripcion}</p>
                       <p><strong>Precio:</strong> ${data.precio}</p>
                       <img src="data:image/jpeg;base64,${data.imagen}" alt="Imagen del Servicio" style="width: 100%; height: auto; display: block; margin-top: 10px;" />`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

function handleCreateFormSubmit(e) {
    e.preventDefault();

    // Obtener los datos del formulario
    let formData = new FormData(e.target);

    fetch('/Servicios/Crear', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire('Creado!', 'El servicio ha sido creado con éxito.', 'success').then(() => {
                location.reload(); // Recargar la página si se ha creado con éxito
            });
        } else {
            response.json().then(data => {
                Swal.fire('Error', `Error al crear el servicio: ${data.message}`, 'error');
            });
        }
    }).catch(error => {
        Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
    });
}

function handleEditFormSubmit(e) {
    e.preventDefault();

    // Obtener los datos del formulario
    let formData = new FormData(e.target);

    fetch('/Servicios/Editar', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire('Editado!', 'El servicio ha sido editado con éxito.', 'success').then(() => {
                location.reload(); // Recargar la página si se ha editado correctamente
            });
        } else {
            response.json().then(data => {
                Swal.fire('Error', `Error al editar el servicio: ${data.message}`, 'error');
            });
        }
    }).catch(error => {
        Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
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
