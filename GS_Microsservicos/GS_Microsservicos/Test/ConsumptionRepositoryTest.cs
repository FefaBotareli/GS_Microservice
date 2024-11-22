using Moq;
using GS_Microsservicos.Models;
using GS_Microsservicos.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GS_Microsservicos.Tests
{
    public class ConsumptionRepositoryTests
    {
        [Fact]
        public async Task ListAll_ShouldReturnAllConsumptions()
        {
            // Arrange
            var consumptions = new List<Consumptiondomain>
            {
                new Consumptiondomain { Id = "1", Consumption = 100.5 },
                new Consumptiondomain { Id = "2", Consumption = 200.0 }
            };

            var consumptionRepositoryMock = new Mock<IConsumptionRepository>();
            consumptionRepositoryMock.Setup(repo => repo.ListAll()).ReturnsAsync(consumptions);

            var consumptionRepository = consumptionRepositoryMock.Object;

           
            var result = await consumptionRepository.ListAll();

            Assert.Equal(consumptions, result);
        }

        [Fact]
        public async Task Save_ShouldCallSaveOnce()
        {
            var consumption = new Consumptiondomain
            {
                Id = "1",
                Consumption = 100.5
            };

            var consumptionRepositoryMock = new Mock<IConsumptionRepository>();
            consumptionRepositoryMock.Setup(repo => repo.Save(It.IsAny<Consumptiondomain>())).Returns(Task.CompletedTask);

            var consumptionRepository = consumptionRepositoryMock.Object;

           
            await consumptionRepository.Save(consumption);

            consumptionRepositoryMock.Verify(repo => repo.Save(It.IsAny<Consumptiondomain>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnConsumption_WhenIdExists()
        {
            var consumption = new Consumptiondomain
            {
                Id = "1",
                Consumption = 100.5
            };

            var consumptionRepositoryMock = new Mock<IConsumptionRepository>();
            consumptionRepositoryMock.Setup(repo => repo.GetById("1")).ReturnsAsync(consumption);

            var consumptionRepository = consumptionRepositoryMock.Object;

            var result = await consumptionRepository.GetById("1");

            Assert.Equal(consumption, result);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            var consumptionRepositoryMock = new Mock<IConsumptionRepository>();
            consumptionRepositoryMock.Setup(repo => repo.GetById("nonexistentId")).ReturnsAsync((Consumptiondomain)null);

            var consumptionRepository = consumptionRepositoryMock.Object;

            var result = await consumptionRepository.GetById("nonexistentId");

            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_ShouldCallDeleteOnce()
        {
            var id = "1";

            var consumptionRepositoryMock = new Mock<IConsumptionRepository>();
            consumptionRepositoryMock.Setup(repo => repo.Delete(It.IsAny<string>())).Returns(Task.CompletedTask);

            var consumptionRepository = consumptionRepositoryMock.Object;

            await consumptionRepository.Delete(id);
            
            consumptionRepositoryMock.Verify(repo => repo.Delete(It.IsAny<string>()), Times.Once);
        }
    }
}
