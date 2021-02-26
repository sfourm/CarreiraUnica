using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioUnica.ViewModel
{
    public class EditarSenha
    {
        [Required]
        public string Token { get; set; }

        public int UsuarioId { get; set; }

        [Display(Name = "Senha", Description = "Digite sua senha")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "A senha deve ter de 8 a 20 dígitos")]
        [Required(ErrorMessage = "Preencha este campo")]
        public string Senha { get; set; }

        [Display(Name = "Confirmar senha", Description = "Confirme sua senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Preencha este campo")]
        public string ConfirmarSenha { get; set; }
    }
}
