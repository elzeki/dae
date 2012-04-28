using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    [Login(rol = "admin")]
    public class TorneosController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();

        /* --------------------------------------------------------------------------------------  */
        public ActionResult Index()
        {
            var listatorneos = new torneomodels().listatorneos();
            return View(listatorneos);
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Ver(int id)
        {
            var torneo = new torneomodels().vertorneo(id).First();
            @ViewBag.id     = torneo.id;
            @ViewBag.nombre = torneo.nombre;
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult Agregar(torneomodels model)
        {
            if (ModelState.IsValid)
            {
                torneo T = new torneo
                {
                    nombre = model.nombre
                };

                db.torneos.InsertOnSubmit(T);
                try
                {
                    db.SubmitChanges();
                }
                catch 
                {
                    TempData["ERROR"] = "No se pudo agregar el torneo, por favor intente nuevamente.";
                    return View();
                }
                TempData["MENSAJE"] = "Se agrego correctamente el torneo.";
                return RedirectToAction("Index");
            }
            TempData["ERROR"] = "Error en el alta del torneo.";
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Agregar()
        {
            return View();
        }
        /* --------------------------------------------------------------------------------------  */

        public ActionResult Editar(int id)
        {
            torneo T = new torneo();
            try
            {
                T = db.torneos.Single(q => q.id == id);
                torneomodels tmodel = new torneomodels();
                tmodel.id       = T.id;
                tmodel.nombre   = T.nombre;
                return View(tmodel);
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar el torneo, por favor intente nuevamente.";
                return RedirectToAction("Index");
            }
        }
        /* --------------------------------------------------------------------------------------  */

        [HttpPost]
        public ActionResult Editar(int id, torneomodels model)
        {
            try
            {
                if (this.SaveTorneo(model))
                {
                    TempData["MENSAJE"] = "Se edito correctamente el torneo.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ERROR"] = "No se pudo editar el torneo, por favor intente nuevamente.";
                    return View();
                }
            }
            catch 
            {
                TempData["ERROR"] = "No se pudo editar el torneo, por favor intente nuevamente.";
                return View();
            }
        }
        /* --------------------------------------------------------------------------------------  */
        public bool SaveTorneo(torneomodels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    torneo T = new torneo();
                    T = db.torneos.Single(q => q.id == model.id);
                    T.nombre = model.nombre;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Eliminar(int id)
        {
            var torneo1 = new torneomodels().vertorneo(id).First();
            torneo T = new torneo
            {
                id = torneo1.id,
                nombre = torneo1.nombre
            };
            try
            {
                db.torneos.Attach(T);
                db.torneos.DeleteOnSubmit(T);
                db.SubmitChanges();
                TempData["MENSAJE"] = "Se elimino correctamente el torneo.";
            }
            catch 
            {
                TempData["ERROR"] = "No se pudo eliminar el torneo, por favor intente nuevamente.";
            }
            return RedirectToAction("Index");
        }
        /* --------------------------------------------------------------------------------------  */
    }
}
