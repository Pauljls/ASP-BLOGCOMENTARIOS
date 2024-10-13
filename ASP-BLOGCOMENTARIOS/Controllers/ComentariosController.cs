using ASP_BLOGCOMENTARIOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ASP_BLOGCOMENTARIOS.Controllers
{
    public class ComentariosController : Controller
    {
        private readonly IConfiguration _configuration;

        public ComentariosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            using (SqlConnection con = new(_configuration["ConnectionStrings:dataconection"])) {
                using (SqlCommand cmd = new("sp_comentariosTotales",con)) {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new(cmd);
                    DataTable dt = new();
                    da.Fill(dt);
                    da.Dispose();
                    List<ComentarioUsuario> list = new();
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        list.Add(new ComentarioUsuario()
                        {
                            id = Convert.ToInt32(dt.Rows[i][0]),
                            contenido = Convert.ToString(dt.Rows[i][1]),
                            date = Convert.ToDateTime(dt.Rows[i][2]),
                            usuario = Convert.ToString(dt.Rows[i][3]) 
                        });
                    }
                    ViewBag.comentarios = list;
                    con.Close();
                    return View();
                }
            }     
        }

        public IActionResult comentar()
        {
            using (SqlConnection con = new(_configuration["ConnectionStrings:dataconection"]))
            {
                using (SqlCommand cmd = new("sp_usuarios", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    adapter.Dispose();
                    List<UsuarioModel> list = new();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        list.Add(new UsuarioModel
                        {
                            Id = Convert.ToInt32(dataTable.Rows[i][0]),
                            Name = Convert.ToString(dataTable.Rows[i][1]),
                            Apellido = Convert.ToString(dataTable.Rows[i][2]),
                            Edad = Convert.ToInt32(dataTable.Rows[i][3])

                        });
                    }

                    ViewBag.usuarios = new SelectList(list,"Id","Name");
                    con.Close();
                    return View();
                    //Console.WriteLine(dataTable.Rows.Count);
                }
            }
        }

        [HttpPost]
        public IActionResult comentar(ComentarioViewModel model)
        {
            try
            {
                using (SqlConnection con = new(_configuration["ConnectionStrings:dataconection"]))
                {
                    using (SqlCommand cmd = new("sp_comentar", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@contenido", SqlDbType.Text).Value = model.Comentario;
                        cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = model.IdUsuario;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine(ex.ToString());
                return View(ex.Message);
            }

        }
    }
}
