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
        private readonly Mock<IConsumptionRepository> _consumptionRepository;
        private readonly ConsumptionController _consumption;

        public ConsumptionControllerTests()
        {
            _consumptionRepository = new Mock<IConsumptionRepository>();
            _consumption = new ConsumptionController(_consumptionRepository.Object);
        }

        [Fact]
        public async Task GetAllConsumptions_ShouldReturnOk()
        {
            var consumptions = new List<Consumptiondomain>
            {
                new Consumptiondomain { Id = "001", Consumption = 100.5 },
                new Consumptiondomain { Id = "002", Consumption = 200.0 }
            };
            _consumptionRepository.Setup(repo => repo.ListAll()).ReturnsAsync(consumptions);

            var result = await _consumption.GetAllConsumptions();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Consumptiondomain>>(okResult.Value);
            Assert.Equal(consumptions.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetConsumptionById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            _consumptionRepository.Setup(repo => repo.GetById(It.IsAny<string>())).ReturnsAsync((Consumptiondomain)null);

            var result = await _consumption.GetConsumptionById("999"); 

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetConsumptionById_ShouldReturnOk_WhenRecordExists()
        {
            var consumption = new Consumptiondomain { Id = "001", Consumption = 100.5 };
            _consumptionRepository.Setup(repo => repo.GetById("001")).ReturnsAsync(consumption);

            var result = await _consumption.GetConsumptionById("001");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Consumptiondomain>(okResult.Value);
            Assert.Equal(consumption.Consumption, returnValue.Consumption);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            var invalidConsumption = new Consumptiondomain { Consumption = -50.0 };

            var result = await _consumption.AddConsumption(invalidConsumption);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid data", ((dynamic)badRequestResult.Value).message);
        }

        [Fact]
        public async Task Post_ShouldReturnCreatedAtAction_WhenDataIsValid()
        {
            var validConsumption = new Consumptiondomain { Id = "003", Consumption = 100.5 };
            _consumptionRepository.Setup(repo => repo.Save(validConsumption)).Returns(Task.CompletedTask);

            var result = await _consumption.AddConsumption(validConsumption);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Consumptiondomain>(createdResult.Value);
            Assert.Equal(validConsumption.Consumption, returnValue.Consumption);
        }
    }
}
