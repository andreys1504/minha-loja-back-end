using Flunt.Validations;

namespace MinhaLoja
{
    public static class FluntExtensions
    {
        public static Contract<T> IsCnpj<T>(
            this Contract<T> contract,
            string val,
            string key)
        {
            if (string.IsNullOrWhiteSpace(val) == false)
                if (IsCnpj(val) == false)
                    contract.AddNotification(key, "CNPJ inválido");

            return contract;
        }

        public static Contract<T> IsCnpj<T>(
            this Contract<T> contract,
            string val,
            string key,
            string message)
        {
            if (string.IsNullOrWhiteSpace(val) == false)
                if (IsCnpj(val) == false)
                    contract.AddNotification(key, message);

            return contract;
        }

        private static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.GetNumbers();

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}
