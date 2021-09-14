using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {

                MStatistics objStatistics = new MStatistics();

                int intEmployee = PersonController.fnListPerson(null, 2).Count(); //2-empleado
                objStatistics.NroEmployee = intEmployee;

                int intClient = PersonController.fnListPerson(null, 1).Count(); //1-Client
                objStatistics.NroClient = intClient;

                objStatistics.ServicesProcess = 0;
                objStatistics.servicesCompleted = 0;

                MMEnterprisesEntities db = new MMEnterprisesEntities();
                var Service = ServiceRequestController.fnListServiceRequest(null,null,null,null,null); //1-Client
                if(Service!=null && Service.Count>0)
                {
                    var ListTableCatalog = db.VWListCatalog.Where(t => t.IdTable == "SERVICESTATUS").ToList();
                    if (ListTableCatalog.Count > 0)
                    {
                        var objProcesando = ListTableCatalog.Where(p => p.DetailDesc == "Procesando").First();
                        var objFinalizado = ListTableCatalog.Where(p => p.DetailDesc == "Finalizado").First();
                        if (objProcesando != null)
                        {
                            var ServicesProcess = Service.FindAll(p => p.IdServiceStatus == objProcesando.IdCatalogDetail).ToList();
                            objStatistics.ServicesProcess = ServicesProcess.Count();
                        }

                        if (objFinalizado != null)
                        {
                            var servicesCompleted = Service.FindAll(p => p.IdServiceStatus == objFinalizado.IdCatalogDetail).ToList();
                            objStatistics.servicesCompleted = servicesCompleted.Count();
                            
                        }
                    }

                }



                return View(objStatistics);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}