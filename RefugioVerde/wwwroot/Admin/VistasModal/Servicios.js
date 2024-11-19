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
            $('#editModal').modal('show');
        });
}

function openDeleteModal(servicioId) {
    $('#confirmDelete').data('servicioId', servicioId);
    $('#deleteModal').modal('show');
}

function openDetailsModal(servicioId) {
    fetch(`/Servicios/Obtener/${servicioId}`)
        .then(response => response.json())
        .then(data => {
            $('#detailsServicioId').text(data.servicioId);
            $('#detailsNombre').text(data.nombre);
            $('#detailsDescripcion').text(data.descripcion);
            $('#detailsPrecio').text(data.precio);
            $('#detailsModal').modal('show');
        });
}

$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/Servicios/Crear', {
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
    let formData = $(this).serialize();
    fetch('/Servicios/Editar', {
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
    let servicioId = $(this).data('servicioId');
    fetch(`/Servicios/Eliminar/${servicioId}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});
