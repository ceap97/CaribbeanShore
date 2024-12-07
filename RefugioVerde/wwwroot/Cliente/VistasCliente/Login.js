document.getElementById('btnIniciarSesion').addEventListener('click', function () {
    mostrarModalIniciarSesion();
});

function mostrarModalIniciarSesion() {
    Swal.fire({
        title: 'Iniciar Sesión',
        html: `
                <form id="loginForm">
                    <div class="form-group mb-3">
                        <label for="correo">Correo</label>
                        <input class="form-control" type="text" placeholder="Correo" required name="correo" id="correo" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="clave">Contraseña</label>
                        <input class="form-control" type="password" placeholder="Contraseña" required name="clave" id="clave" />
                    </div>
                    <button type="submit" class="btn btn-primary">Iniciar Sesion</button>
                    <button type="button" class="btn btn-secondary" id="btnRegistrarse">Registrarse</button>
                    <button type="button" class="btn btn-danger" id="btnGoogleLogin">Iniciar con Google</button>
                    <button type="button" class="btn btn-link" id="btnOlvideContraseña">
                        Olvidé mi contraseña
                    </button>
                </form>
            `,
        showConfirmButton: false
    });

    document.getElementById('loginForm').addEventListener('submit', function (e) {
        e.preventDefault();
        var form = new FormData(this);

        fetch('/Inicio/IniciarSesion', {
            method: 'POST',
            body: form
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    if (data.redirectUrl.includes("Cliente")) {
                        window.location.href = "/Cliente";
                    } else {
                        window.location.href = data.redirectUrl;
                    }
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message,
                        showConfirmButton: true
                    }).then(() => {
                        mostrarModalIniciarSesion();
                    });
                }
            });
    });

    document.getElementById('btnRegistrarse').addEventListener('click', function () {
        mostrarModalRegistrarse();
    });

    document.getElementById('btnGoogleLogin').addEventListener('click', function () {
        window.location.href = '/Inicio/GoogleLogin';
    });

    document.getElementById('btnOlvideContraseña').addEventListener('click', function() {
        mostrarModalRecuperarContraseña();
    });
}

function mostrarModalRegistrarse() {
    Swal.fire({
        title: 'Registrarse',
        html: `
        <div class="card-body p-4">
                <form id="registerForm">
                <div class="row mb-3">
                    <div class="col-md-6">
                        <label for="nombreUsuario">Nombre de Usuario</label>
                        <input class="form-control" type="text" placeholder="Nombre de Usuario" required name="nombreUsuario" id="nombreUsuario" />
                    </div>
                   <div class="col-md-6">
                    <label for="correo">Correo</label>
                        <input class="form-control" type="email" placeholder="Correo" required name="correo" id="correo" />
                       
                    </div>
                    <div class="col-md-6">
                     <label for="nombre">Nombre</label>
                        <input class="form-control" type="text" placeholder="Nombre" required name="nombre" id="nombre" />
                       
                    </div>
                    <div class="col-md-6">
                     <label for="apellido">Apellido</label>
                        <input class="form-control" type="text" placeholder="Apellido" required name="apellido" id="apellido" />
                       
                    </div>
                     <div class="col-md-6">
                        <label for="documentoIdentidad">Documento</label>
                        <input class="form-control" type="text" placeholder="Documento de Identidad" required name="documentoIdentidad" id="documentoIdentidad" />
                    </div>
                   
                    <div class="col-md-6">
                     <label for="genero">Género</label>
                        <select class="form-control" required name="genero" id="genero">
                            <option value="">Seleccione su género</option>
                            <option value="Masculino">Masculino</option>
                            <option value="Femenino">Femenino</option>
                            <option value="Otro">Otro</option>
                        </select>
                       
                    </div>
                    <div class="col-md-6">
                           <label for="direccion">Dirección</label>
                        <input class="form-control" type="text" placeholder="Dirección" required name="direccion" id="direccion" />
                    </div>
                    <div class="col-md-6">
                        <label for="telefono">Teléfono</label>
                        <input class="form-control" type="text" placeholder="Teléfono" required name="telefono" id="telefono" />
                    </div>
                    <div class="form-group mb-3">
                    <label for="clave">Contraseña</label>
                        <input class="form-control" type="password" placeholder="Contraseña" required name="clave" id="clave" />
                    </div>
                    <button type="submit" class="btn btn-primary">Registrarse</button>
                    </div>
                </form>
                </div>
            `,
        showConfirmButton: false
    });

    document.getElementById('registerForm').addEventListener('submit', function (e) {
        e.preventDefault();
        var form = new FormData(this);

        fetch('/Inicio/Registrarse', {
            method: 'POST',
            body: form
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(error => {
                        throw new Error(error.message);
                    });
                }
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Registro Exitoso',
                        text: '¡Te has registrado exitosamente!',
                        showConfirmButton: true
                    }).then(() => {
                        // Después de un registro exitoso, iniciamos sesión automáticamente
                        iniciarSesionAutomatico(form.get('correo'), form.get('clave'));
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message,
                        showConfirmButton: true
                    }).then(() => {
                        mostrarModalRegistrarse();
                    });
                }
            })
            .catch(error => {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: error.message,
                    showConfirmButton: true
                });
            });
    });
}




