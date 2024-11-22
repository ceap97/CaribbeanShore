// EditarCliente.js
function openEditModal() {
    Swal.fire({
        title: 'Editar Cliente',
        html: `
            <div class="modal-body">
                <form id="editForm">
                    <input type="hidden" id="editClienteId" name="clienteId">
                    <div class="mb-3">
                        <label for="editNombre" class="form-label">Nombre</label>
                        <input type="text" class="form-control" id="editNombre" name="nombre" required minlength="2" maxlength="20" pattern="^[a-zA-Z\s]+$" title="Por favor, ingrese un nombre entre 2 y 20 caracteres que solo contenga letras.">
                        <div class="form-text">El nombre debe ser solo letras y entre 2 y 20 caracteres.</div>
                    </div>
                    <div class="mb-3">
                        <label for="editApellido" class="form-label">Apellido</label>
                        <input type="text" class="form-control" id="editApellido" name="apellido" required minlength="2" maxlength="20" pattern="^[a-zA-Z\s]+$" title="Por favor, ingrese un apellido entre 2 y 20 caracteres que solo contenga letras.">
                        <div class="form-text">El apellido debe ser solo letras y entre 2 y 20 caracteres.</div>
                    </div>
                    <div class="mb-3">
                        <label for="editDocumentoIdentidad" class="form-label">Documento de Identidad</label>
                        <input type="text" class="form-control" id="editDocumentoIdentidad" name="documentoIdentidad" required minlength="8" maxlength="18" title="Ingrese un documento de identidad válido entre 8 y 18 caracteres.">
                        <div class="form-text">Debe tener entre 8 y 18 caracteres.</div>
                    </div>
                    <div class="mb-3">
                        <label for="editTelefono" class="form-label">Teléfono</label>
                        <input type="text" class="form-control" id="editTelefono" name="telefono" required pattern="\\d{10}" title="El teléfono debe contener exactamente 10 dígitos numéricos.">
                        <div class="form-text">Debe contener exactamente 10 dígitos.</div>
                    </div>
                    <div class="mb-3">
                        <label for="editCorreo" class="form-label">Correo</label>
                        <input type="email" class="form-control" id="editCorreo" name="correo" required title="Ingrese un correo electrónico válido.">
                        <div class="form-text">Debe ser un correo electrónico válido (ejemplo@dominio.com).</div>
                    </div>
                    <div class="mb-3">
                        <label for="editMunicipioId" class="form-label">Municipio</label>
                        <select class="form-select" id="editMunicipioId" name="municipioId">
                            <option value="">Seleccione un municipio</option>
                        </select>
                    </div>
                    <div class="mb-3">
                        <label for="editUsuarioId" class="form-label">Usuario</label>
                        <select class="form-select" id="editUsuarioId" name="usuarioId" required>
                            <option value="">Seleccione un usuario</option>
                        </select>
                    </div>
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <button type="submit" class="btn btn-primary">Guardar</button>
                </form>
            </div>
        `,
        showConfirmButton: false,
        width: '600px'
    });
}
