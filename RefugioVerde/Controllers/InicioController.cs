using RefugioVerde.Models;
using RefugioVerde.Recursos;
using RefugioVerde.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace RefugioVerde.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioServices _usuariosServicio;
        public InicioController(IUsuarioServices usuariosServicio)
        {
            _usuariosServicio = usuariosServicio;
        }
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);
            Usuario usuario_creado = await _usuariosServicio.SaveUsuario(modelo);
            if (usuario_creado.UsuarioId > 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "No se pudo crear el usuario" });
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            // Encriptar la clave antes de buscar el usuario
            string claveEncriptada = Utilidades.EncriptarClave(clave);
            Usuario usuario_encontrado = await _usuariosServicio.GetUsuario(correo, claveEncriptada);

            if (usuario_encontrado == null)
            {
                return Json(new { success = false, message = "No se encontraron coincidencias" });
            }

            // Crear los claims, incluyendo el ID del usuario
            List<Claim> claims = new List<Claim>()
    {
        new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario),
        new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.UsuarioId.ToString()), // Claim con el ID del usuario
        new Claim("EmpleadoId", usuario_encontrado.EmpleadoId.ToString() ?? string.Empty)
    };

            // Agregar los permisos del rol del empleado a los claims
            if (usuario_encontrado.Empleado?.Rol != null)
            {
                foreach (var permiso in usuario_encontrado.Empleado.Rol.Permisos)
                {
                    claims.Add(new Claim("Permiso", permiso.Nombre));
                }
            }

            // Crear la identidad y autenticación del usuario
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // Redirigir según el EmpleadoId
            if (usuario_encontrado.EmpleadoId.HasValue)
            {
                return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Usuarios") });
            }
            else
            {
                return Json(new { success = true, redirectUrl = Url.Action("Cliente", "Home") });
            }
        }




    }
}

