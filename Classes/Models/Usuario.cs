using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Classes.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        /// Verificação da força da senha.
        /// <returns>1 a 5 sendo 1 inaceitavel e 5 senha segura</returns>
        public int VerifyPasswordStrong(string password) 
        {
            int totalPoint = 0;
            totalPoint += GetPointsBySize(password.Length);
            totalPoint += GetPointBySmallLetters(password);
            totalPoint += GetPointByCapsLockLetters(password);
            totalPoint += GetPointbyNumber(password);
            totalPoint += GetPointBySymbol(password);
            totalPoint += GetPointByRepeat(password);

            //Níveis de 1 a 5 (1 = inaceitavel | 5 = segura)
            if (totalPoint < 50)
                return 1;
            else if (totalPoint < 60)
                return 2;
            else if (totalPoint < 80)
                return 3;
            else if (totalPoint < 100)
                return 4;
            else
                return 5;
        }

        /// Calcular pontuação da password de acordo ao tamanho.
        /// Seis pontos serão atribuídos para cada caractere na password, até um máximo de sessenta pontos.
        /// <returns>de 0 a 60 pontos</returns>
        private int GetPointsBySize(int passwordLength)
        {
            return Math.Min(10, passwordLength) * 6;
        }

        /// Calcular pontuação de acordo a quantidade de letras minusculas
        /// Cinco pontos serão concedidos se a password inclui uma letra minúscula. Dez pontos serão atribuídos se mais de uma letra minúscula estiver presente.
        /// <returns>de 0 a 10 pontos</returns>
        private int GetPointBySmallLetters(string password)
        {
            int points = password.Length - Regex.Replace(password, "[a-z]", "").Length;
            return Math.Min(2, points) * 5;
        }

        /// Calcular pontuação de acordo a quantidade de letras maiúsculas.
        /// Cinco pontos serão concedidos se a password incluir uma letra maiúscula. Dez pontos serão atribuídos se mais de uma letra maiúscula estiver presente.
        /// <returns>de 0 a 10 pontos</returns>
        private int GetPointByCapsLockLetters(string password)
        {
            int rawplacar = password.Length - Regex.Replace(password, "[A-Z]", "").Length;
            return Math.Min(2, rawplacar) * 5;
        }

        /// Calcular pontuação de acordo a quantidade de numeros.
        /// Cinco pontos serão concedidos se a password incluir um dígito numérico. Dez pontos serão atribuídos se mais de um dígito numérico estiver presente.
        /// <returns>de 0 a 10 pontos</returns>
        private int GetPointbyNumber(string password)
        {
            int rawplacar = password.Length - Regex.Replace(password, "[0-9]", "").Length;
            return Math.Min(2, rawplacar) * 5;
        }

        /// Calcular pontuação de acordo a quantidade de simbolos.
        /// <returns>de 0 a 10 pontos</returns>
        private int GetPointBySymbol(string password)
        {
            int rawplacar = Regex.Replace(password, "[a-zA-Z0-9]", "").Length;
            return Math.Min(2, rawplacar) * 5;
        }

        /// Calcula a pontuação de acordo a quantidade de caracteres repetidos.
        /// <returns>0 ou 30 pontos</returns>
        private int GetPointByRepeat(string password)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\w)*.*\1");
            bool repeat = regex.IsMatch(password);
            if (repeat)
            {
                return 30;
            }
            else
            {
                return 0;
            }
        }
    }
}
