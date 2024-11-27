using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RefugioVerde.Controllers;
using RefugioVerde.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace RefugioVerde.Tests
{
    public class ReservasControllerTests
    {
        private Mock<RefugioVerdeContext> CreateMockContext()
        {
            var options = new DbContextOptionsBuilder<RefugioVerdeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var mockContext = new Mock<RefugioVerdeContext>(options);
            mockContext.Setup(m => m.Reservas.Add(It.IsAny<Reserva>())).Verifiable();
            mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            return mockContext;
        }

        [Fact]
        public async Task Crear_ReturnsOkResult_WhenModelStateIsValid()
        {
            // Arrange
            var mockContext = CreateMockContext();
            var controller = new ReservasController(mockContext.Object);
            var reserva = new Reserva
            {
                ReservaId = 1,
                FechaReserva = DateTime.Now,
                FechaInicio = DateTime.Now.AddDays(1),
                FechaFin = DateTime.Now.AddDays(2),
                ClienteId = 1,
                HabitacionId = 1,
                EstadoReservaId = 1,
                ServicioId = 1,
                ComodidadId = 1
            };

            // Act
            var result = await controller.Crear(reserva);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Crear_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockContext = CreateMockContext();
            var controller = new ReservasController(mockContext.Object);
            controller.ModelState.AddModelError("Error", "Model state is invalid");
            var reserva = new Reserva
            {
                ReservaId = 1,
                FechaReserva = DateTime.Now,
                FechaInicio = DateTime.Now.AddDays(1),
                FechaFin = DateTime.Now.AddDays(2),
                ClienteId = 1,
                HabitacionId = 1,
                EstadoReservaId = 1,
                ServicioId = 1,
                ComodidadId = 1
            };

            // Act
            var result = await controller.Crear(reserva);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}