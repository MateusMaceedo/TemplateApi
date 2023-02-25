using TemplateApi.Application.Models;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Application.Mappers
{
    public class EntityToModelProfile
    {
        public static CepModel Map(EnderecoEntity cep)
        {
            if (cep == null)
            {
                return null;
            }

            return new CepModel
            {
                Id = cep.Id,
                Cep = cep.Cep,
                Logradouro = cep.Logradouro,
                Complemento = cep.Complemento,
                Bairro = cep.Bairro,
                Localidade = cep.Localidade,
                Uf = cep.Uf,
                Ibge = cep.Ibge,
                Gia = cep.Gia,
                Ddd = cep.Ddd,
                Siafi = cep.Siafi
            };
        }
    }
}
