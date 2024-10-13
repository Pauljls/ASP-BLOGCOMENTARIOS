using ASP_BLOGCOMENTARIOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace ASP_BLOGCOMENTARIOS.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsuariosController(IConfiguration configuration) { 
            _configuration = configuration;
        }

    public IActionResult Index()
        {
          try
            {
                using (SqlConnection con = new(_configuration["ConnectionStrings:dataconection"]))
                {
                    using (SqlCommand cmd = new("sp_usuarios",con))
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
                        
                        ViewBag.usuarios = list;
                        con.Close();
                        return View();
                        //Console.WriteLine(dataTable.Rows.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public IActionResult registrar() {
            return View();
        }

        [HttpPost]
        public IActionResult registrar(UsuarioModel usuario) {

            try
            {
                using (SqlConnection con = new(_configuration["ConnectionStrings:dataconection"]))
                {
                    using (SqlCommand cmd = new("sp_registrar", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", SqlDbType.Text).Value = usuario.Name;
                        cmd.Parameters.Add("@Apellido", SqlDbType.Text).Value = usuario.Apellido;
                        cmd.Parameters.Add("@Edad", SqlDbType.Int).Value = usuario.Edad;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
