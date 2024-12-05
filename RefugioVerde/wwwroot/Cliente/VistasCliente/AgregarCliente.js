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
                                <input type="text" class="form-control" id="nombre" name="nombre" required minlength="2" maxlength="20" pattern="^[a-zA-Z\s]+$" title="Por favor, ingrese un nombre entre 2 y 20 caracteres que solo contenga letras.">
                            </div>
                            <div class="mb-3">
                                <label for="apellido" class="form-label">Apellido</label>
                                <input type="text" class="form-control" id="apellido" name="apellido" required minlength="2" maxlength="20" pattern="^[a-zA-Z\s]+$" title="Por favor, ingrese un apellido entre 2 y 20 caracteres que solo contenga letras.">
                            </div>
                            <div class="mb-3">
                                <label for="documentoIdentidad" class="form-label">Documento de Identidad</label>
                                <input type="text" class="form-control" id="documentoIdentidad" name="documentoIdentidad" required minlength="8" maxlength="18" title="Ingrese un documento de identidad válido entre 8 y 18 caracteres.">
                            </div>
                            <div class="mb-3">
                                <label for="telefono" class="form-label">Teléfono</label>
                                <input type="tel" class="form-control" id="telefono" name="telefono" required pattern="\\d{10}" title="El teléfono debe contener exactamente 10 dígitos numéricos.">
                            </div>
                            <div class="mb-3">
                                <label for="correo" class="form-label">Correo</label>
                                <input type="email" class="form-control" id="correo" name="correo" required title="Ingrese un correo electrónico válido.">
                            </div>
                            <div class="mb-3">
                                <label for="direccion" class="form-label">Dirección</label>
                                <input type="text" class="form-control" id="direccion" name="direccion">
                            </div>
                            <div class="mb-3">
                                <label for="genero" class="form-label">Género</label>
                                <select class="form-select" id="genero" name="genero">
                                    <option value="">Seleccione un género</option>
                                    <option value="Masculino">Masculino</option>
                                    <option value="Femenino">Femenino</option>
                                    <option value="Otro">Otro</option>
                                </select>
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
    // Establecer el usuario por defecto como el usuario asociado al usuario que inició sesión
    fetch('/Usuarios/ObtenerUsuarioActual')
        .then(response => response.json())
        .then(data => {
            document.getElementById('usuarioId').value = data.usuarioId;
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

