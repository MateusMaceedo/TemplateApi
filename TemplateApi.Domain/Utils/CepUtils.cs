namespace TemplateApi.Domain.Utils
{
    public static class CepUtils
    {
        public static bool IsValidCep(string cep)
        {
            // Verifica se o CEP possui 8 dígitos
            if (cep.Length != 8)
            {
                return false;
            }

            // Verifica se o CEP contém apenas dígitos
            foreach (char c in cep)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
