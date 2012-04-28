using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAE_Prode.Models;

namespace DAE_Prode.Controllers
{
    public class LoginAttribute : ActionFilterAttribute
    {
        public String rol;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            var ctx = HttpContext.Current;
 
            // Soporta Session
            if (ctx.Session != null)
            {                
                if (ctx.Session.IsNewSession)
                {
                    ctx.Response.Redirect("~/Home");
                }
                else
                {
                    var rol = ctx.Session["rol"];
                    if (!rol.Equals(rol))
                    {
                        //Redirigimos al usuario para que vuelva a validarse
                        ctx.Response.Redirect("~/Home");
                    }                    
                }
            }
            base.OnActionExecuting(filterContext);
        }
            
    }

}