using Refit;
using System.Threading.Tasks;
using TemplateApi.Application.DTOs;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Application.UseCases.Externals
{
    public interface IViaCepAPI
    {
        [Get("/ws/{cep}/json/")]
        Task<EnderecoEntity> GetEnderecoAsync(string cep);

        [Get("/ws/{id}/json/")]
        Task<ConsultaCepDTO> ObterEnderecoPorId(string id);
    }
}
