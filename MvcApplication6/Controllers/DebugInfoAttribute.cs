using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using MvcApplication6.Models;
using System.Web.Routing;
using System.Web.Security;

namespace MvcApplication6.Controllers
{
    public class DebugInfoAttribute : ActionFilterAttribute
    {        

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuariomodels u = new usuariomodels();

            SessionStateTempDataProvider s = new SessionStateTempDataProvider();
            String y = s.SaveTempData(controllerContext, ["Rol", "usuario]");
            
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            

        }
    }
}