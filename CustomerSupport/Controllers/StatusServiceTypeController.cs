using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class StatusServiceTypeController : Controller
    {
        public ActionResult GetListStatusServiceType(int? IdServiceType)
        {
            List<MCatalogDetail> ListStatusServiceType = new List<MCatalogDetail>();
            ListStatusServiceType = fnListStatusServiceType(IdServiceType, null, true, true);

            return Json(ListStatusServiceType, JsonRequestBehavior.AllowGet);
        }

        public static List<MCatalogDetail> fnListStatusServiceType(int? IdServiceType, int? IdServiceStatus, bool? StatusServiceType = null, bool? StatusServiceStatus = null)
        {
            List<MCatalogDetail> ListStatusServiceType = new List<MCatalogDetail>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ListStatusServiceType = (from result in db.GNListStatusServiceType(IdServiceType, IdServiceStatus, StatusServiceType, StatusServiceStatus).ToList()
                          select new MCatalogDetail
                          {
                              IdCatalogDetail = result.IdServiceStatus,
                              IdTableDetail = result.IdTableDetail,
                              Description = result.ServiceStatus
                          }).ToList();



            return ListStatusServiceType;
        }

    }
}
