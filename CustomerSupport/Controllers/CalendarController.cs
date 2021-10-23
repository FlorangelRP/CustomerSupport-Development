using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar

        public ActionResult ListCalendar()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTask").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Visible == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public ActionResult GetCalendarData(int? id)
        {
            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                int? idResponsable = null;
                if(id!=null && id>0)
                {
                    idResponsable = id;
                }

                int? IdUser = ((MSerUser)Session["Usuario"]).IdUser;

                // Loading.
                List<MCalendar> data = fnListCalendar(null, null, null, idResponsable, null, null, null, null, null, IdUser, null);

                // Processing.
                result = this.Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }


        public static List<MCalendar> fnListCalendar(int? IdTask = null, DateTime? dttDateIni = null, DateTime? dttDateEnd = null, int? IdResponsable = null, string strTittle = "", int? IdPriority = null, int? IdStatus = null, int? IdTypeTask = null, int? IdServiceRequest = null, int? IdUser = null, int? IdFatherTask = null)
        {


            List<MCalendar> listMCalendar = new List<MCalendar>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser objUser = new MUser();


            listMCalendar = (List<MCalendar>)(from tsk in db.GNListTask(IdTask, dttDateIni, dttDateEnd, IdResponsable, strTittle, IdPriority, IdStatus, IdTypeTask, IdServiceRequest, IdUser, IdFatherTask).ToList()
                                     select new MCalendar
                                     {
                                         IdTask = tsk.IdTask,
                                         Tittle = tsk.Tittle,
                                         Activity = tsk.Activity,
                                         DateIni = tsk.DateIni.ToString("yyyy-MM-dd"),
                                         DateEnd = tsk.DateEnd.ToString("yyyy-MM-dd"),
                                         HourIni = tsk.HourIni.ToString(),
                                         HourEnd = tsk.HourEnd.ToString(),
                                         StatusTask = tsk.Status
                                     }).ToList();
            return listMCalendar;

        }
    }
}
