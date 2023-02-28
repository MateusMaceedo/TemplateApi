using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TemplateApi.Application.DTOs;
using TemplateApi.Application.UseCases;
using TemplateApi.Application.UseCases.Externals;
using TemplateApi.Domain.Interfaces;

namespace TemplateApi.TestsUnit.UseCases
{
    [TestClass]
    public class RealizarConsultaPorCepUseCaseTests
    {
        private Mock<IRedisRepository<object>> _mockRedisRepository;
        private Mock<IViaCepAPI> _mockViaCepApi;
        private RealizarConsultaPorCepUseCase _useCase;

        [TestInitialize]
        public void Initialize()
        {
            _mockRedisRepository = new Mock<IRedisRepository<object>>();
            _mockViaCepApi = new Mock<IViaCepAPI>();
            _useCase = new RealizarConsultaPorCepUseCase(
                new HttpClient(new MockHttpMessageHandler()),
                new ConfigurationBuilder().Build(),
                _mockRedisRepository.Object);
        }

        [TestMethod]
        public async Task ObterEnderecoPorCep_DeveRetornarEndereco()
        {
            // Arrange
            var cep = "01001-000";
            var endereco = new ConsultaCepDTO
            {
                Cep = cep,
                Logradouro = "Praça da Sé",
                Bairro = "Sé",
                Localidade = "São Paulo",
                Uf = "SP"
            };
            _mockRedisRepository.Setup(x => x.GetAsync($"Endereco:{cep}")).ReturnsAsync((string)null);
            _mockViaCepApi.Setup(x => x.GetEnderecoAsync(cep));

            // Act
            var result = await _useCase.ObterEnderecoPorCep(cep);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cep, result.Cep);
            Assert.AreEqual(endereco.Logradouro, result.Logradouro);
            Assert.AreEqual(endereco.Bairro, result.Bairro);
            Assert.AreEqual(endereco.Localidade, result.Bairro);
            Assert.AreEqual(endereco.Uf, result.Uf);
        }

        [TestMethod]
        public async Task ObterEnderecoPorCep_DeveRetornarEnderecoDoCache()
        {
            // Arrange
            var cep = "01001-000";
            var enderecoJson = JsonConvert.SerializeObject(new ConsultaCepDTO
            {
                Cep = cep,
                Logradouro = "Praça da Sé",
                Bairro = "Sé",
                Localidade = "São Paulo",
                Uf = "SP"
            });
            _mockRedisRepository.Setup(x => x.GetAsync($"Endereco:{cep}")).ReturnsAsync(enderecoJson);

            // Act
            var result = await _useCase.ObterEnderecoPorCep(cep);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cep, result.Cep);
            Assert.AreEqual("Praça da Sé", result.Logradouro);
            Assert.AreEqual("Sé", result.Bairro);
            Assert.AreEqual("São Paulo", result.Bairro);
            Assert.AreEqual("SP", result.Uf);
            _mockViaCepApi.Verify(x => x.GetEnderecoAsync(It.IsAny<string>()), Times.Never);
            _mockRedisRepository.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        }
    }
}
