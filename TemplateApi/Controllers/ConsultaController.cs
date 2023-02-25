using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Refit;
using System.Threading.Tasks;
using TemplateApi.Application.Mappers;
using TemplateApi.Application.Models;
using TemplateApi.Application.UseCases.Externals;
using TemplateApi.Domain.Interfaces;

namespace TemplateApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : Controller
    {
        private readonly IViaCepAPI _realizarConsultaCepUseCase;
        private readonly IRedisRepository<object> _redisRepository;

        public ConsultaController(
            IRedisRepository<object> redisRepository)
        {
            _realizarConsultaCepUseCase = RestService.For<IViaCepAPI>("https://viacep.com.br");
            _redisRepository = redisRepository;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult<CepModel>> GetEnderecoAsync(string cep)
        {
            try
            {
                var cacheKey = $"Endereco:{cep}";

                var enderecoJson = _redisRepository.Get(cacheKey);

                if (!string.IsNullOrEmpty(enderecoJson.ToString()))
                {
                    var cepModel = JsonConvert.DeserializeObject<CepModel>(enderecoJson.ToString());
                    return Ok(cepModel);
                }

                var endereco = await _realizarConsultaCepUseCase.GetEnderecoAsync(cep);

                if (endereco == null)
                {
                    return NotFound();
                }

                var enderecoDto = EntityToModelProfile.Map(endereco);
                enderecoJson = JsonConvert.SerializeObject(enderecoDto);

                _redisRepository.Add(cacheKey, enderecoJson);

                return Ok(enderecoDto);
            }
            catch (ApiException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Content);
            }
        }
    }
}
