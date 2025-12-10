namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Utils
{
    public static class CpfValidator
    {
        public static bool IsValid(string? cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            var numbers = new string(cpf.Where(char.IsDigit).ToArray());

            if (numbers.Length != 11)
                return false;

            if (numbers.Distinct().Count() == 1)
                return false;

            bool ValidateDigit(int[] multipliers, int checkPosition)
            {
                var sum = 0;
                for (int i = 0; i < multipliers.Length; i++)
                    sum += (numbers[i] - '0') * multipliers[i];

                var remainder = sum % 11;
                var digit = remainder < 2 ? 0 : 11 - remainder;
                return digit == numbers[checkPosition] - '0';
            }

          
            if (!ValidateDigit(new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 }, 9))
                return false;

            if (!ValidateDigit(new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 }, 10))
                return false;

            return true;
        }
    }
}
