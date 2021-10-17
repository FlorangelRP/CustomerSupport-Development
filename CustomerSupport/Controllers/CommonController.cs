using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class CommonController : Controller
    {
        public JsonResult ResetSession()
        {
            var nTimeout = Session.Timeout;
            return Json(nTimeout, JsonRequestBehavior.AllowGet);
        }

    }
}
