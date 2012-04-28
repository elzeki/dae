using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    public class AdminController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();

        /*----------------------------------------------------------------------------------------*/
        // GET: /Admin/

        public ActionResult Index()
        {
            if (Session.Count != 0)
            {   // El rol Admin tiene Id = 1
                if (Session["rol"].ToString() == "admin")
                {
                    return RedirectToAction("Home", "Admin");
                }
                else
                {// El rol Operador tiene Id = 2
                    if (Session["rol"].ToString() == "operador")
                    {
                        return RedirectToAction("HomeOperador", "Admin");
                    }
                    else
                    { // Esta logueado y es un usuario
                        return RedirectToAction("LogOff", "Admin");
                    }
                }
            }
            else
            {
                return RedirectToAction("LogOn", "Admin");
            }
        }
        /*----------------------------------------------------------------------------------------*/
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Admin");
        }
        /*----------------------------------------------------------------------------------------*/
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            if (Session.Count != 0)
            {
                if (Session["rol"].ToString() == "admin")
                {
                    String mensaje = "Bienvenido " + Session["nombre"].ToString() + "!";
                    @ViewBag.Message = mensaje;
                    // Codigo del Home del Admin
                    return RedirectToAction("Home", "Admin");
                }
            }
            ViewBag.Error = TempData["Error"];
            return View();
        }
        /*----------------------------------------------------------------------------------------*/
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid) {
                if (ValidateUser(model.UserName, model.Password))
                {
                    return RedirectToAction("Home", "Admin");
                }
                else
                {
                    TempData["Error"] = "The user name or password provided is incorrect.";
                }
            }
            return RedirectToAction("Index", "Admin");
        }
        /*----------------------------------------------------------------------------------------*/
        public Boolean ValidateUser(String username, String password)
        {
            try
            {
                var existeusuario = db.usuarios.Single(u => u.username == username);
                if ((existeusuario.password == password) && ((existeusuario.idrol == 1) || (existeusuario.idrol == 2)))
                {
                    Session.Add("username", existeusuario.username);
                    Session.Add("id", existeusuario.id);
                    Session.Add("nombre", existeusuario.nombre);                   
                    if (existeusuario.idrol == 1)
                    {
                        Session.Add("rol", "admin");
                    } else {
                        Session.Add("rol", "operador");
                    }
                    
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /*----------------------------------------------------------------------------------------*/
        public ActionResult Home()
        {
            if (Session.Count != 0)
            {   
                if (Session["rol"].ToString() == "admin")
                {
                    String mensaje = "Bienvenido " + Session["nombre"].ToString() + "!";
                    @ViewBag.Message = mensaje;
                    // Codigo del Home del Admin
                    return View();
                }
            }
            return RedirectToAction("Index", "Admin");
        }
        /*----------------------------------------------------------------------------------------*/
        public ActionResult HomeOperador()
        {
            if (Session.Count != 0)
            {   
                if (Session["rol"].ToString() == "operador")
                {
                    String mensaje = "Bienvenido " + Session["nombre"].ToString() + "!";
                    @ViewBag.Message = mensaje;
                    // Codigo del Home del Operador
                    return View();
                }
            }
            return RedirectToAction("Index", "Admin");
        }


    }
}
