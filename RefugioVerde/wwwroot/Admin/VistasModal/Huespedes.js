document.addEventListener('DOMContentLoaded', function () {
    // Cargar los datos de huéspedes
    fetch('/Huespedes/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbHuespedes tbody');
            data.forEach(huesped => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${huesped.huespedId}</td>
                    <td>${huesped.nombre}</td>
                    <td>${huesped.apellido}</td>
                    <td>${huesped.documentoIdentidad}</td>
                    <td>${huesped.telefono}</td>
                    <td>${huesped.email}</td>
                    <td>${huesped.municipio.nombre}</td>
                    <td>
                        <button class="btn btn-warning btn-sm" onclick="openEditModal(${huesped.huespedId})">
                            <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${huesped.huespedId})">
                            <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-info btn-sm" onclick="openDetailsModal(${huesped.huespedId})">
                            <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px;" />
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            // Inicializar DataTable para la tabla
            let table = new DataTable('#tbHuespedes', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });

    //// Agregar event listeners para los formularios
    //document.getElementById('createForm').addEventListener('submit', handleCreateFormSubmit);
    //document.getElementById('editForm').addEventListener('submit', handleEditFormSubmit);
});

function openCreateModal() {
    loadMunicipios('#municipioId');
    loadMunicipios('#reservaId'); 
    var createModal = new bootstrap.Modal(document.getElementById('createModal'));
    createModal.show();
}

function openEditModal(huespedId) {
    // Cargar los datos del huésped
    fetch(`/Huespedes/Obtener/${huespedId}`)
        .then(response => response.json())
        .then(huesped => {
            document.getElementById('editHuespedId').value = huesped.huespedId;
            document.getElementById('editNombre').value = huesped.nombre;
            document.getElementById('editApellido').value = huesped.apellido;
            document.getElementById('editDocumentoIdentidad').value = huesped.documentoIdentidad;
            document.getElementById('editTelefono').value = huesped.telefono;
            document.getElementById('editEmail').value = huesped.email;
            loadMunicipios('#editMunicipioId'); // Cargar municipios

            var editModal = new bootstrap.Modal(document.getElementById('editModal'));
            editModal.show();
        });
}

function openDetailsModal(huespedId) {
    fetch(`/Huespedes/Obtener/${huespedId}`)
        .then(response => response.json())
        .then(huesped => {
            Swal.fire({
                title: `Detalles de ${huesped.nombre} ${huesped.apellido}`,
                html: `
                    <p><strong>HuespedId:</strong> ${huesped.huespedId}</p>
                    <p><strong>Nombre:</strong> ${huesped.nombre}</p>
                    <p><strong>Apellido:</strong> ${huesped.apellido}</p>
                    <p><strong>Documento de Identidad:</strong> ${huesped.documentoIdentidad}</p>
                    <p><strong>Teléfono:</strong> ${huesped.telefono}</p>
                    <p><strong>Email:</strong> ${huesped.email}</p>
                    <p><strong>Municipio:</strong> ${huesped.municipio.nombre}</p>
                `,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

function openDeleteModal(huespedId) {
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
            fetch(`/Huespedes/Eliminar/${huespedId}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Eliminado!',
                            'El huésped ha sido eliminado correctamente.',
                            'success'
                        ).then(() => location.reload());
                    } else {
                        Swal.fire(
                            'Error',
                            'Hubo un problema al intentar eliminar el huésped.',
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

function loadMunicipios(selectId) {
    fetch('/Municipios/Listar')
        .then(response => response.json())
        .then(data => {
            let municipioSelect = document.querySelector(selectId);
            municipioSelect.innerHTML = '<option value="">Seleccione un Municipio</option>';
            data.forEach(municipio => {
                let option = document.createElement('option');
                option.value = municipio.municipioId;
                option.textContent = municipio.nombre;
                municipioSelect.appendChild(option);
            });
        });
}

function handleCreateFormSubmit(e) {
    e.preventDefault();

    // Obtener los datos del formulario
    let formData = new FormData(e.target);

    // Enviar los datos al servidor
    fetch('/Huespedes/Crear', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (response.ok) {
                Swal.fire(
                    'Creado!',
                    'El huésped ha sido creado correctamente.',
                    'success'
                ).then(() => location.reload());
            } else {
                Swal.fire(
                    'Error',
                    'Hubo un problema al intentar crear el huésped.',
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

function handleEditFormSubmit(e) {
    e.preventDefault();

    // Obtener los datos del formulario
    let formData = new FormData(e.target);

    // Enviar los datos al servidor
    fetch('/Huespedes/Editar', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (response.ok) {
                Swal.fire(
                    'Actualizado!',
                    'El huésped ha sido actualizado correctamente.',
                    'success'
                ).then(() => location.reload());
            } else {
                Swal.fire(
                    'Error',
                    'Hubo un problema al intentar actualizar el huésped.',
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
