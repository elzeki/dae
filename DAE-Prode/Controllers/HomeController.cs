using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAE_Prode.Models;
using System.Web.Caching;

namespace DAE_Prode.Controllers
{
    public class HomeController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();
      
        public ActionResult Index()
        {
            Session["id"] = 5;
            Session["username"] = "zeki";
            Session["rol"] = "usuario";
            var countSession = Session.Count;
            if (countSession != 0)
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
            IEnumerable <topfivemodels> ltopfives = new topfivemodels().Vertopfive();
            ViewData["ltopfives"] = ltopfives;
            IEnumerable<toptenmodels> ltoptens = new toptenmodels().Vertopten();
            ViewData["ltoptens"] = ltoptens;
            IEnumerable<tablageneralmodels> ltablageneral = new tablageneralmodels().Vertablageneral();
            ViewData["ltablageneral"] = ltablageneral;

            return View();
        }
        public ActionResult Seleccionartorneo()
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;
            torneomodels T = new torneomodels();
            return View(T);
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult Seleccionartorneo(string accion, torneomodels T)
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;
            return RedirectToAction(accion, new { idt = T.id });
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Verfechasportorneo(int idt)
        {
            IEnumerable<fechamodels> lfechas = new fechamodels().listafechasportorneo(idt);
            ViewBag.nombretorneo = new torneomodels().nombretorneo(idt);
            ViewData["lfechas"] = lfechas;
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Reglamento()
        {
            return View();
        }
    }
}           