document.addEventListener('DOMContentLoaded', function () {
    // Cargar las comodidades cuando la página esté lista
    fetch('/Comodidades/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbComodidades tbody');
            data.forEach(comodidad => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${comodidad.comodidadId}</td>
                    <td>${comodidad.nombre}</td>
                    <td>${comodidad.precio}</td>
                    <td>
                        <div class="action-buttons">
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${comodidad.comodidadId})">
                                <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${comodidad.comodidadId})">
                                <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${comodidad.comodidadId})">
                                <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                        </div>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            // Inicializar DataTable
            let table = new DataTable('#tbComodidades', {
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

function openEditModal(comodidadId) {
    // Lógica para abrir el modal de edición y cargar los datos de la comodidad
    fetch(`/Comodidades/Obtener/${comodidadId}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('editComodidadId').value = data.comodidadId;
            document.getElementById('editNombre').value = data.nombre;
            document.getElementById('editDescripcion').value = data.descripcion;
            document.getElementById('editPrecio').value = data.precio;
            document.getElementById('editImagePreview').src = data.imagen;
            document.getElementById('editImagePreview').style.display = 'block';

            var editModal = new bootstrap.Modal(document.getElementById('editModal'));
            editModal.show();
        });
}

function openDeleteModal(comodidadId) {
    Swal.fire({
        title: '¿Está seguro de que desea eliminar esta comodidad?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cerrar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Comodidades/Eliminar/${comodidadId}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire('Eliminado!', 'La comodidad ha sido eliminada.', 'success').then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire('Error', 'Hubo un problema al eliminar la comodidad.', 'error');
                }
            }).catch(error => {
                Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
            });
        }
    });
}

function openDetailsModal(comodidadId) {
    fetch(`/Comodidades/Obtener/${comodidadId}`)
        .then(response => response.json())
        .then(data => {
            Swal.fire({
                title: 'Detalles de la Comodidad',
                html: `<p><strong>ComodidadId:</strong> ${data.comodidadId}</p>
                       <p><strong>Nombre:</strong> ${data.nombre}</p>
                       <p><strong>Descripción:</strong> ${data.descripcion}</p>
                       <p><strong>Precio:</strong> ${data.precio}</p>
                       <img src="${data.imagen}" alt="Imagen de la Comodidad" style="width: 100%; height: auto; display: block; margin-top: 10px;" />`,
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
            formData.append('imagen', event.target.result); // Agregar imagen como base64 al formulario
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
}

function submitFormData(formData) {
    fetch('/Comodidades/Crear', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire('Creado!', 'La comodidad ha sido creada con éxito.', 'success').then(() => {
                location.reload(); // Recargar la página si se ha creado con éxito
            });
        } else {
            response.json().then(data => {
                Swal.fire('Error', `Error al crear la comodidad: ${data.message}`, 'error');
            });
        }
    }).catch(error => {
        Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
    });
}

function submitEditFormData(formData) {
    fetch('/Comodidades/Editar', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            Swal.fire('Editado!', 'La comodidad ha sido editada con éxito.', 'success').then(() => {
                location.reload(); // Recargar la página si se ha editado correctamente
            });
        } else {
            response.json().then(data => {
                Swal.fire('Error', `Error al editar la comodidad: ${data.message}`, 'error');
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
