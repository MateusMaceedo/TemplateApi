using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateApi.Application.Interfaces;
using TemplateApi.Domain.Utils;

namespace TemplateApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : Controller
    {
        private readonly IRealizarConsultaPorCepUseCase _realizarConsultaPorCepUseCase;

        public ConsultaController(IRealizarConsultaPorCepUseCase realizarConsultaPorCepUseCase)
        {
            _realizarConsultaPorCepUseCase = realizarConsultaPorCepUseCase;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult> GetEnderecoAsync(string cep)
        {
            // Verifica se o CEP é válido
            if (!CepUtils.IsValidCep(cep))
            {
                return NotFound();
            }

            var result = await _realizarConsultaPorCepUseCase.ObterEnderecoPorCep(cep);

            return Ok(result);
        }
    }
}