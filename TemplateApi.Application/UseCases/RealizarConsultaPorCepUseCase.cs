using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TemplateApi.Application.Interfaces;
using TemplateApi.Application.Mappers;
using TemplateApi.Application.UseCases.Externals;
using TemplateApi.Domain.Entities;
using TemplateApi.Domain.Interfaces;

namespace TemplateApi.Application.UseCases
{
    public class RealizarConsultaPorCepUseCase : IRealizarConsultaPorCepUseCase
    {
        private readonly IViaCepAPI _viaCepApi;
        private readonly IRedisRepository<object> _redisRepository;

        public RealizarConsultaPorCepUseCase(
            HttpClient httpClient, 
            IConfiguration configuration, 
            IRedisRepository<object> redisRepository)
        {
            var viaCepUrl = configuration.GetSection("ConnectionStrings:APIViaCEP").Value;
            httpClient.BaseAddress = new Uri(viaCepUrl);
            _viaCepApi = RestService.For<IViaCepAPI>(httpClient);
            _redisRepository = redisRepository;
        }

        public async Task<EnderecoEntity> ObterEnderecoPorCep(string cep)
        {
            try
            {
                var cacheKey = $"Endereco:{cep}";

                var enderecoJson = _redisRepository.Get(cacheKey);

                var endereco = await _viaCepApi.GetEnderecoAsync(cep);

                if (endereco == null)
                {
                    return null;
                }

                var enderecoDto = EntityToModelProfile.Map(endereco);
                enderecoJson = JsonConvert.SerializeObject(enderecoDto);

                _redisRepository.Add(cacheKey, enderecoJson);

                var enderecoCep = await _viaCepApi.GetEnderecoAsync(cep);
                
                return endereco;
            }
            catch (ApiException ex)
            {
                throw new Exception($"Erro ao realizar a operação, msg de erro: {ex.Message}");
            }
        }
    }
}
