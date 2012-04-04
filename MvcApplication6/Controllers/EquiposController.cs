using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication6.Models;

namespace MvcApplication6.Controllers
{
    public class EquiposController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();

        /* --------------------------------------------------------------------------------------  */
        public ActionResult Index()
        {
            var listaequipos = new equipomodels().listaequipos();
            return View(listaequipos);
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Ver(int id)
        {
            var equipo = new equipomodels().verequipo(id).First();
            @ViewBag.id = equipo.id;
            @ViewBag.nombre = equipo.nombre;
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult Agregar(equipomodels model)
        {
            if (ModelState.IsValid)
            {
                equipo E = new equipo
                {
                    nombre = model.nombre
                };

                db.equipos.InsertOnSubmit(E);
                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    TempData["ERROR"] = "No se pudo agregar el equipo, por favor intente nuevamente.";
                    return View();
                }
                TempData["MENSAJE"] = "Se agrego correctamente el equipo.";
                return RedirectToAction("Index");
            }
            TempData["ERROR"] = "Error en el alta del equipo.";
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
            equipo T = new equipo();
            try
            {
                T = db.equipos.Single(q => q.id == id);
                equipomodels emodel = new equipomodels();
                emodel.id       = T.id;
                emodel.nombre   = T.nombre;
                return View(emodel);
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar el equipo, por favor intente nuevamente.";
                return RedirectToAction("Index");
            }
        }
        /* --------------------------------------------------------------------------------------  */

        [HttpPost]
        public ActionResult Editar(int id, equipomodels model)
        {
            try
            {
                if (this.SaveEquipo(model))
                {
                    TempData["MENSAJE"] = "Se edito correctamente el equipo.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ERROR"] = "No se pudo editar el equipo, por favor intente nuevamente.";
                    return View();
                }
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar el equipo, por favor intente nuevamente.";
                return View();
            }
        }

        /* --------------------------------------------------------------------------------------  */


        /* --------------------------------------------------------------------------------------  */
        public bool SaveEquipo(equipomodels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    equipo T = new equipo();
                    T = db.equipos.Single(q => q.id == model.id);
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
            var equipo = new equipomodels().verequipo(id).First();
            MvcApplication6.Models.equipo T = new equipo
            {
                id      = equipo.id,
                nombre  = equipo.nombre
            };
            try
            {
                db.equipos.Attach(T);
                db.equipos.DeleteOnSubmit(T);
                db.SubmitChanges();
                TempData["MENSAJE"] = "Se elimino correctamente el equipo.";
            }
            catch {
                TempData["MENSAJE"] = "EL equipo no ha podido eliminarse.";
            }
            return RedirectToAction("Index");
        }
        /* --------------------------------------------------------------------------------------  */
       

    }
}
