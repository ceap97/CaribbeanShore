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
    $('#confirmDelete').data('permisoId', permisoId);
    $('#deleteModal').modal('show');
}

function openDetailsModal(permisoId) {
    // Lógica para obtener los datos del permiso y mostrar los detalles
    fetch(`/Permisos/Obtener/${permisoId}`)
        .then(response => response.json())
        .then(data => {
            $('#detailsPermisoId').text(data.permisoId);
            $('#detailsNombre').text(data.nombre);
            $('#detailsDescripcion').text(data.descripcion);
            $('#detailsModal').modal('show');
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
            location.reload();
        }
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
            location.reload();
        }
    });
});

$('#confirmDelete').click(function () {
    let permisoId = $(this).data('permisoId');
    // Lógica para eliminar el permiso
    fetch(`/Permisos/Eliminar/${permisoId}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});
