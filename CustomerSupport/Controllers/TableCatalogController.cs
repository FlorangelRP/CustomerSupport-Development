using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerSupport.BDContext;
using CustomerSupport.Models;

namespace CustomerSupport.Controllers
{
    public class TableCatalogController : Controller
    {

        public ActionResult ListTableCatalog()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTableCatalog").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Visible == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            MCatalog objMCatalog = new MCatalog();
            return View(objMCatalog);
        }

        [HttpPost]
        public ActionResult ListTableCatalog(string submit, MCatalog objMCatalog)
        {
            if (objMCatalog == null || objMCatalog.IdCatalog == 0)
            {
                return View();
            }

            TempData["DataTableCatalog"] = objMCatalog;

            switch (submit)
            {
                case "searchRow":
                    return RedirectToAction("DetailTableCatalog", "TableCatalog");
                case "editRow":
                    return RedirectToAction("EditTableCatalog", "TableCatalog");
                default:
                    return View();
            }

        }

        public ActionResult GetListTableCatalog()
        {
            List<MCatalog> ListTableCatalog = new List<MCatalog>();
            ListTableCatalog = fnListTableCatalog();

            return Json(ListTableCatalog, JsonRequestBehavior.AllowGet);
        }

        // GET: TableCatalog/DetailTableCatalog/5
        public ActionResult DetailTableCatalog(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTableCatalog").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Search == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataTableCatalog"] != null)
            {
                if (((MCatalog)TempData["DataTableCatalog"]) != null && ((MCatalog)TempData["DataTableCatalog"]).IdCatalog > 0)
                {
                    id = ((MCatalog)TempData["DataTableCatalog"]).IdCatalog;
                }
                else
                {
                    return RedirectToAction("ListTableCatalog", "TableCatalog");
                }
            }
            if (id == null)
            {
                return RedirectToAction("ListTableCatalog", "TableCatalog");
            }
            //-----------------------------------------------------

            MCatalog objTableCatalog = new MCatalog();
            objTableCatalog = fnListTableCatalog(id).First(); 
            return View(objTableCatalog);
        }

        // GET: TableCatalog/AddTableCatalog
        public ActionResult AddTableCatalog()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTableCatalog").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Create == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            MCatalog objMCatalog = new MCatalog();
            objMCatalog.TableDetails = new List<MCatalogDetail>();

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objMCatalog);
        }

        // POST: TableCatalog/AddTableCatalog
        [HttpPost]
        public ActionResult AddTableCatalog(MCatalog objCatalog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string mensaje = "";
                    int resultDb = fnGNTranTableCatalog(objCatalog, "I", ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;
                        return RedirectToAction("AddTableCatalog");
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objCatalog);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objCatalog);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos: " + ex.Message;
                return View(objCatalog);
            }
        }

        // GET: TableCatalog/EditTableCatalog/5
        public ActionResult EditTableCatalog(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTableCatalog").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Edit == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataTableCatalog"] != null)
            {
                if (((MCatalog)TempData["DataTableCatalog"]) != null && ((MCatalog)TempData["DataTableCatalog"]).IdCatalog > 0)
                {
                    id = ((MCatalog)TempData["DataTableCatalog"]).IdCatalog;
                }
                else
                {
                    return RedirectToAction("ListTableCatalog", "TableCatalog");
                }
            }
            if (id == null)
            {
                return RedirectToAction("ListTableCatalog", "TableCatalog");
            }
            //-----------------------------------------------------

            MCatalog objMCatalog = new MCatalog();
            objMCatalog = fnListTableCatalog(id).First();

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objMCatalog);
        }

        // POST: TableCatalog/EditTableCatalog
        [HttpPost]
        public ActionResult EditTableCatalog(MCatalog objCatalog)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string mensaje = "";
                    int resultDb = fnGNTranTableCatalog(objCatalog, "U", ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;

                        //Para evitar que se vea el id en la Url------------
                        MCatalog objMCatalog = new MCatalog();
                        objMCatalog.IdCatalog = objCatalog.IdCatalog;
                        TempData["DataTableCatalog"] = objMCatalog;
                        return RedirectToAction("EditTableCatalog");
                        //---------------------------------------------------
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objCatalog);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objCatalog);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos: " + ex.Message;
                return View(objCatalog);
            }

        }

        public static List<MCatalog> fnListTableCatalog(int? IdCatalog = null, string IdTable = null, int? IdCatalogDetail=null, string IdTableDetail=null, bool? Status=null)
        {
            List<MCatalog> ListTableCatalog = new List<MCatalog>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ListTableCatalog = (from result in db.GNListCatalog(IdCatalog, IdTable).ToList()
                                select new MCatalog
                                {
                                    IdCatalog = result.IdCatalog,
                                    IdTable = result.IdTable,
                                    Description = result.Description,
                                    TableDetails = (from result2 in db.GNListCatalogDetail(result.IdCatalog, result.IdTable, IdCatalogDetail, IdTableDetail, Status).ToList()
                                                 select new MCatalogDetail
                                                 {
                                                     IdCatalogDetail = result2.IdCatalogDetail,
                                                     IdTableDetail = result2.IdTableDetail,
                                                     IdCatalog = result2.IdCatalog,
                                                     Description = result2.DetailDesc,
                                                     Status = result2.DetailStatus,
                                                     StatusDesc = result2.DetailStatus == true ? "Activo" : "Inactivo"
                                                 }).ToList()
                                }).ToList();

            return ListTableCatalog;

        }

        public ActionResult GetTableCatalog(string idTable)
        {            
            object ListTableCatalog;
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ListTableCatalog = db.VWListCatalog.Where(t => t.IdTable == idTable).ToList();

            return Json(ListTableCatalog, JsonRequestBehavior.AllowGet); 
            
        }

        public String GetDescDetailTableCatalog(string idTable, int idDetail)
        {
            string DetailDesc;
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            DetailDesc = db.VWListCatalog.Where(t => t.IdTable == idTable && t.IdCatalogDetail== idDetail).First().DetailDesc;

            return DetailDesc;

        }

        public static int fnGNTranTableCatalog(MCatalog objCatalog, string TransactionType, ref string Mensaje)
        {
            try
            {
                //TextInfo ti = CultureInfo.CurrentCulture.TextInfo; //para capitalizar textos

                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int IdCatalog;
                int IdCatalogDetail;
                int SqlResultCatalog;
                int SqlResult;

                SqlParameter paramOutIdCatalog = new SqlParameter();
                paramOutIdCatalog.ParameterName = "@IdCatalog";
                paramOutIdCatalog.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdCatalog.Direction = System.Data.ParameterDirection.InputOutput;
                paramOutIdCatalog.Value = objCatalog.IdCatalog;

                SqlResultCatalog = db.Database.ExecuteSqlCommand("GNTranCatalog @TransactionType, @IdCatalog OUT, @IdTable, @Description ",
                        new SqlParameter[]{
                            new SqlParameter("@TransactionType", TransactionType),
                            paramOutIdCatalog,
                            new SqlParameter("@IdTable", objCatalog.IdTable.ToUpper()),
                            new SqlParameter("@Description", objCatalog.Description)
                        }
                    );

                IdCatalog = Int32.Parse(paramOutIdCatalog.Value.ToString());

                if (IdCatalog != 0)
                {
                    if (objCatalog.TableDetails != null && objCatalog.TableDetails.Count()>0)
                    {
                        foreach (var item in objCatalog.TableDetails)
                        {

                            SqlParameter paramOutIdCatalogDetail = new SqlParameter();
                            paramOutIdCatalogDetail.ParameterName = "@IdCatalogDetail";
                            paramOutIdCatalogDetail.SqlDbType = System.Data.SqlDbType.Int;
                            paramOutIdCatalogDetail.Direction = System.Data.ParameterDirection.InputOutput;
                            paramOutIdCatalogDetail.Value = item.IdCatalogDetail;

                            SqlParameter paramDescription = new SqlParameter();
                            paramDescription.ParameterName = "@Description";
                            if (item.Description != null)
                            {
                                paramDescription.Value = item.Description;
                            }
                            else
                            {
                                paramDescription.Value = DBNull.Value;
                            }

                            SqlParameter paramIdTableDetail = new SqlParameter();
                            paramIdTableDetail.ParameterName = "@IdTableDetail";
                            //paramIdTableDetail.SqlDbType = System.Data.SqlDbType.Int;
                            paramIdTableDetail.Direction = System.Data.ParameterDirection.Input;
                            paramIdTableDetail.IsNullable = true;
                            if (!string.IsNullOrEmpty(item.IdTableDetail))
                            {
                                paramIdTableDetail.Value = item.IdTableDetail.ToUpper();
                            }
                            else
                            {
                                paramIdTableDetail.Value = DBNull.Value;
                            }

                            bool blStatus = TransactionType == "I" ? true : item.Status;
                            int intIdCatalog = TransactionType == "I" ? IdCatalog : item.IdCatalog;

                            SqlResult = db.Database.ExecuteSqlCommand("GNTranCatalogDetail @TransactionType, @IdCatalogDetail OUT, @IdTableDetail " +
                                                                      ", @IdCatalog, @Description, @Status ",
                                new SqlParameter[]{
                                    new SqlParameter("@TransactionType", TransactionType),
                                    paramOutIdCatalogDetail,
                                    paramIdTableDetail,
                                    new SqlParameter("@IdCatalog", intIdCatalog),
                                    paramDescription,
                                    new SqlParameter("@Status", blStatus)
                                }
                            );
                            IdCatalogDetail = Int32.Parse(paramOutIdCatalogDetail.Value.ToString());
                        }
                        
                    }

                    Mensaje = "Datos grabados exitosamente.";
                }
                else
                {
                    Mensaje = "No se pudo realizar la transaccion, intente nuevamente.";
                }

                return SqlResultCatalog;

            }
            catch (SqlException ex)
            {
                Mensaje = "Error al grabar datos: " + ex.Message;
                return 0;
            }
        }


        // GET: TableCatalog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


    }
}
