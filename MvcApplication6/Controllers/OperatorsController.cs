using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication6.Models;

namespace MvcApplication6.Controllers
{
    public class OperatorsController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();

        /* --------------------------------------------------------------------------------------  */
        public ActionResult Cargarresultados(int idf)
        {
            var lpartidos = new partidomodels().listapartidosporfecha(idf);
            ViewData["lpartidos"] = lpartidos;
            partidomodels T = new partidomodels();
            return View();
        }
        [HttpPost]
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Cargarresultados(int idf, partidomodels model)
        {
            var lpartidos = new partidomodels().listapartidosporfecha(idf);
            ViewData["lpartidos"] = lpartidos;
            try
            {
                if (this.SavePartido(model))
                {
                    TempData["MENSAJE"] = "Partido Actualizado Correctamente";
                    return RedirectToAction("Cargarresultados", new { idf = model.idfechatorneo });
                }
                else
                {
                    TempData["ERROR"] = "El Partido no se pudo actualizar, por favor intente nuevamente";
                    return View();
                }

            }
            catch
            {
                TempData["ERROR"] = "El Partido no se pudo actualizar, por favor intente nuevamente";
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
                    MvcApplication6.Models.partido T = new MvcApplication6.Models.partido();
                    T = db.partidos.Single(q => q.id == model.id);
                    T.equipolocal = model.equipolocal;
                    T.equipovisita = model.equipovisita;
                    T.goleslocal = model.goleslocal;
                    T.golesvisita = model.golesvisita;
                    T.idfechatorneo = model.idfechatorneo;
                    T.estadio = model.estadio;
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
        [HttpGet]
        public JsonResult getfechasportorneo(int idf)
        {
            var lfechas = new fechamodels().listafechasportorneo(idf);
            return Json(lfechas, JsonRequestBehavior.AllowGet);
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Seleccionarfechaportorneo()
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;
            fechamodels F = new fechamodels();
            return View(F);
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult Seleccionarfechaportorneo(string accion, fechamodels F)
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;

            return RedirectToAction(accion, new { idf = F.id });
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Meterpuntosboleta(int idf)
        {   /*  recorro partidos y y voy metiendo los puntos en boletas*/
            try { new boletamodels().meterpuntosboleta(idf);
            TempData["MENSAJE"] = "Se Calcularon Correctamente los Puntos de los Usuarios."; 
            }
            catch { TempData["ERROR"] = "No se pudo Calcular los puntos de los usuarios, por favor intenten nuevamente."; 
            }
            return RedirectToAction("HomeOperador", "Admin");
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Cargartopfive(int idf)
        {
            try
            {
                var lboletas = new boletamodels().boletasxfechaxpuntos(idf);
                new topfivemodels().eliminartabla();
                new topfivemodels().insertarpuntos(lboletas);
                TempData["MENSAJE"] = "Se Cargo Correctamente el Top FIVE."; 
            }
            catch
            {
                TempData["ERROR"] = "No se pudo Calcular el Top Five, por favor intenten nuevamente.";
            }
            return RedirectToAction("HomeOperador", "Admin");
        }
            /* --------------------------------------------------------------------------------------  */
        public ActionResult Cargartopten(int idt)
        {
            try
            {
                var lboletas = new boletamodels().boletasxtorneoxpuntos(idt);
                new toptenmodels().eliminartabla();
                new toptenmodels().insertarpuntos(lboletas);
                TempData["MENSAJE"] = "Se Cargo Correctamente el Top Ten."; 
            }
            catch
            {
                TempData["ERROR"] = "No se pudo Calcular el Top Ten, por favor intenten nuevamente.";
            }
            return RedirectToAction("HomeOperador", "Admin");
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Seleccionartorneo()
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;
            torneomodels  T = new torneomodels();
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
            
    }

}
