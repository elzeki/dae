using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication6.Models;

namespace MvcApplication6.Controllers
{
    public class UsuariosController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();
        
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Editarboleta(int idb)
        {
            var idUser = (int)Session["id"];
            var boleta = new boletamodels().boletaporfechaporuser(idb, idUser);
            ViewData["listaboletas"] = boleta;
            boletamodels T = new boletamodels();
            return View(T);
        }
        /* --------------------------------------------------------------------------------------  */

        [HttpPost]
        public ActionResult Editarboleta(int idb, boletamodels model)
        {
            var idUser = (int)Session["id"];
            var boleta = new boletamodels().boletaporfechaporuser(idb, idUser);
            ViewData["listaboletas"] = boleta;
            try
            {
                if (this.SaveBoleta(model))
                {
                    TempData["MENSAJE"] = "Boleta Actualizada Correctamente";
                }
                else
                {
                    TempData["ERROR"] = "La Boleta no se pudo actualizar, por favor intente nuevamente";
                    return View();
                }
                return RedirectToAction("Editarboleta", new { idb = model.idfechatorneo });
            }
            catch
            {
                TempData["ERROR"] = "No se pudo cargar la Boleta, por favor intente nuevamente";
                return View();
            }
        }
        /* --------------------------------------------------------------------------------------  */
        public bool SaveBoleta(boletamodels model)
        {
            if (ModelState.IsValid)
            {
                boleta T = new boleta();
                var idUser = (int)Session["id"];
                T = db.boletas.Single(q => q.id == model.id);
                T.idusuario = idUser;
                T.equipolocal = model.equipolocal;
                T.equipovisita = model.equipovisita;
                T.goleslocal = model.goleslocal;
                T.golesvisita = model.golesvisita;
                T.idfechatorneo = model.idfechatorneo;
                T.estadio = model.estadio;
                T.puntostotales = model.puntostotales;
                T.editable = model.editable;
                db.SubmitChanges();
                return true;
            }
            return false;
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Cargarboleta(int idf)
        {
            var idUser = (int)Session["id"];
            var lboletas = new boletamodels().boletaporfechaporuser(idf, idUser);
            var cantboletas = lboletas.Count();

            if (cantboletas == 0) // si es 0, es por que el usuario no cargo la boleta correspondiente a la fecha.
            {
                var lpartidos = new partidomodels().listapartidosporfecha(idf);
                if (lpartidos.Count() == 0)
                {
                    TempData["ERROR"] = "No se definieron partidos para la fecha seleccionada, por favor seleccione otra fecha.";
                    return RedirectToAction("boletaporfecha");
                }               
                foreach (var aux in lpartidos)
                {
                    boleta F = new boleta();
                    F.idfechatorneo = aux.idfechatorneo;
                    F.equipolocal = aux.equipolocal;
                    F.equipovisita = aux.equipovisita;
                    F.goleslocal = 0;
                    F.golesvisita = 0;
                    F.estadio = aux.estadio;
                    F.puntostotales = 0;
                    F.idusuario = idUser;
                    F.editable = 1;
                    db.boletas.InsertOnSubmit(F);
                    try
                    {
                        db.SubmitChanges();
                    }
                    catch
                    {
                        TempData["ERROR"] = "No se pudo cargar la Boleta, por favor intente nuevamente";
                    }

                }//foreach
            }// if (cantboletas == 0)
            return RedirectToAction("Editarboleta", new { idb = idf });
        }
        /* --------------------------------------------------------------------------------------  */        
        public ActionResult Verpuntosportorneo(int idtorneo)
        {
            var iduser = (int)Session["id"];
            var total = new usuariomodels().vermispuntosportorneo(idtorneo, iduser);
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Verpuntosporfecha(int idf)
        {          var  iduser =(int) Session["id"];
                   var total = new usuariomodels().vermispuntosporfecha(idf, iduser);
                   return View();
        }

        /* --------------------------------------------------------------------------------------  */
      
        public ActionResult Index()
        {
            var lusuarios = new usuariomodels().listausuarios();
            return View(lusuarios);
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Ver(int id)
        {
            var auxusuario = new usuariomodels().verusuario(id).First();
            ViewBag.id          = auxusuario.id;
            ViewBag.username    = auxusuario.username;
            ViewBag.password    = auxusuario.password;
            ViewBag.nombre      = auxusuario.nombre;
            ViewBag.appellido   = auxusuario.apellido;
            ViewBag.fechanac    = auxusuario.fechanac;
            ViewBag.email       = auxusuario.email;

            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Eliminar(int id)
        {
            var aux = new usuariomodels().verusuario(id).First();
            usuario T = new usuario
            {
                id = aux.id,
                nombre = aux.nombre,
                username = aux.username,
                password = aux.password,
                apellido = aux.apellido,
                fechanac = aux.fechanac,
                email = aux.email,
            };
            db.usuarios.Attach(T);
            db.usuarios.DeleteOnSubmit(T);
            db.SubmitChanges();

            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Make some adjustments.
                // ...
                // Try again.
                //db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult Agregar(usuariomodels model)
        {
            if (ModelState.IsValid)
            {
                usuario U = new usuario
                {
                    id          = model.id,
                    nombre      = model.nombre,
                    username    = model.username,
                    password    = model.password,
                    apellido    = model.apellido,
                    fechanac    = model.fechanac,
                    email       = model.email,
                };

                db.usuarios.InsertOnSubmit(U);
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Make some adjustments.
                    // ...
                    // Try again.
                    //db.SubmitChanges();
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Agregar()
        {
            return View();
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult Editar(int idu)
        {
            MvcApplication6.Models.usuario Us = new MvcApplication6.Models.usuario();
            try
            {
                Us = db.usuarios.Single(q => q.id == idu);
                usuariomodels tmodel = new usuariomodels();
                
                    tmodel.id          = Us.id;
                    tmodel.nombre      = Us.nombre;
                    tmodel.username    = Us.username;
                    tmodel.password    = Us.password;
                    tmodel.apellido    = Us.apellido;
                    tmodel.fechanac    = Us.fechanac;
                    tmodel.email       = Us.email;
                return View(tmodel);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        /* --------------------------------------------------------------------------------------  */

        [HttpPost]
        public ActionResult Editar(int idu, usuariomodels model)
        {
            try
            {
                if (this.SaveTorneo(model))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }

        /* --------------------------------------------------------------------------------------  */
        public bool SaveTorneo(usuariomodels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    usuario T = new usuario();
                    T = db.usuarios.Single(q => q.id == model.id);

                    T.id        =model.id;
                    T.nombre    =model.nombre;
                    T.username  =model.username;
                    T.password  =model.password;
                    T.apellido  =model.apellido;
                    T.fechanac  =model.fechanac;
                    T.email     = model.email;
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
        /* -------------------------------------------------------------------------------------------------------  */
        [HttpGet]
        public JsonResult getboletasportorneo(int idb)
        {
            var lfechas = new fechamodels().listafechasportorneo(idb);
            return Json(lfechas, JsonRequestBehavior.AllowGet);
        }
        /* --------------------------------------------------------------------------------------  */
        public ActionResult boletaporfecha(String accion)
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;
            fechamodels F = new fechamodels();
            return View(F);
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult boletaporfecha(String accion, fechamodels F)
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;
            return RedirectToAction(accion, new { idf = F.id });
        }

        /* --------------------------------------------------------------------------------------  */
        public ActionResult boletasportorneo(string accion)
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;
            torneomodels T = new torneomodels();
            return View(T);
        }
        /* --------------------------------------------------------------------------------------  */
        [HttpPost]
        public ActionResult boletasportorneo(string accion, torneomodels T)
        {
            var ltorneos = new torneomodels().listatorneos();
            ViewData["listatorneos"] = ltorneos;

            return RedirectToAction(accion, new { idtorneo = T.id });
        }
        /* --------------------------------------------------------------------------------------  */
         
        /* --------------------------------------------------------------------------------------  */
         /* --------------------------------------------------------------------------------------  */
         /* --------------------------------------------------------------------------------------  */
         /* --------------------------------------------------------------------------------------  */
         /* --------------------------------------------------------------------------------------  */
         /* --------------------------------------------------------------------------------------  */
        /* --------------------------------------------------------------------------------------  */
       
    }

}
