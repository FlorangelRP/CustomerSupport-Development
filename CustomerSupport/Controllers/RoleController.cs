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
    public class RoleController : Controller
    {
        // GET: Client
        [HttpGet]
        public ActionResult ListRole()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListRole").First();
            if(ObjAcces!=null)
            {
                if(ObjAcces.Visible ==false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            MRole objMRole = new MRole();
            return View(objMRole);
        }

        [HttpPost]
        public ActionResult ListRole(string submit, MRole objMRole)
        {
            if (objMRole == null || objMRole.IdRole == 0)
            {
                return View();
            }

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objMRole);
            TempData["DataRole"] = jsonString;

            switch (submit)
            {
                case "searchRow":
                    return RedirectToAction("DetailRole", "Role");
                case "editRow":
                    return RedirectToAction("EditRole", "Role");
                default:
                    return View();
            }

        }

        public ActionResult GetListRole()
        {
            List<MRole> ListRole = new List<MRole>();
            ListRole = fnListRole();

            return Json(ListRole, JsonRequestBehavior.AllowGet); 
        }

        // GET: Role/DetailRole/5
        public ActionResult DetailRole(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListRole").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Search == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataRole"] != null)
            {
                var objTempData = Newtonsoft.Json.JsonConvert.DeserializeObject<MRole>((string)TempData["DataRole"]);
                if (objTempData != null && objTempData.IdRole > 0)
                {
                    id = objTempData.IdRole;
                }
                else
                {
                    return RedirectToAction("ListRole", "Role");
                }
            }
            if (id == null) 
            {
                return RedirectToAction("ListRole", "Role");
            }
            //-----------------------------------------------------

            MRole objRole = new MRole();
            objRole = fnListRole(id).First();
            return View(objRole);
        }

        // GET: Role/AddRole
        public ActionResult AddRole()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListRole").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Create == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            MRole objRole = new MRole();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            objRole.RoleAccesPadre = (from result3 in db.GNListRoleAcces(null, null).ToList()
                                      select new MRoleAcces
                                      {
                                          IdOption = result3.IdOption,
                                          OptionName = result3.OptionName,
                                          Visible = result3.Visible == null ? false : (bool)result3.Visible,
                                          Create = result3.Create == null ? false : (bool)result3.Create,
                                          Search = result3.Search == null ? false : (bool)result3.Search,
                                          Edit = result3.Edit == null ? false : (bool)result3.Edit,
                                          Delete = result3.Edit == null ? false : (bool)result3.Delete,
                                          IdAssociated = result3.IdAssociated
                                      }).ToList();

            objRole.RoleAcces = new List<MRoleAcces>();

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objRole);
        }

        // POST: Role/AddRole
        [HttpPost]
        public ActionResult AddRole(MRole objRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //valores por defecto
                    objRole.Status = true; //activo

                    string mensaje = "";
                    int resultDb = fnGNTranRole(objRole, "I", ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;
                        return RedirectToAction("AddRole");
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objRole);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objRole);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos del rol: " + ex.Message;
                return View(objRole);
            }
        }


        // GET: Role/EditRole/5
        public ActionResult EditRole(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListRole").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Edit == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataRole"] != null)
            {
                var objTempData = Newtonsoft.Json.JsonConvert.DeserializeObject<MRole>((string)TempData["DataRole"]);
                if (objTempData != null && objTempData.IdRole > 0)
                {
                    id = objTempData.IdRole;
                }
                else
                {
                    return RedirectToAction("ListRole", "Role");
                }
            }
            if (id == null)
            {
                return RedirectToAction("ListRole", "Role");
            }
            //-----------------------------------------------------

            MRole objRole = new MRole();
            objRole = fnListRole(id).First();

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objRole);
        }

        // POST: Role/EditRole/5
        [HttpPost]
        public ActionResult EditRole(MRole objRole)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string mensaje = "";
                    int resultDb = fnGNTranRole(objRole, "U", ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;

                        //Para evitar que se vea el id en la Url------------
                        MRole objMRole = new MRole();
                        objMRole.IdRole = objRole.IdRole;

                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objMRole);
                        TempData["DataRole"] = jsonString;
                        return RedirectToAction("EditRole");
                        //---------------------------------------------------
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objRole);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objRole);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos del Rol: " + ex.Message;
                return View(objRole);
            }

        }

        public ActionResult DetailMenuOption(int IdRole, int IdAssociated)
        {

            List<MRoleAcces> ObjRoleAcces = new List<MRoleAcces>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ObjRoleAcces = (from result3 in db.GNListRoleAcces(IdRole, IdAssociated).ToList()
                       select new MRoleAcces
                       {
                           IdOption = result3.IdOption,
                           OptionName = result3.OptionName,
                           Visible = result3.Visible == null ? false : (bool)result3.Visible,
                           Create = result3.Create == null ? false : (bool)result3.Create,
                           Search = result3.Search == null ? false : (bool)result3.Search,
                           Edit = result3.Edit == null ? false : (bool)result3.Edit,
                           Delete = result3.Edit == null ? false : (bool)result3.Delete,
                           IdAssociated = result3.IdAssociated
                       }).ToList();

            return Json(ObjRoleAcces, JsonRequestBehavior.AllowGet);

        }

        public static List<MRole> fnListRole(int? IdRole=null, bool? Status=null)
        {
            List<MRole> ListRole = new List<MRole>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ListRole = (from result in db.GNListRole(IdRole, Status).ToList()
                          select new MRole
                          {
                              IdRole=result.IdRole,
                              NameRole=result.NameRole,
                              Status=result.Status,
                              StatusDesc = result.Status == true ? "Activo" : "Inactivo",
                              RoleAcces = (from result3 in db.GNListRoleAcces(result.IdRole, null).ToList()
                                           select new MRoleAcces
                                           {
                                               IdOption = result3.IdOption,
                                               OptionName = result3.OptionName,
                                               Visible = result3.Visible == null ? false : (bool)result3.Visible,
                                               Create = result3.Create == null ? false : (bool)result3.Create,
                                               Search = result3.Search == null ? false : (bool)result3.Search,
                                               Edit = result3.Edit == null ? false : (bool)result3.Edit,
                                               Delete = result3.Edit == null ? false : (bool)result3.Delete,
                                               IdAssociated = result3.IdAssociated,
                                               Action = result3.Action,
                                               Controller = result3.Controller
                                           }).ToList(),
                              RoleAccesPadre = (from result3 in db.GNListRoleAcces(null, null).ToList()
                                                select new MRoleAcces
                                                {
                                                    IdOption = result3.IdOption,
                                                    OptionName = result3.OptionName,
                                                    Visible = result3.Visible == null ? false : (bool)result3.Visible,
                                                    Create = result3.Create == null ? false : (bool)result3.Create,
                                                    Search = result3.Search == null ? false : (bool)result3.Search,
                                                    Edit = result3.Edit == null ? false : (bool)result3.Edit,
                                                    Delete = result3.Edit == null ? false : (bool)result3.Delete,
                                                    IdAssociated = result3.IdAssociated,
                                                    Action = result3.Action,
                                                    Controller = result3.Controller
                                                }).ToList()
                        }).ToList();

            return ListRole;

        }

        public static int fnGNTranRole(MRole objRole, string TransactionType, ref string Mensaje)
        {
            try
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo; //para capitalizar textos

                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int IdRole;
                int SqlResult;

                SqlParameter paramOutIdRole = new SqlParameter();
                paramOutIdRole.ParameterName = "@IdRole";
                paramOutIdRole.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdRole.Direction = System.Data.ParameterDirection.InputOutput;
                paramOutIdRole.Value = objRole.IdRole;

                SqlResult = db.Database.ExecuteSqlCommand("GNTranRole @TransactionType, @IdRole OUT, @NameRole, @Status ",
                                new SqlParameter[]{
                                    new SqlParameter("@TransactionType", TransactionType),
                                    paramOutIdRole,
                                    new SqlParameter("@NameRole", ti.ToTitleCase(objRole.NameRole)),
                                    new SqlParameter("@Status", objRole.Status)
                                }
                            );

                IdRole = Int32.Parse(paramOutIdRole.Value.ToString());

                if (IdRole != 0)
                {
                    foreach (var item in objRole.RoleAcces)
                    {

                        SqlResult = db.Database.ExecuteSqlCommand("GNTranRoleAcces @IdRole, @IdOption, @blnVisible " +
                                                                  ", @blnCreate, @blnSearch, @blnEdit, @blnDelete ",
                                        new SqlParameter[]{
                                            new SqlParameter("@IdRole", IdRole),
                                            new SqlParameter("@IdOption", item.IdOption),
                                            new SqlParameter("@blnVisible", item.Visible),
                                            new SqlParameter("@blnCreate", item.Create),
                                            new SqlParameter("@blnSearch", item.Search),
                                            new SqlParameter("@blnEdit", item.Edit),
                                            new SqlParameter("@blnDelete", item.Delete)
                                        }
                                    );
                    }

                    //Mensaje = "Datos grabados exitosamente para el Código de empleado: (" + IdPerson + ").";
                    Mensaje = "Datos grabados exitosamente.";
                }
                else
                {
                    Mensaje = "No se pudo realizar la transaccion, intente nuevamente.";
                }

                return SqlResult;

            }
            catch (SqlException ex)
            {
                Mensaje = "Error al grabar datos: " + ex.Message;
                return 0;
            }
        }


    }
}
