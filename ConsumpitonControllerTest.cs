using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GS_Microsservicos.Controllers;
using GS_Microsservicos.Models;
using GS_Microsservicos.Repositories;
using Moq;
using Xunit;

namespace GS_Microsservicos.Tests
{
    public class ConsumptionControllerTests
    {
        private readonly Mock<IConsumptionRepository> _consumptionRepositoryMock;
        private readonly ConsumptionController _controller;

        public ConsumptionControllerTests()
        {
            _consumptionRepositoryMock = new Mock<IConsumptionRepository>();
            _controller = new ConsumptionController(_consumptionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllConsumptionsShouldReturnOk()
        {
            // Arrange
            var consumptions = new List<Consumptiondomain>
            {
                new Consumptiondomain { Id = "001", Consumption = 100.5 },
                new Consumptiondomain { Id = "002", Consumption = 200.0 }
            };
            _consumptionRepositoryMock.Setup(repo => repo.ListAll()).ReturnsAsync(consumptions);

            // Act
            var result = await _controller.GetAllConsumptions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Consumptiondomain>>(okResult.Value);
            Assert.Equal(consumptions.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetConsumptionByIdShouldReturnNotFoundWhenRecordDoesNotExist()
        {
            // Arrange
            _consumptionRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>())).ReturnsAsync((Consumptiondomain)null);

            // Act
            var result = await _controller.GetConsumptionById("999");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetConsumptionByIdShouldReturnOkWhenRecordExists()
        {
            // Arrange
            var consumption = new Consumptiondomain { Id = "001", Consumption = 100.5 };
            _consumptionRepositoryMock.Setup(repo => repo.GetById("001")).ReturnsAsync(consumption);

            // Act
            var result = await _controller.GetConsumptionById("001");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Consumptiondomain>(okResult.Value);
            Assert.Equal(consumption.Consumption, returnValue.Consumption);
        }

        [Fact]
        public async Task PostShouldReturnBadRequestWhenDataIsInvalid()
        {
            // Arrange
            var invalidConsumption = new Consumptiondomain { Consumption = -50.0 };

            // Act
            var result = await _controller.AddConsumption(invalidConsumption);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid data", ((dynamic)badRequestResult.Value).message);
        }

        [Fact]
        public async Task PostShouldReturnCreatedAtActionWhenDataIsValid()
        {
            // Arrange
            var validConsumption = new Consumptiondomain { Id = "003", Consumption = 100.5 };
            _consumptionRepositoryMock.Setup(repo => repo.Save(validConsumption)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddConsumption(validConsumption);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Consumptiondomain>(createdResult.Value);
            Assert.Equal(validConsumption.Consumption, returnValue.Consumption);
        }
    }
}
