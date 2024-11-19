document.getElementById('btnIniciarSesion').addEventListener('click', function () {
    mostrarModalIniciarSesion();
});

function mostrarModalIniciarSesion() {
    Swal.fire({
        title: 'Iniciar Sesion',
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
}

function mostrarModalRegistrarse() {
    Swal.fire({
        title: 'Registrarse',
        html: `
                <form id="registerForm">
                    <div class="form-group mb-3">
                        <label for="nombreUsuario">Nombre de Usuario</label>
                        <input class="form-control" type="text" placeholder="Nombre de Usuario" required name="nombreUsuario" id="nombreUsuario" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="correo">Correo</label>
                        <input class="form-control" type="email" placeholder="Correo" required name="correo" id="correo" />
                    </div>
                    <div class="form-group mb-3">
                        <label for="clave">Contraseña</label>
                        <input class="form-control" type="password" placeholder="Contraseña" required name="clave" id="clave" />
                    </div>
                    <button type="submit" class="btn btn-primary">Registrarse</button>
                </form>
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
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Registro Exitoso',
                        text: '¡Te has registrado exitosamente!',
                        showConfirmButton: true
                    }).then(() => {
                        mostrarModalIniciarSesion();
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
            });
    });
}