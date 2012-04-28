using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    [Login(rol = "admin")]
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
            try
            {
                db.usuarios.Attach(T);
                db.usuarios.DeleteOnSubmit(T);
                db.SubmitChanges();
                TempData["MENSAJE"] = "Se elimino correctamente el usuario.";
            }
            catch
            {
                TempData["ERROR"] = "No se pudo eliminar el usuario, por favor intente nuevamente.";
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
                    username = model.username,
                    password = model.password,
                    nombre = model.nombre,
                    apellido = model.apellido,
                    fechanac = model.fechanac,
                    email = model.email,
                    idrol = 1,
                };
                db.usuarios.InsertOnSubmit(U);
                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    TempData["ERROR"] = "No se pudo agregar el usuario, por favor intente nuevamente.";
                    return View();
                }
                TempData["MENSAJE"] = "Se agrego correctamente el usuario.";
                return RedirectToAction("Index");
            }
            TempData["ERROR"] = "Error en el alta del usuario.";
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
            usuario U = new usuario();
            try
            {
                U = db.usuarios.Single(q => q.id == id);
                usuariomodels tmodel = new usuariomodels();
                
                    tmodel.id          = U.id;
                    tmodel.nombre      = U.nombre;
                    tmodel.username    = U.username;
                    tmodel.password    = U.password;
                    tmodel.apellido    = U.apellido;
                    tmodel.fechanac    = U.fechanac;
                    tmodel.email       = U.email;
                return View(tmodel);
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar el usuario, por favor intente nuevamente.";
                return RedirectToAction("Index");
            }
        }
        /* --------------------------------------------------------------------------------------  */

        [HttpPost]
        public ActionResult Editar(int id, usuariomodels model)
        {
            try
            {
                if (this.SaveUsuario(model))
                {
                    TempData["MENSAJE"] = "Se edito correctamente el usuario.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ERROR"] = "No se pudo editar el usuario, por favor intente nuevamente.";
                    return View();
                }
            }
            catch
            {
                TempData["ERROR"] = "No se pudo editar el usuario, por favor intente nuevamente.";
                return View();
            }
        }

        /* --------------------------------------------------------------------------------------  */
        public bool SaveUsuario(usuariomodels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    usuario U = new usuario();
                    U = db.usuarios.Single(q => q.id == model.id);

                    U.id = model.id;
                    U.nombre = model.nombre;
                    U.username = model.username;
                    U.password = model.password;
                    U.apellido = model.apellido;
                    U.fechanac = model.fechanac;
                    U.email = model.email;
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
    }

}
