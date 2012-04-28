using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DAE_Prode;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    public class AccountController : Controller
    {
        private PRODEDataContext db = new PRODEDataContext();

        /*----------------------------------------------------------------------------------------*/
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            ViewBag.Error = TempData["Error"];
            return View();
        }
        /*----------------------------------------------------------------------------------------*/
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString("LoginInCorrecto"));
                    return View();
                }
            }
            ModelState.AddModelError("", ErrorCodeToString("LoginInCorrecto"));
            return RedirectToAction("Index", "Home");
        }
        /*----------------------------------------------------------------------------------------*/
        public Boolean ValidateUser(String username, String password)
        {
            try
            {
                var existeusuario = db.usuarios.Single(u => u.username == username);
                if ((existeusuario.password == password) && (existeusuario.idrol == 3))
                {
                    Session.Add("id", existeusuario.id);
                    Session.Add("username", existeusuario.username);                    
                    Session.Add("nombre", existeusuario.nombre);
                    Session.Add("rol", "usuario");
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
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        /*----------------------------------------------------------------------------------------*/
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }
        /*----------------------------------------------------------------------------------------*/
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                String createStatus = this.CreateUser(model.UserName);
                if (createStatus == "Success")
                {
                    if (AgregarUsuario(model))
                    {
                        TempData["Message"] = "Se Realizo con exito su registro.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // Si llegamos hasta aquí, algo falló, redisplay form
            return View(model);
        }
        /*----------------------------------------------------------------------------------------*/
        public String CreateUser(String username)
        {
            try
            {
                var existeusuario = db.usuarios.Single(u => u.username == username);
            }
            catch
            {
                return "Success";
            }
            return "DuplicateUserName";
        }
        /* --------------------------------------------------------------------------------------  */
        public Boolean AgregarUsuario(RegisterModel m)
        {
            usuario U = new usuario
            {
                username = m.UserName,
                password = m.Password,
                nombre = m.Nombre,
                apellido = m.Apellido,
                fechanac = m.Fechanac,
                email = m.Email,
                idrol = m.idrol,
            };
            try
            {
                db.usuarios.InsertOnSubmit(U);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /*----------------------------------------------------------------------------------------*/
        // GET: /Account/Perfil

        public ActionResult Perfil()
        {
            return View();
        }
        /* --------------------------------------------------------------------------------------  */


        #region Status Codes
        private static string ErrorCodeToString(String createStatus)
        {
            switch (createStatus)
            {
                case "DuplicateUserName":
                    return "El nombre de usuario ya existe. Por favor, introduzca un nombre de usuario diferente.";

                case "DuplicateEmail":
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case "InvalidPassword":
                    return "La contraseña proporcionada no es válida. Por favor, introduzca un valor de contraseña válida.";

                case "InvalidEmail":
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case "InvalidAnswer":
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case "InvalidQuestion":
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case "InexistentUserName":
                    return "The user name provided is invalid. Please check the value and try again.";

                case "ProviderError":
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case "UserRejected":
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case "LoginCorrecto":
                    return "El login fué exitoso.";

                case "LoginInCorrecto":
                    return "El nombre de usuario o contraseña es incorrecto.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
