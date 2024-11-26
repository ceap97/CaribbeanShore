function openAddClientModal() {
    const modalTemplate = `
        <div class="modal fade" id="addClientModal" tabindex="-1" aria-labelledby="addClientModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="addClientModalLabel">Agregar Cliente</h5>
                    </div>
                    <div class="modal-body">
                        <form id="addClientForm">
                            <input type="hidden" id="usuarioId" name="usuarioId" required>
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
        // Establecer el usuario por defecto como el usuario asociado al usuario que inició sesión
        fetch('/Usuarios/ObtenerUsuarioActual')
            .then(response => response.json())
            .then(data => {
                document.getElementById('usuarioId').value = data.usuarioId;
            });
    });
    $('#addClientModal').modal('show');

    document.getElementById('addClientForm').addEventListener('submit', function (e) {
        e.preventDefault();
        let formData = new FormData(this);
        console.log('Datos enviados:', Object.fromEntries(formData.entries())); // Verificar los datos enviados
        fetch('/Clientes/Crear', {
            method: 'POST',
            body: formData
        }).then(response => {
            if (response.ok) {
                $('#addClientModal').modal('hide');
                location.reload();
            } else {
                Swal.fire('Error', 'Hubo un problema al crear el cliente.', 'error');
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
