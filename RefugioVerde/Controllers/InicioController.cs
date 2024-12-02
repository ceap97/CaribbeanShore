using RefugioVerde.Models;
using RefugioVerde.Recursos;
using RefugioVerde.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;

namespace RefugioVerde.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioServices _usuariosServicio;
        private readonly ILogger<InicioController> _logger;

        public InicioController(IUsuarioServices usuariosServicio, ILogger<InicioController> logger)
        {
            _usuariosServicio = usuariosServicio;
            _logger = logger;
        }
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Inicio");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                }).ToList();

            // Obtener el correo electrónico del usuario autenticado
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (emailClaim == null)
            {
                return RedirectToAction("IniciarSesion", "Inicio");
            }

            // Buscar el usuario en la base de datos
            Usuario usuario = await _usuariosServicio.GetUsuarioPorCorreo(emailClaim);
            if (usuario == null)
            {
                // Crear un nuevo usuario si no existe
                usuario = new Usuario
                {
                    NombreUsuario = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                    Correo = emailClaim,
                    Clave = Utilidades.EncriptarClave(Guid.NewGuid().ToString()), // Generar una clave aleatoria
                    Imagen = claims.FirstOrDefault(c => c.Type == "picture")?.Value
                };
                usuario = await _usuariosServicio.SaveUsuario(usuario);
            }

            // Crear los claims para la autenticación
            List<Claim> userClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
        new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
        new Claim(ClaimTypes.Email, usuario.Correo)
    };

            // Crear la identidad y autenticación del usuario
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return RedirectToAction("Cliente", "Home");
        }



        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            try
            {
                // Verificar si el correo ya está registrado
                var usuarioExistente = await _usuariosServicio.GetUsuarioPorCorreo(modelo.Correo);
                if (usuarioExistente != null)
                {
                    return BadRequest(new { message = "El correo ya está registrado. Intente con otro." });
                }

                modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);
                Usuario usuario_creado = await _usuariosServicio.SaveUsuario(modelo);
                if (usuario_creado.UsuarioId > 0)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "No se pudo crear el usuario" });
            }
            catch (Exception ex)
            {
                // Registrar el error
                _logger.LogError(ex, "Ocurrió un error al registrar el usuario.");
                return StatusCode(500, "Ocurrió un error en el servidor.");
            }
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

