document.addEventListener('DOMContentLoaded', function () {
    fetch('/Habitaciones/Listar')
        .then(response => response.json())
        .then(data => {
            let tbody = document.querySelector('#tbHabitaciones tbody');
            data.forEach(habitacion => {
                let tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${habitacion.habitacionId}</td>
                    <td>${habitacion.numero}</td>
                    <td>${habitacion.nombreHabitacion}</td>
                    <td>${habitacion.tipo}</td>
                    <td>${habitacion.precio}</td>
                    <td>${habitacion.estadoHabitacionId}</td>
                    <td>
                        <div class="action-buttons">
                            <button class="btn btn-warning btn-sm" onclick="openEditModal(${habitacion.habitacionId})">
                                <img src="Admin/Fonts/pen-to-square-solid.svg" alt="Editar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-danger btn-sm" onclick="openDeleteModal(${habitacion.habitacionId})">
                                <img src="Admin/Fonts/trash-solid.svg" alt="Eliminar" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                            <button class="btn btn-info btn-sm" onclick="openDetailsModal(${habitacion.habitacionId})">
                                <img src="Admin/Fonts/circle-info-solid.svg" alt="Detalles" style="width: 16px; height: 16px; margin-right: 5px;" />
                            </button>
                        </div>
                    </td>
                `;
                tbody.appendChild(tr);
            });
            let table = new DataTable('#tbHabitaciones', {
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                }
            });
        });
});

function openCreateModal() {
    $('#createModal').modal('show');
    loadEstadosHabitacion(); // Carga estados de habitación en el modal de crear
}

function openEditModal(habitacionId) {
    fetch(`/Habitaciones/Obtener/${habitacionId}`)
        .then(response => response.json())
        .then(data => {
            $('#editHabitacionId').val(data.habitacionId);
            $('#editNumero').val(data.numero);
            $('#editNombreHabitacion').val(data.nombreHabitacion);
            $('#editTipo').val(data.tipo);
            $('#editPrecio').val(data.precio);
            $('#editEstadoHabitacionId').val(data.estadoHabitacionId);
            $('#editModal').modal('show');
            loadEstadosHabitacion(); // Carga estados de habitación en el modal de editar
        });
}

function openDeleteModal(habitacionId) {
    Swal.fire({
        title: '¿Está seguro de que desea eliminar esta habitación?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cerrar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Habitaciones/Eliminar/${habitacionId}`, {
                method: 'DELETE'
            }).then(response => {
                if (response.ok) {
                    Swal.fire('Eliminado!', 'La habitación ha sido eliminada.', 'success').then(() => {
                        location.reload();
                    });
                } else {
                    Swal.fire('Error', 'Hubo un problema al eliminar la habitación.', 'error');
                }
            }).catch(error => {
                Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
            });
        }
    });
}


function openDetailsModal(habitacionId) {
    fetch(`/Habitaciones/Obtener/${habitacionId}`)
        .then(response => response.json())
        .then(data => {
            Swal.fire({
                title: 'Detalles de la Habitación',
                html: `<p><strong>Habitación ID:</strong> ${data.habitacionId}</p>
                       <p><strong>Número:</strong> ${data.numero}</p>
                       <p><strong>Nombre:</strong> ${data.nombreHabitacion}</p>
                       <p><strong>Tipo:</strong> ${data.tipo}</p>
                       <p><strong>Precio:</strong> ${data.precio}</p>
                       <p><strong>Estado:</strong> ${data.estadoHabitacionId}</p>`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        });
}


$('#createForm').submit(function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    fetch('/Habitaciones/Crear', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

$('#editForm').submit(function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    fetch('/Habitaciones/Editar', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

$('#confirmDelete').click(function () {
    let habitacionId = $(this).data('habitacionId');
    fetch(`/Habitaciones/Eliminar/${habitacionId}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {
            location.reload();
        }
    });
});

function loadEstadosHabitacion() {
    fetch('/EstadoHabitaciones/Listar')  // Asegúrate de que la ruta sea correcta
        .then(response => response.json())
        .then(data => {
            // Para los selects de estado (Crear y Editar)
            let estadoSelects = document.querySelectorAll('#estadoHabitacionId, #editEstadoHabitacionId');
            estadoSelects.forEach(select => {
                select.innerHTML = `<option value="">Seleccione un Estado</option>`;
                data.forEach(estado => {
                    let option = `<option value="${estado.estadoHabitacionId}" ${estado.estadoHabitacionId}>${estado.nombre}</option>`;
                    select.innerHTML += option;
                });
            });

            // Mostrar el nombre del estado en los detalles y en el listado
            updateEstadoHabitacion(data);
        });
}

// Función para mostrar el nombre del estado en el listado de habitaciones y en los detalles
function updateEstadoHabitacion(data) {
    // Listado de habitaciones: muestra el nombre del estado en lugar del ID
    let rows = document.querySelectorAll('#tbHabitaciones tbody tr');
    rows.forEach(row => {
        let estadoIdCell = row.cells[5]; // Asumiendo que el estado está en la columna 5 (índice 4)
        let estadoId = estadoIdCell.innerText.trim();

        let estado = data.find(e => e.estadoHabitacionId == estadoId);
        if (estado) {
            estadoIdCell.innerText = estado.nombre;  // Cambiar ID por nombre
        }
    });

    // Detalles: mostrar el nombre del estado en lugar del ID
    let estadoIdDetail = document.getElementById('detailsEstado');
    if (estadoIdDetail) {
        let estadoId = estadoIdDetail.innerText.trim();
        let estado = data.find(e => e.estadoHabitacionId == estadoId);
        if (estado) {
            estadoIdDetail.innerText = estado.nombre;  // Mostrar el nombre en lugar del ID
        }
    }
}

// Llamar la función de carga de estados al cargar la página o al obtener datos
document.addEventListener('DOMContentLoaded', function () {
    loadEstadosHabitacion(); // Cargar los estados al iniciar
});
