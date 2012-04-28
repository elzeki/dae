using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    [Login(rol = "operador")]
    public class OperatorsController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();

        public ActionResult Cerrarboletas(FormCollection formCollection)
        {
            
           var idfecha=System.Convert.ToInt32( formCollection["idfecha"]);
           IEnumerable<boletamodels> lboletas = new boletamodels().boletaporfecha(idfecha);
           foreach (var auxb in lboletas)
           {
               boleta T = new boleta();
               T = db.boletas.Single(q => q.id == auxb.id);
               T.idusuario      = auxb.idusuario;
               T.equipolocal    = auxb.equipolocal;
               T.equipovisita   = auxb.equipovisita;
               T.goleslocal     = auxb.goleslocal;
               T.golesvisita    = auxb.golesvisita;
               T.idfechatorneo  = auxb.idfechatorneo;
               T.estadio        = auxb.estadio;
               T.puntostotales  = auxb.puntostotales;
               T.editable       = 0;
               db.SubmitChanges();
           }
           return RedirectToAction("Cargarresultados", new  {idf=idfecha });
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Cargarresultados(int idf)
        {
            ViewBag.idfecha = idf;
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
            ViewBag.idfecha = idf;
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
                    DAE_Prode.Models.partido T = new DAE_Prode.Models.partido();
                    T = db.partidos.Single(q => q.id == model.id);
                    T.equipolocal   = model.equipolocal;
                    T.equipovisita  = model.equipovisita;
                    T.goleslocal    = model.goleslocal;
                    T.golesvisita   = model.golesvisita;
                    T.idfechatorneo = model.idfechatorneo;
                    T.jugado        = 1;
                    T.estadio       = model.estadio;
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
            catch { TempData["ERROR"] = "No se pudo Calcular los puntos de los usuarios, por favor intenten nuevamente.";}
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
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Cargartablageneral(int idt)
        {
            var lpartidos = new partidomodels().listapartidosportorneo(idt);
            var pepe = lpartidos.Count();
            var puntoslocal = 0;
            var puntosvisitante = 0;
            new tablageneralmodels().eliminartabla();

            foreach (var auxp in lpartidos)
            {
                if (auxp.goleslocal > auxp.golesvisita)
                {
                    puntoslocal = 2;
                    puntosvisitante = 0;
                }
                else if (auxp.goleslocal < auxp.golesvisita)
                    {
                        puntoslocal = 0;
                        puntosvisitante = 2;
                    }
                    else
                    {
                        puntoslocal = 1;
                        puntosvisitante = 1;
                    }

                 Agregarpartido(auxp.equipolocal, puntoslocal);
                 Agregarpartido(auxp.equipovisita, puntosvisitante);
            }


            return View();
        }

        /* --------------------------------------------------------------------------------------  */
        public void Agregarpartido(int idequipo, int puntos)
        {
            tablageneral TG = new tablageneral
            {
                idequipo = idequipo,
                puntos = puntos,
            };
            db.tablagenerals.InsertOnSubmit(TG);
            try
            {
                db.SubmitChanges();
            }
            catch
            {
                TempData["ERROR"] = "No se pudo agregar el equipo, por favor intente nuevamente.";
            }
        }

        /* --------------------------------------------------------------------------------------  */
            
    }

}
