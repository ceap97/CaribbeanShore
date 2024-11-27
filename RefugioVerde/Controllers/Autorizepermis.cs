

using RefugioVerde.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class AuthorizePermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _permission;

    public AuthorizePermissionAttribute(string permission)
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = int.Parse(userIdClaim);
        var dbContext = context.HttpContext.RequestServices.GetService(typeof(RefugioVerdeContext)) as RefugioVerdeContext;
        var empleado = dbContext.Empleados
            .Include(u => u.Rol)
            .ThenInclude(r => r.Permisos)
            .FirstOrDefault(u => u.EmpleadoId == userId);

        if (empleado == null || !empleado.Rol.Permisos.Any(p => p.Nombre == _permission))
        {
            context.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/AccessDenied.cshtml",
                StatusCode = 403
            };
        }
    }
}
