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

    // Agregar event listeners para la vista previa de imágenes
    document.getElementById('imagen').addEventListener('change', previewImage);
    document.getElementById('editImagen').addEventListener('change', previewEditImage);

    // Agregar event listeners para los formularios
    document.getElementById('createForm').addEventListener('submit', handleCreateFormSubmit);
    document.getElementById('editForm').addEventListener('submit', handleEditFormSubmit);
});

function openCreateModal() {
    var createModal = new bootstrap.Modal(document.getElementById('createModal'));
    createModal.show();
}

function openEditModal(servicioId) {
    fetch(`/Servicios/Obtener/${servicioId}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('editServicioId').value = data.servicioId;
            document.getElementById('editNombre').value = data.nombre;
            document.getElementById('editDescripcion').value = data.descripcion;
            document.getElementById('editPrecio').value = data.precio;
            document.getElementById('editImagePreview').src = `data:image/jpeg;base64,${data.imagen}`;
            document.getElementById('editImagePreview').style.display = 'block';

            var editModal = new bootstrap.Modal(document.getElementById('editModal'));
            editModal.show();
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

    // Obtener la imagen como base64
    const imageInput = document.getElementById('imagen');
    if (imageInput.files.length > 0) {
        const file = imageInput.files[0];
        const reader = new FileReader();
        reader.onload = function (event) {
            formData.append('imagen', event.target.result.split(',').pop()); // Agregar imagen como base64 al formulario
            submitFormData(formData);
        };
        reader.readAsDataURL(file);
    } else {
        submitFormData(formData); // Enviar formulario sin imagen si no se ha seleccionado una
    }
}

function handleEditFormSubmit(e) {
    e.preventDefault();

    // Obtener los datos del formulario
    let formData = new FormData(e.target);

    // Obtener la imagen como base64
    const imageInput = document.getElementById('editImagen');
    const imagePreview = document.getElementById('editImagePreview');

    if (imageInput.files.length > 0) {
        const file = imageInput.files[0];
        const reader = new FileReader();
        reader.onload = function (event) {
            formData.append('imagen', event.target.result.split(',').pop()); // Agregar imagen como base64 al formulario
            submitEditFormData(formData);
        };
        reader.readAsDataURL(file);
    } else {
        // Si no se selecciona una nueva imagen, dejamos la imagen previa como está
        if (imagePreview.src) {
            formData.append('imagen', imagePreview.src.split(',').pop());
        }
        submitEditFormData(formData);
    }
}

function submitFormData(formData) {
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
                Swal.fire('Error', data.message, 'error');
            });
        }
    }).catch(error => {
        Swal.fire('Error', 'Error en la solicitud: ' + error.message, 'error');
    });
}

function submitEditFormData(formData) {
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
                Swal.fire('Error', data.message, 'error');
            });
        }
    }).catch(error => {
        Swal.fire('Error', 'Error en la solicitud: ' + error.message, 'error');
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

