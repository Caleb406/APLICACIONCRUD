using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using AplicacionCRUD.Models;

namespace AplicacionCRUD.Controllers
{
    public class ContactoController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();

        private static List<Contacto> olista = new List<Contacto>();
        // GET: Contacto
        public ActionResult Inicio()
        {
            olista = new List<Contacto>();
            using(SqlConnection oconexion = new SqlConnection(conexion)) 
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CONTACTO", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                using (SqlDataReader dr  = cmd.ExecuteReader()) 
                {
                    while (dr.Read()) 
                    {
                        Contacto nuevocontacto = new Contacto();

                        nuevocontacto.idContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevocontacto.Nombres = dr["Nombres"].ToString();
                        nuevocontacto.Apellidos = dr["Apellidos"].ToString();
                        nuevocontacto.Telefono = dr["Telefono"].ToString();
                        nuevocontacto.Correo = dr["Correo"].ToString();

                        olista.Add(nuevocontacto);
                    }
                }
            }
            return View(olista);
        }

        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }


        [HttpPost]

        public ActionResult Registrar(Contacto ocontacto) 
        {

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Registrar", oconexion);
                cmd.Parameters.AddWithValue("Nombres", ocontacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", ocontacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio","Contacto");
            
        }

        [HttpGet]
        public ActionResult Editar(int? idcontacto)
        {
            if (idcontacto == null)
                return RedirectToAction("Inicio", "Contacto");

            Contacto contacto = olista.Where(c => c.idContacto == idcontacto).FirstOrDefault();
            if (contacto == null)
                return HttpNotFound(); 

            return View(contacto); 
        }

        [HttpPost]
        public ActionResult Editar(Contacto ocontacto) 
        {
            
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", ocontacto.idContacto);
                cmd.Parameters.AddWithValue("Nombres", ocontacto.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", ocontacto.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", ocontacto.Telefono);
                cmd.Parameters.AddWithValue("Correo", ocontacto.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Contacto");
        }


        [HttpGet]
        public ActionResult Eliminar(int? idcontacto)
        {
            if (idcontacto == null)
                return RedirectToAction("Inicio", "Contacto");

            Contacto ocontacto = olista.Where(c => c.idContacto == idcontacto).FirstOrDefault();

            return View(ocontacto);
        }

        [HttpPost]
        public ActionResult Eliminar(string IdContacto)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", IdContacto);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                cmd.ExecuteNonQuery();

            }

            return RedirectToAction("Inicio", "Contacto");

        }

    }
}