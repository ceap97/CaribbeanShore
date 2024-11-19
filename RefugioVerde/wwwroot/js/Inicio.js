// Espera a que el documento esté completamente cargado
document.addEventListener("DOMContentLoaded", function () {

    // Obtener el botón que abre el modal
    const abrirModalBtn = document.querySelector('#btnAbrirModal');
    // Obtener el modal
    const modal = document.querySelector('#iniciarSesionModal');
    // Obtener el botón de cerrar dentro del modal
    const cerrarModalBtn = modal.querySelector('.btn-close');

    // Función para abrir el modal
    function abrirModal() {
        modal.classList.add('show');
        modal.style.display = 'block';  // Hace visible el modal
        document.body.style.overflow = 'hidden';  // Evita que la página se desplace
    }

    // Función para cerrar el modal
    function cerrarModal() {
        modal.classList.remove('show');
        modal.style.display = 'none';  // Oculta el modal
        document.body.style.overflow = 'auto';  // Restaura el desplazamiento de la página
    }

    // Evento para abrir el modal al hacer clic en el botón
    if (abrirModalBtn) {
        abrirModalBtn.addEventListener('click', abrirModal);
    }

    // Evento para cerrar el modal al hacer clic en el botón de cerrar
    if (cerrarModalBtn) {
        cerrarModalBtn.addEventListener('click', cerrarModal);
    }

    // También cerramos el modal si el usuario hace clic fuera del modal
    window.addEventListener('click', function (event) {
        if (event.target === modal) {
            cerrarModal();
        }
    });

    // Manejo del formulario y validación con SweetAlert
    const formulario = modal.querySelector('form');
    if (formulario) {
        formulario.addEventListener('submit', function (event) {
            event.preventDefault(); // Prevenimos el envío por defecto del formulario

            const correo = formulario.querySelector('input[name="correo"]').value;
            const clave = formulario.querySelector('input[name="clave"]').value;

            // Validar el formulario (puedes agregar más validaciones según necesites)
            if (correo === "" || clave === "") {
                SweetAlert2.fire({
                    icon: 'error',
                    title: 'Error!',
                    text: 'Por favor, completa todos los campos.',
                });
            } else {
                // Simulación de una solicitud AJAX para iniciar sesión
                // Aquí deberías realizar la solicitud al servidor para autenticar al usuario
                SweetAlert2.fire({
                    icon: 'success',
                    title: '¡Éxito!',
                    text: 'Inicio de sesión exitoso.',
                    confirmButtonText: 'OK'
                }).then(function () {
                    // Si la autenticación es exitosa, cierra el modal
                    cerrarModal();
                    // Realiza la redirección o cualquier otra acción post-login aquí
                    window.location.href = '/inicio';  // Redirige a la página de inicio o dashboard
                });
            }
        });
    }

});
