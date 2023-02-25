using System.Threading.Tasks;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Application.Interfaces
{
    public interface IRealizarConsultaPorCepUseCase
    {
        Task<EnderecoEntity> ObterEnderecoPorCep(string cep);
    }
}
