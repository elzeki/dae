using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    [Login(rol = "admin")]
    public class FechasController : Controller
    {   
        private PRODEDataContext db = new PRODEDataContext();

        /* --------------------------------------------------------------------------------------  */
        public ActionResult Index()
        {            
            var lfechas = new fechamodels().listafechas();
            return View(lfechas);
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Ver(int id)
        {            
            var fecha = new fechamodels().verfecha(id).First();
            @ViewBag.id = fecha.id;
            @ViewBag.idtorneo = fecha.idtorneo;
            @ViewBag.nombre     = fecha.nombre;
            @ViewBag.fecha       = fecha.fecha1;
            return View();
        }

        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult Agregar(fechamodels model)
        {
            var ltorneos = new torneomodels().listatorneos(); 
            ViewData["listatorneos"] = ltorneos;

            if (ModelState.IsValid)
            {

                fecha F = new DAE_Prode.Models.fecha
                {
                    idtorneo = model.idtorneo,
                    nombre = model.nombre,
                    fecha1 = model.fecha1,
                };

                db.fechas.InsertOnSubmit(F);
                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    TempData["ERROR"] = "No se pudo agregar la fecha, por favor intente nuevamente.";
                    return View();

                }
                TempData["MENSAJE"] = "Se agrego correctamente la fecha.";
                return RedirectToAction("Index");
            }
            TempData["ERROR"] = "Error en el alta de la fecha. Modelo Invalido";
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
       
        public ActionResult Agregar()
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;

            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Eliminar(int id)
        {
            var aux = new fechamodels().verfecha(id).First();
            DAE_Prode.Models.fecha F = new fecha
            {
                id = aux.id,
                idtorneo = aux.idtorneo,
                nombre = aux.nombre,
                fecha1 = aux.fecha1,
            };            
            try
            {
                db.fechas.Attach(F);
                db.fechas.DeleteOnSubmit(F);
                db.SubmitChanges();
                TempData["MENSAJE"] = "Se elimino correctamente la fecha.";
            }
            catch
            {
                TempData["ERROR"] = "No se pudo eliminar la fecha, por favor intente nuevamente.";
            }
            return RedirectToAction("Index");
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Editar(int id)
        {
            fecha T = new fecha();
            try
            {
                T = db.fechas.Single(q => q.id == id);
                fechamodels fmodel = new fechamodels();
                fmodel.id       = T.id;
                fmodel.idtorneo = T.idtorneo;
                fmodel.nombre   = T.nombre;
                fmodel.fecha1 = T.fecha1;
                return View(fmodel);
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar la fecha, por favor intente nuevamente.";
                return RedirectToAction("Index");
            }
        }
        /* --------------------------------------------------------------------------------------  */

        [HttpPost]
        public ActionResult Editar(int id, fechamodels model)
        {
            try
            {
                if (this.SaveFecha(model))
                {
                    TempData["MENSAJE"] = "Se edito correctamente la fecha.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ERROR"] = "No se pudo editar la fecha, por favor intente nuevamente.";
                    return View();
                }
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar la fecha, por favor intente nuevamente.";
                return View();
            }
        }

        /* --------------------------------------------------------------------------------------  */
        public bool SaveFecha(fechamodels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    fecha T = new fecha();
                    T = db.fechas.Single(q => q.id == model.id);
                    T.nombre    = model.nombre;
                    T.idtorneo  = model.idtorneo;
                    T.nombre    = model.nombre;
                    T.fecha1    = model.fecha1;
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
	

    }
}