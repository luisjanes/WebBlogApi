using System.ComponentModel.DataAnnotations;

namespace WebBlog.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="O nome é obrigatório")]
        [MaxLength(40, ErrorMessage ="O nome é longo demais")]
        [MinLength(3, ErrorMessage ="O nome é curto demais")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage ="O e-mail é inválido")]
        public string Email { get; set; }
    }
}
