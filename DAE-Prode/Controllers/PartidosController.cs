using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    [Login(rol = "admin")]
    public class PartidosController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();

        /* --------------------------------------------------------------------------------------  */
        public ActionResult Index()
        {
            var lpartidos = new partidomodels().listapartidos();
            return View(lpartidos);
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Ver(int id)
        {
            var partido = new partidomodels().verpartido(id).First();
            
            ViewBag.id              = partido.id;
            ViewBag.equipolocal     = partido.equipolocal;
            ViewBag.equipovisita    = partido.equipovisita;
            ViewBag.goleslocal      = partido.goleslocal;
            ViewBag.golesvisita     = partido.golesvisita;
            ViewBag.idfechatorneo   = partido.idfechatorneo;
            ViewBag.jugado          = partido.jugado;
            ViewBag.estadio         = partido.estadio;
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Eliminar(int id)
        {
            var aux = new partidomodels().verpartido(id).First();
            partido F = new partido
            {
                id              = aux.id,
                idfechatorneo   = aux.idfechatorneo,
                equipolocal     = aux.equipolocal,
                equipovisita    = aux.equipovisita,
                goleslocal      = aux.goleslocal,
                golesvisita     = aux.golesvisita,
                jugado          =aux.jugado,
                estadio         = aux.estadio,
            };

            try
            {
                db.partidos.Attach(F);
                db.partidos.DeleteOnSubmit(F);
                db.SubmitChanges();
                TempData["MENSAJE"] = "Se elimino correctamente el partido.";
            }
            catch 
            {
                TempData["ERROR"] = "No se pudo eliminar el partido, por favor intente nuevamente.";
            }
            return RedirectToAction("Index");
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult Agregar(partidomodels model)
        {
            var lfechas = new fechamodels().listafechas();
            ViewData["listafechas"] = lfechas;


            var lequipos = new equipomodels().listaequipos();
            ViewData["listaequipos"] = lequipos;

            if (ModelState.IsValid)
            {
                if (model.equipolocal != model.equipovisita)
                {
                    partido F = new partido
                    {
                        id              = model.id,
                        idfechatorneo   = model.idfechatorneo,
                        equipolocal     = model.equipolocal,
                        equipovisita    = model.equipovisita,
                        goleslocal      = 0,
                        golesvisita     = 0,
                        jugado          = 0,
                        estadio         = model.estadio,
                    };

                    db.partidos.InsertOnSubmit(F);

                    try
                    {
                        db.SubmitChanges();
                    }
                    catch 
                    {
                        TempData["ERROR"] = "No se pudo agregar el partido, por favor intente nuevamente.";
                        return View();
                    }

                    TempData["MENSAJE"] = "Se agrego correctamente el partido.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ERROR"] = "Error en el alta del partido, los equipos deben ser distintos, por favor intente nuevamente.";
                    return View();

                }
            }
            else
            {
                TempData["ERROR"] = "Error en el alta del partido, por favor intente nuevamente.";
            }
            
       
         return View();
        }
        /* --------------------------------------------------------------------------------------  */

        public ActionResult Agregar()
        {
            var lfechas = new fechamodels().listafechas();
            ViewData["listafechas"] = lfechas;

            var lequipos = new equipomodels().listaequipos();
            ViewData["listaequipos"] = lequipos;

            return View();
        }
       
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Editar(int idp)
        {
            partido T = new partido();

            var lfechas = new fechamodels().listafechas();
            ViewData["listafechas"] = lfechas;


            var lequipos = new equipomodels().listaequipos();
            ViewData["listaequipos"] = lequipos;

            try
            {
                T = db.partidos.Single(q => q.id == idp);
                partidomodels pmodel = new partidomodels();
                pmodel.id           =T.id;
                pmodel.equipolocal  =T.equipolocal;
                pmodel.equipovisita =T.equipovisita;
                pmodel.goleslocal   =T.goleslocal;
                pmodel.golesvisita  =T.golesvisita;
                pmodel.idfechatorneo=T.idfechatorneo;
                pmodel.jugado       = T.jugado;
                pmodel.estadio      = T.estadio;
                return View(pmodel);
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar el partido, por favor intente nuevamente.";
                return RedirectToAction("Index");
            }
        }
        /* --------------------------------------------------------------------------------------  */

        [HttpPost]
        public ActionResult Editar(int idp, partidomodels model)
        {
            var lfechas = new fechamodels().listafechas();
            ViewData["listafechas"] = lfechas;


            var lequipos = new equipomodels().listaequipos();
            ViewData["listaequipos"] = lequipos;

            try
            {
                if (this.SavePartido(model))
                {
                    TempData["MENSAJE"] = "Se edito correctamente el partido.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ERROR"] = "No se pudo editar el partido, por favor intente nuevamente.";
                    return View();
                }
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar el partido, por favor intente nuevamente.";
                return View();
            }
        }

        /* --------------------------------------------------------------------------------------  */
        public bool SavePartido(partidomodels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    partido T = new partido();
                    T = db.partidos.Single(q => q.id == model.id);
                    T.equipolocal   = model.equipolocal;
                    T.equipovisita  = model.equipovisita;
                    T.goleslocal    = model.goleslocal;
                    T.golesvisita   = model.golesvisita;
                    T.idfechatorneo = model.idfechatorneo;
                    T.jugado        = model.jugado;
                    T.estadio       = model.estadio;
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else { TempData["ERROR"] = "No se pudo editar el partido, por favor intente nuevamente."; }
            return false;
        }
        /* --------------------------------------------------------------------------------------  */   
    }
}
