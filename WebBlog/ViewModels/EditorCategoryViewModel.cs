using System.ComponentModel.DataAnnotations;

namespace WebBlog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage ="O nome é obrigatório")]
        [StringLength(40, MinimumLength =3,ErrorMessage ="Este campo deve conter entre 3 e 40 caracteres")]
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