function iniciarSesionAutomatico(correo, clave) {
    var formData = new FormData();
    formData.append('correo', correo);
    formData.append('clave', clave);

    fetch('/Inicio/IniciarSesion', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                if (data.redirectUrl.includes("Cliente")) {
                    window.location.href = "/Cliente";
                } else {
                    window.location.href = data.redirectUrl;
                }
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: data.message,
                    showConfirmButton: true
                }).then(() => {
                    mostrarModalIniciarSesion();
                });
            }
        });
}

function mostrarModalRecuperarContraseña() {
    Swal.fire({
        title: 'Recuperar Contraseña',
        html: `
            <form id="recuperarForm">
                <div class="form-group mb-3">
                    <label for="correo">Correo Electrónico</label>
                    <input class="form-control" type="email" required 
                        placeholder="Ingrese su correo" id="correo" name="correo">
                </div>
                <button type="submit" class="btn btn-primary">Enviar</button>
            </form>
        `,
        showConfirmButton: false
    });

    document.getElementById('recuperarForm').addEventListener('submit', function(e) {
        e.preventDefault();
        var form = new FormData(this);

        fetch('/Usuarios/SolicitarRestablecerContraseña', {
            method: 'POST',
            body: form
        })
        .then(response => response.json())
        .then(data => {
            if (data.message) {
                mostrarModalIngresarToken(form.get('correo'));
            }
        })
        .catch(error => {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al procesar la solicitud.'
            });
        });
    });
}

function mostrarModalIngresarToken(correo) {
    Swal.fire({
        title: 'Restablecer Contraseña',
        html: `
            <form id="restablecerForm">
                <input type="hidden" name="correo" value="${correo}">
                <div class="form-group mb-3">
                    <label for="token">Código de Verificación</label>
                    <input class="form-control" type="text" required 
                        placeholder="Ingrese el código" id="token" name="token">
                </div>
                <div class="form-group mb-3">
                    <label for="nuevaClave">Nueva Contraseña</label>
                    <input class="form-control" type="password" required 
                        placeholder="Nueva contraseña" id="nuevaClave" name="nuevaClave">
                </div>
                <button type="submit" class="btn btn-primary">Restablecer</button>
            </form>
        `,
        showConfirmButton: false,
        allowOutsideClick: false
    });

    document.getElementById('restablecerForm').addEventListener('submit', function(e) {
        e.preventDefault();
        var formData = new FormData(this);

        Swal.showLoading();

        fetch('/Usuarios/RestablecerContraseña', {
            method: 'POST',
            body: formData
        })
        .then(response => {
            if (!response.ok) {
                return response.json().then(error => {
                    throw new Error(error.message || 'Error al restablecer la contraseña');
                });
            }
            return response.json();
        })
        .then(data => {
            Swal.fire({
                icon: 'success',
                title: 'Éxito',
                text: 'Contraseña actualizada correctamente',
                confirmButtonText: 'Iniciar Sesión'
            }).then(() => {
                mostrarModalIniciarSesion();
            });
        })
        .catch(error => {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: error.message || 'Ocurrió un error al restablecer la contraseña'
            }).then(() => {
                // Volver a mostrar el modal de ingreso de token
                mostrarModalIngresarToken(correo);
            });
        });
    });
}