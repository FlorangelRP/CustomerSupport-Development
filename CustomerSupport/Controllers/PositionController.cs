using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CustomerSupport.Controllers
{
    public class PositionController : Controller
    {


        public static List<MPosition> fnListPosition(int? IdPosition = null, int? IdDepartment=null, int? IdPositionLevel = null, bool? Status=null)
        {
            List<MPosition> ListPosition = new List<MPosition>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();


            ListPosition = (from result in db.GNListPosition(IdPosition, IdDepartment, IdPositionLevel, Status).ToList()
                            select new MPosition
                            {
                                IdPosition=result.IdPosition,
                                Description=result.Description,
                                IdDepartment=result.IdDepartment,
                                Department=result.Department,
                                IdPositionLevel=result.IdPositionLevel,
                                PositionLevel=result.PositionLevel,
                                Status=result.Status,
                                StatusDesc = result.Status == true ? "Activo" : "Inactivo",
                                listAssociatePosition = (List<MAssociatePosition>)(from result2 in db.GNListAssociatePosition(result.IdPosition, null).ToList()
                                                                                   select new MAssociatePosition
                                                                                   {
                                                                                       IdPosition = result2.IdPosition,
                                                                                       IdAssociate = result2.IdAssociate,
                                                                                       PositionDesc = result2.PositionDesc,
                                                                                       AssociateDesc = result2.AssociateDesc
                                                                                   }).ToList()

                            }).ToList();



            return ListPosition;

        }


        public ActionResult GetPositionForDeparment(int? IdDepartment=null)
        {
            List<MPosition> ListPosition = new List<MPosition>();

            ListPosition = fnListPosition(null,IdDepartment,null,true);

            return Json(ListPosition, JsonRequestBehavior.AllowGet);

        }
    }
}
