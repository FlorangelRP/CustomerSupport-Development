using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CustomerSupport.Controllers
{
    public class PerformanceController : Controller
    {
        // GET: Performance
        public ActionResult ListPerformance()
        {

            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;

            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListPerformance").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Visible == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult BarChart(Mperformace objDatos)
        {
            if(objDatos.XEmployee ==false)
            {
                objDatos.IdEmployee = 0;
            }
            else
            {
                if (objDatos.IdEmployee == null)
                    objDatos.IdEmployee=0;

                if (objDatos.IdDepartment == null)
                    objDatos.IdDepartment = 0;

            }

            if (objDatos.XDepartament == false && objDatos.XEmployee == false)
            {
                objDatos.IdDepartment = 0;
            }else
            {
                if(objDatos.IdDepartment==null)
                {
                    objDatos.IdDepartment = 0;
                }

            }

            if (objDatos.XMonth == false)
            {
                objDatos.Year = 0;
            }
            else
            {
                if (objDatos.Year == null)
                {
                    objDatos.Year = 0;
                }
            }

            if (objDatos.XYear == false)
            {
                objDatos.YearIni = 0;
                objDatos.YearEnd = 0;
            }

            if (objDatos.XDate == false)
            {
                objDatos.DateIni = null;
                objDatos.DateEnd = null;
            }

            var result = Json(CreatedataTable(objDatos), JsonRequestBehavior.AllowGet);

            return result;
        }


        public List<object> CreatedataTable(Mperformace objDatos)
        {
            MEstadistica objEsta = new MEstadistica();
            DataTable dt = new DataTable();
            dt.Columns.Add("label", System.Type.GetType("System.String"));
            dt.Columns.Add("backgroundColor", System.Type.GetType("System.String"));
            dt.Columns.Add("borderColor", System.Type.GetType("System.String"));
            dt.Columns.Add("Estado", System.Type.GetType("System.Object"));
            dt.Columns.Add("data", System.Type.GetType("System.Object"));
            

            List<MPerformance> listMPerformance = new List<MPerformance>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            listMPerformance = (List<MPerformance>)(from tsk in db.GNLisChart(objDatos.XEmployee,objDatos.IdEmployee,objDatos.XDepartament, objDatos.IdDepartment,objDatos.XMonth,objDatos.Year,objDatos.XYear,objDatos.YearIni,objDatos.YearEnd,objDatos.XDate, objDatos.DateIni,objDatos.DateEnd,objDatos.Xtype,objDatos.IdTypeTask).ToList()
                                                    select new MPerformance
                                                    {
                                                        Nombre = tsk.Nombre,
                                                        StatusTask = tsk.StatusTask,
                                                        Cantidad = (int)tsk.Cantidad
                                                    }).ToList();


            var grouped = from p in listMPerformance
                          group p by new
                          {
                              Nombre = p.Nombre
                          } into d
                          select new { d.Key };

            List<string> objLista = new List<string>();
            List <VWListCatalog>ListTableCatalog = null;
            if (objDatos.Xtype==true)
            {
                 ListTableCatalog = db.VWListCatalog.Where(t => t.IdTable == "TYPETASK").ToList();
            }
            else
            { 
             ListTableCatalog = db.VWListCatalog.Where(t => t.IdTable == "STATETASK").ToList();
            }
            List<string> objListEstado = new List<string>();
            foreach (var item in ListTableCatalog)
            {               
               objListEstado.Add(item.DetailDesc);
            }


            if (ListTableCatalog.Count > 0)
            {

                foreach (var itemg in grouped)
                {
                    objLista.Add(itemg.Key.Nombre);

                    List<int> objLisCant = new List<int>();
                

                    foreach (var item in ListTableCatalog)
                    {

                        var LisResp =  listMPerformance.Where(x=>x.Nombre== itemg.Key.Nombre && x.StatusTask == item.DetailDesc).ToList();
                        if(LisResp.Count>0)
                        {
                            objLisCant.Add(LisResp[0].Cantidad);
                        }
                       else
                        {
                            objLisCant.Add(0);
                        }

                    }

                    dt.Rows.Add(itemg.Key.Nombre, " ", "", objLisCant.ToArray(), objListEstado.ToArray());
                }

            }

            var listColor = (from tsk in db.GNLisColor() select new { tsk.backgroundColor, tsk.borderColor }).ToList();

            int h = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                while (i < listColor.Count-1 )
                {
                    dt.Rows[i]["backgroundColor"] = listColor[i].backgroundColor;
                    dt.Rows[i]["borderColor"] = listColor[i].borderColor;
                    break;
                }

             }

            dt.AcceptChanges();

            
            objEsta.Nombre = objLista;
            objEsta.Data = dt;
            List<object> iData = new List<object>();
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return iData;

        }
    }
}
