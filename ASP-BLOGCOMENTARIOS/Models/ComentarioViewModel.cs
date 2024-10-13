using System.ComponentModel.DataAnnotations;

namespace ASP_BLOGCOMENTARIOS.Models
{
    public class ComentarioViewModel
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string Comentario { get; set; }
    }
}
