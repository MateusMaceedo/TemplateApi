using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TemplateApi.Application.Interfaces;

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
            var result = await _realizarConsultaPorCepUseCase.ObterEnderecoPorCep(cep);

            if (cep is null || !string.IsNullOrEmpty(cep))
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
