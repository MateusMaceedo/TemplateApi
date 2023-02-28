using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TemplateApi.Application.DTOs;
using TemplateApi.Application.Interfaces;
using TemplateApi.Controllers;

namespace TemplateApi.TestsUnit.Controllers
{
    [TestClass]
    public class ConsultaControllerTests
    {
        [TestMethod]
        public async Task GetEnderecoAsync_DeveRetornarEndereco_QuandoCepValido()
        {
            // Arrange
            var cep = "12345678";
            var endereco = new ConsultaCepDTO
            {
                Logradouro = "Rua Exemplo",
                Bairro = "Bairro Exemplo",
                Uf = "Cidade Exemplo",
                Localidade = "UF"
            };

            var mockRealizarConsultaPorCepUseCase = new Mock<IRealizarConsultaPorCepUseCase>();
            mockRealizarConsultaPorCepUseCase.Setup(x => x.ObterEnderecoPorCep(cep));

            var controller = new ConsultaController(mockRealizarConsultaPorCepUseCase.Object);

            // Act
            var actionResult = await controller.GetEnderecoAsync(cep);

            var result = new OkObjectResult(actionResult)
            {
                 StatusCode = 200
            };

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            var okResult = actionResult as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetEnderecoAsync_DeveRetornarNotFound_QuandoCepInvalido()
        {
            // Arrange
            var cep = "12345";

            var mockRealizarConsultaPorCepUseCase = new Mock<IRealizarConsultaPorCepUseCase>();
            mockRealizarConsultaPorCepUseCase.Setup(x => x.ObterEnderecoPorCep(cep));

            var controller = new ConsultaController(mockRealizarConsultaPorCepUseCase.Object);

            // Act
            var actionResult = await controller.GetEnderecoAsync(cep);

            // Assert
            Assert.IsNotNull(actionResult);

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }
    }
}
