document.addEventListener('DOMContentLoaded', function () {
    fetch('/Roles/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbRoles tbody');
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
            let table = new DataTable('#tbRoles', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

function openCreateModal() {
    $('#createModal').modal('show');
}

function openEditModal(rolId) {
    fetch(`/Roles/Obtener/${rolId}`)
        .then(response => response.json())
        .then(data => {
            $('#editRolId').val(data.rolId);
            $('#editNombre').val(data.nombre);
            $('#editModal').modal('show');
        });
}

function openDeleteModal(rolId) {
    $('#confirmDelete').data('rolId', rolId);
    $('#deleteModal').modal('show');
}

function openDetailsModal(rolId) {
    fetch(`/Roles/Obtener/${rolId}`)
        .then(response => response.json())
        .then(data => {
            $('#detailsRolId').text(data.rolId);
            $('#detailsNombre').text(data.nombre);
            $('#detailsModal').modal('show');
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
            location.reload();
        }
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
            location.reload();
        }
    });
});

$('#confirmDelete').click(function () {
    let rolId = $(this).data('rolId');
    fetch(`/Roles/Eliminar/${rolId}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
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