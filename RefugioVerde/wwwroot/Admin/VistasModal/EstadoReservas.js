document.addEventListener('DOMContentLoaded', function () {
    // Cargar los datos de Estado de Reservas
    fetch('/EstadoReservas/Listar')  // Suponiendo que tienes un endpoint '/EstadoReservas/Listar'
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbEstadoReservas tbody');
            data.forEach(estado => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${estado.estadoReservaId}</td>
                    <td>${estado.nombre}</td>
                    <td>
                        <button class="btn btn-warning btn-sm" onclick="openEditModal(${estado.estadoReservaId})">
                            <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${estado.estadoReservaId})">
                            <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px;" />
                        </button>
                        <button class="btn btn-info btn-sm" onclick="openDetailsModal(${estado.estadoReservaId})">
                            <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px;" />
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });

            // Inicializar DataTable para la tabla
            let table = new DataTable('#tbEstadoReservas', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

// Abre el modal de creación
function openCreateModal() {
    $('#createModal').modal('show');
}

// Abre el modal de edición
function openEditModal(estadoReservaId) {
    // Cargar los datos del estado de reserva
    fetch(`/EstadoReservas/Obtener/${estadoReservaId}`)  // Suponiendo que tienes un endpoint '/EstadoReservas/Obtener/{id}'
        .then(response => response.json())
        .then(estado => {
            $('#editEstadoReservaId').val(estado.estadoReservaId);
            $('#editNombre').val(estado.nombre);
            $('#editModal').modal('show');
        });
}

// Abre el modal de detalles con SweetAlert2
function openDetailsModal(estadoReservaId) {
    fetch(`/EstadoReservas/Obtener/${estadoReservaId}`)
        .then(response => response.json())
        .then(estado => {
            Swal.fire({
                title: `Detalles del Estado de Reserva`,
                html: `
                    <p><strong>EstadoReservaId:</strong> ${estado.estadoReservaId}</p>
                    <p><strong>Nombre:</strong> ${estado.nombre}</p>
                `,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}

// Abre el modal de eliminación con SweetAlert2
function openDeleteModal(estadoReservaId) {
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
            fetch(`/EstadoReservas/Eliminar/${estadoReservaId}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Eliminado!',
                            'El estado de reserva ha sido eliminado correctamente.',
                            'success'
                        ).then(() => location.reload());
                    } else {
                        Swal.fire(
                            'Error',
                            'Hubo un problema al intentar eliminar el estado de reserva.',
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

// Función para cargar el formulario de creación
$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/EstadoReservas/Crear', {  // Suponiendo que tienes un endpoint '/EstadoReservas/Crear'
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

// Función para cargar el formulario de edición
$('#editForm').submit(function (e) {
    e.preventDefault();
    let formData = $(this).serialize();
    fetch('/EstadoReservas/Editar', {  // Suponiendo que tienes un endpoint '/EstadoReservas/Editar'
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
