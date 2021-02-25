using System.ComponentModel.DataAnnotations;

namespace DesafioUnica.ViewModel
{
    public class RecuperarSenha
    {
        [Display(Name = "E-mail", Description = "E-mail de recuperação")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "E-mail inválido.")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "O e-mail deve de 5 a 60 dígitos")]
        [Required(ErrorMessage = "Preencha este campo")]
        public string Email { get; set; }
    }
}
