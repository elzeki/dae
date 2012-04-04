using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication6.Models;

namespace MvcApplication6.Controllers
{
    public class HomeController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();
        
        public ActionResult Index()
        {   
             Session["id"]=5;
             Session["username"]="zeki";
             Session["rol"] = "usuario";           


            if (Session.Count != 0)
            {   
                if (Session["rol"].ToString() == "usuario")
                {
                    String idUsuario = Session["id"].ToString();
                    usuario U = new usuario();
                    U = db.usuarios.Single(q => q.id.Equals(idUsuario));

                    ViewBag.username = U.username;
                    ViewBag.nombre = U.nombre;
                    ViewBag.apellido = U.apellido;
                    ViewBag.email = U.email;
                    ViewBag.fechanac = U.fechanac;
                }
                else {                   
                    Session.RemoveAll();
                }
            }
            return View();
        }

        public ActionResult Reglamento()
        {
            return View();
        }
    }
}           