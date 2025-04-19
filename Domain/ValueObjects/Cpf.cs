using System;
using System.Text.RegularExpressions;
using Flunt.Notifications;
using Flunt.Validations;

namespace Domain.ValueObjects
{
    public class Cpf : BaseValueObject
    {
        public string Numero { get; private set; }

        protected Cpf() { }

        public Cpf(string numero)
        {
            numero = ApenasNumeros(numero);

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsTrue(ValidarCpf(numero), Key, "CPF inválido"));

            if (IsValid)
                Numero = numero;
        }

        public override string ToString()
        {
            return Convert.ToUInt64(Numero).ToString(@"000\.000\.000\-00");
        }

        private static string ApenasNumeros(string texto)
        {
            return Regex.Replace(texto ?? string.Empty, "[^0-9]", "");
        }

        private static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = ApenasNumeros(cpf);

            if (cpf.Length != 11) return false;

            // Descarta CPFs com todos os dígitos iguais
            if (new string(cpf[0], cpf.Length) == cpf) return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;
            var cpfValidado = tempCpf + digito2;
            return cpf == cpfValidado;
        }
    }
}
