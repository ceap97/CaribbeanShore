using Xunit;
using Assert = Xunit.Assert;

namespace TestProject1
{
    public class PermisosTests
    {
        [Fact]
        public void TienePermiso_AdminConAccesoTotal_DeberiaRetornarTrue()
        {
            // Arrange
            var permisos = new Permisos();
            var usuario = "admin";
            var permiso = "acceso_total";

            // Act
            var resultado = permisos.TienePermiso(usuario, permiso);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void TienePermiso_UsuarioNormalSinAcceso_DeberiaRetornarFalse()
        {
            // Arrange
            var permisos = new Permisos();
            var usuario = "usuario_normal";
            var permiso = "acceso_total";

            // Act
            var resultado = permisos.TienePermiso(usuario, permiso);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void ContarPermisos_Admin_DeberiaRetornarCinco()
        {
            // Arrange
            var permisos = new Permisos();
            var usuario = "admin";

            // Act
            var resultado = permisos.ContarPermisos(usuario);

            // Assert
            Assert.Equal(5, resultado);
        }

        [Fact]
        public void ContarPermisos_UsuarioNormal_DeberiaRetornarUno()
        {
            // Arrange
            var permisos = new Permisos();
            var usuario = "usuario_normal";

            // Act
            var resultado = permisos.ContarPermisos(usuario);

            // Assert
            Assert.Equal(1, resultado);
        }
    }
}
