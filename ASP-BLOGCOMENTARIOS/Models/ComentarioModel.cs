namespace ASP_BLOGCOMENTARIOS.Models
{
    public class ComentarioModel
    {
        public int id { get; set; }
        public string contenido { get; set; }
        public DateTime date { get; set; }
        public int idUsuario { get; set; }
    }
}
