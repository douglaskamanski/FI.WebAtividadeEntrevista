using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FI.WebAtividadeEntrevista.Models.Validations
{
    public class CPFValidator : ValidationAttribute
    {
        public CPFValidator()
        {
        }

        public override bool IsValid(object value)
        {
            var cpf = String.Join("", System.Text.RegularExpressions.Regex.Split(value.ToString(), @"[^\d]"));
            
            var sumFirst9 = MultiplyAndSumNumbers(cpf.Substring(0, cpf.Length - 2));
            var firstDigit = CalculateDigit(sumFirst9);

            var sumFirst10 = MultiplyAndSumNumbers(cpf.Substring(0, cpf.Length - 1));
            var secondDigit = CalculateDigit(sumFirst10);

            var digit = firstDigit.ToString() + secondDigit.ToString();
            var cpfDigit = cpf.Substring(cpf.Length - 2, 2);

            if (string.Equals(digit, cpfDigit))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static int MultiplyAndSumNumbers(string numbers)
        {
            var inverse = new string(numbers.Reverse().ToArray());
            var sum = 0;

            for (int i = 0; i < inverse.Length; i++)
            {
                sum += (int)Char.GetNumericValue(inverse[i]) * (i + 2);
            }

            return sum;
        }

        private static int CalculateDigit(int sum)
        {
            int resto = sum % 11;

            if (resto < 2)
            {
                return 0;
            }
            else
            {
                return 11 - resto;
            }
        }
    }
}   
