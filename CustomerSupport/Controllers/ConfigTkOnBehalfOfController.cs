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
    public class ConfigTkOnBehalfOfController : Controller
    {

        // GET: ConfigTkOnBehalfOf/ListConfigTkOnBehalfOf/5
        public ActionResult ListConfigTkOnBehalfOf()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListConfigTkOnBehalfOf").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Visible == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var IdUserOnBehalfOf = ((MSerUser)Session["Usuario"]).IdUser;

            List<MConfigTkOnBehalfOf> LstConfigTkOnBehalfOf = new List<MConfigTkOnBehalfOf>();

            LstConfigTkOnBehalfOf = fnListMConfigTkOnBehalfOf(null,IdUserOnBehalfOf,null);

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(LstConfigTkOnBehalfOf);
        }

        // POST: ConfigTkOnBehalfOf/ListConfigTkOnBehalfOf
        [HttpPost]
        public ActionResult ListConfigTkOnBehalfOf(List<MConfigTkOnBehalfOf> ListConfigTkOnBehalfOf)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string mensaje = "";
                    int resultDb = fnGNTranConfigTkOnBehalfOf(ListConfigTkOnBehalfOf, "U", ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;
                        return RedirectToAction("ListConfigTkOnBehalfOf");
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";                    
                }

                //var IdUserOnBehalfOf = ((MSerUser)Session["Usuario"]).IdUser;
                //ListConfigTkOnBehalfOf = new List<MConfigTkOnBehalfOf>();
                //ListConfigTkOnBehalfOf = fnListMConfigTkOnBehalfOf(null, IdUserOnBehalfOf, null);

                if (ListConfigTkOnBehalfOf == null) { ListConfigTkOnBehalfOf = new List<MConfigTkOnBehalfOf>(); }

                return View(ListConfigTkOnBehalfOf);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos: " + ex.Message;
                return View(ListConfigTkOnBehalfOf);
            }

        }

        /// <summary>
        /// Filtrar por "IdUserOnBehalfOf" para la pantalla "ListConfigTkOnBehalfOf" , la tabla se llena con  IdConfig,IdUser,NumIdentification,Name,LastName.
        /// Filtrar por "IdUser" para la pantalla "AddTask" y para llenar el combobox usar IdUserOnBehalfOf,NumIdentificationOnBehalfOf,NameOnBehalfOf,LastNameOnBehalfOf.
        /// </summary>
        public static List<MConfigTkOnBehalfOf> fnListMConfigTkOnBehalfOf(int? IdUser = null, int? IdUserOnBehalfOf = null, int? IdConfig=null)
        {
            List<MConfigTkOnBehalfOf> ListConfigTkOnBehalfOf = new List<MConfigTkOnBehalfOf>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser objUser = new MUser();

            ListConfigTkOnBehalfOf = (from result in db.GNListConfigTkOnBehalfOf(IdUser, IdUserOnBehalfOf, IdConfig).ToList()
                                select new MConfigTkOnBehalfOf
                                {
                                    IdConfig=result.IdConfig,
                                    IdUserOnBehalfOf = result.IdUserOnBehalfOf,
                                    NumIdentificationOnBehalfOf = objUser.Desencriptar(result.NumIdentificationOnBehalfOf),
                                    NameOnBehalfOf = result.NameOnBehalfOf,
                                    LastNameOnBehalfOf = result.LastNameOnBehalfOf,
                                    IdUser = result.IdUser,
                                    NumIdentification = objUser.Desencriptar(result.NumIdentification),
                                    Name = result.Name,
                                    LastName =result.LastName
                                }).ToList();
            
            return ListConfigTkOnBehalfOf;

        }

        public static int fnGNTranConfigTkOnBehalfOf(List<MConfigTkOnBehalfOf> ListConfigTkOnBehalfOf, string TransactionType, ref string Mensaje)
        {
            try
            {
                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int IdConfig=0;
                int SqlResult;

                if (ListConfigTkOnBehalfOf != null && ListConfigTkOnBehalfOf.Count() > 0)
                {
                    //actualizar, se eliminan los registros para volver a insertar
                    if (TransactionType == "U")
                    {
                        SqlResult = db.Database.ExecuteSqlCommand("GNTranConfigTkOnBehalfOf @TransactionType, @IdConfig OUT " +
                                                                  ", @IdUserOnBehalfOf, @IdUser ",
                            new SqlParameter[]{
                                new SqlParameter("@TransactionType", TransactionType),
                                new SqlParameter("@IdConfig", DBNull.Value),
                                new SqlParameter("@IdUserOnBehalfOf", ListConfigTkOnBehalfOf.First().IdUserOnBehalfOf),
                                new SqlParameter("@IdUser", DBNull.Value)
                            }
                        );
                    }

                    foreach (var item in ListConfigTkOnBehalfOf)
                    {

                        if (item.Status!=false)
                        {
                            SqlParameter paramOutIdConfig = new SqlParameter();
                            paramOutIdConfig.ParameterName = "@IdConfig";
                            paramOutIdConfig.SqlDbType = System.Data.SqlDbType.Int;
                            paramOutIdConfig.Direction = System.Data.ParameterDirection.InputOutput;
                            paramOutIdConfig.Value = item.IdConfig;

                            SqlResult = db.Database.ExecuteSqlCommand("GNTranConfigTkOnBehalfOf @TransactionType, @IdConfig OUT " +
                                                                      ", @IdUserOnBehalfOf, @IdUser ",
                                new SqlParameter[]{
                                new SqlParameter("@TransactionType", "I"),
                                paramOutIdConfig,
                                new SqlParameter("@IdUserOnBehalfOf", item.IdUserOnBehalfOf),
                                new SqlParameter("@IdUser", item.IdUser)
                                }
                            );
                            IdConfig = Int32.Parse(paramOutIdConfig.Value.ToString());
                        }
                    }
                    Mensaje = "Datos grabados exitosamente.";
                }

                return IdConfig;

            }
            catch (SqlException ex)
            {
                Mensaje = "Error al grabar datos: " + ex.Message;
                return 0;
            }
        }

        public ActionResult GetConfigTkOnBehalfOf(int IdUser)
        {
            object ListMConfigTkOnBehalfOf;
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ListMConfigTkOnBehalfOf = fnListMConfigTkOnBehalfOf(IdUser,null,null).ToList();

            return Json(ListMConfigTkOnBehalfOf, JsonRequestBehavior.AllowGet);

        }

    }
}
