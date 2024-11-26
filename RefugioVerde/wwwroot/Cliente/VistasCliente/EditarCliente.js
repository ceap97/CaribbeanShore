

    document.body.insertAdjacentHTML('beforeend', modalTemplate);
    loadMunicipios().then(() => {
        // Establecer el usuario por defecto como el usuario asociado al usuario que inició sesión
        fetch('/Usuarios/ObtenerUsuarioActual')
            .then(response => response.json())
            .then(data => {
                document.getElementById('usuarioId').value = data.usuarioId;
            });
    });
    $('#clientModal').modal('show');

    document.getElementById('clientForm').addEventListener('submit', function (e) {
        e.preventDefault();
        let formData = new FormData(this);
        console.log('Datos enviados:', Object.fromEntries(formData.entries())); // Verificar los datos enviados
        fetch('/Clientes/Crear', {
            method: 'POST',
            body: formData
        }).then(response => {
            if (response.ok) {
                $('#clientModal').modal('hide');
                location.reload();
            } else {
                Swal.fire('Error', 'Hubo un problema al crear el cliente.', 'error');
            }
        }).catch(error => {
            Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
        });
    });


function openEditClientModal(clienteId) {
    const modalTemplate = `
        <div class="modal fade" id="clientModal" tabindex="-1" aria-labelledby="clientModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="clientModalLabel">Editar Cliente</h5>
                    </div>
                    <div class="modal-body">
                        <form id="clientForm">
                            <input type="hidden" id="usuarioId" name="usuarioId" required>
                            <input type="hidden" id="clienteId" name="clienteId">
                            <div class="mb-3">
                                <label for="nombre" class="form-label">Nombre</label>
                                <input type="text" class="form-control" id="nombre" name="nombre" required>
                            </div>
                            <div class="mb-3">
                                <label for="apellido" class="form-label">Apellido</label>
                                <input type="text" class="form-control" id="apellido" name="apellido" required>
                            </div>
                            <div class="mb-3">
                                <label for="documentoIdentidad" class="form-label">Documento de Identidad</label>
                                <input type="text" class="form-control" id="documentoIdentidad" name="documentoIdentidad" required>
                            </div>
                            <div class="mb-3">
                                <label for="municipioId" class="form-label">Municipio</label>
                                <select class="form-control" id="municipioId" name="municipioId" required></select>
                            </div>
                            <div class="mb-3">
                                <label for="telefono" class="form-label">Teléfono</label>
                                <input type="tel" class="form-control" id="telefono" name="telefono" required>
                            </div>
                            <div class="mb-3">
                                <label for="correo" class="form-label">Correo</label>
                                <input type="email" class="form-control" id="correo" name="correo" required>
                            </div>
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            <button type="submit" class="btn btn-primary">Guardar</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalTemplate);
    loadMunicipios().then(() => {
        fetch(`/Clientes/Obtener/${clienteId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('clienteId').value = data.clienteId;
                document.getElementById('nombre').value = data.nombre;
                document.getElementById('apellido').value = data.apellido;
                document.getElementById('documentoIdentidad').value = data.documentoIdentidad;
                document.getElementById('municipioId').value = data.municipioId;
                document.getElementById('telefono').value = data.telefono;
                document.getElementById('correo').value = data.correo;
                document.getElementById('usuarioId').value = data.usuarioId;
            });
    });
    $('#clientModal').modal('show');

    document.getElementById('clientForm').addEventListener('submit', function (e) {
        e.preventDefault();
        let formData = new FormData(this);
        console.log('Datos enviados:', Object.fromEntries(formData.entries())); // Verificar los datos enviados
        fetch('/Clientes/Editar', {
            method: 'POST',
            body: formData
        }).then(response => {
            if (response.ok) {
                $('#clientModal').modal('hide');
                location.reload();
            } else {
                Swal.fire('Error', 'Hubo un problema al editar el cliente.', 'error');
            }
        }).catch(error => {
            Swal.fire('Error', 'Hubo un problema en la solicitud.', 'error');
        });
    });
}

function loadMunicipios() {
    return fetch('/Municipios/Listar')
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al obtener la lista de municipios');
            }
            return response.json();
        })
        .then(data => {
            let municipioSelect = document.querySelector('#municipioId');
            municipioSelect.innerHTML = '<option value="">Seleccione un municipio</option>';
            data.forEach(municipio => {
                let option = document.createElement('option');
                option.value = municipio.municipioId;
                option.textContent = municipio.nombre;
                municipioSelect.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire('Error', 'Hubo un problema al cargar la lista de municipios.', 'error');
        });
}

