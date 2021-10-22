using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class CommonController : Controller
    {
        #region Session Expire Notification 
        public JsonResult ResetSession()
        {
            return Json(Session.Timeout, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
