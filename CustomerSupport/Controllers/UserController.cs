using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerSupport.BDContext;
using CustomerSupport.Models;

namespace CustomerSupport.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult ListUser()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");

            }
            else
            {
                var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
                var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListUser").First();
                if (ObjAcces != null)
                {
                    if (ObjAcces.Visible == false)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                MUser objUser = new MUser();
                return View(objUser);
            }
        }

        [HttpPost]
        public ActionResult ListUser(string submit, MUser objMUser)
        {
            if (objMUser==null || objMUser.IdUser==0) 
            {
                return View();
            }

            TempData["DataUser"] = objMUser;

            switch (submit)
            {
                case "searchRow":
                    return RedirectToAction("DetailUser", "User");
                case "editRow":
                    return RedirectToAction("EditUser", "User");
                default:
                    return View();
            }

        }

        public ActionResult GetListUser()
        {            
            List<MUser> ListUser = new List<MUser>();

            ListUser = fnListUser();

            return Json(ListUser, JsonRequestBehavior.AllowGet); 
            
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(MUser objUser)
        {
            //Session.Timeout = 30; //Se coloco en el web config
            try
            {
                //objUser.IdPerson = 1;

                if (string.IsNullOrEmpty(objUser.Login) || string.IsNullOrEmpty(objUser.Password))
                {
                    return View(objUser);
                }

                MMEnterprisesEntities db = new MMEnterprisesEntities();
                MUser OUser = new MUser();

                int SqlResult;

                SqlParameter paramOutIdUsuario = new SqlParameter();
                paramOutIdUsuario.ParameterName = "@IdUser";
                paramOutIdUsuario.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdUsuario.Direction = System.Data.ParameterDirection.Output;
                paramOutIdUsuario.Value = objUser.IdUser;

                SqlResult = db.Database.ExecuteSqlCommand("GNAuthenticationUser @strLogin, @strPassword, @IdUser OUT ",
                        new SqlParameter[]{
                                new SqlParameter("@strLogin",objUser.Login),
                                new SqlParameter("@strPassword", OUser.Encriptar(objUser.Password)),
                                paramOutIdUsuario
                        }
                    );

                if(SqlResult!=0) 
                {

                    int IdUser = Int32.Parse(paramOutIdUsuario.Value.ToString());
                    if (IdUser != 0)
                    {

                        MUser ObjUser = new MUser();

                        ObjUser = fnSearchUserSession(IdUser);

                        Session["Usuario"] = ObjUser;

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ViewBag.ErrorSave = "Error al Autenticar";
                        return View(objUser);
                    }

                }
                else
                {
                    ViewBag.ErrorSave = "Error al Autenticar";
                    return View(objUser);
                }

            }
            catch (Exception e)
            {
                ViewBag.ErrorSave = e.Message;
                return View(objUser);
            }

        }

        public ActionResult DetailUser(int? id) //string id / MUser objMUser / MParameterUrl objMParameter
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
                var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListUser").First();
                if (ObjAcces != null)
                {
                    if (ObjAcces.Search == false)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
                if (TempData["DataUser"] != null) 
                {
                    if (((MUser)TempData["DataUser"]) != null && ((MUser)TempData["DataUser"]).IdUser > 0)
                    {
                        id = ((MUser)TempData["DataUser"]).IdUser;
                    }
                    else 
                    {
                        return RedirectToAction("ListUser", "User");
                    }
                }
                if (id == null)
                {
                    return RedirectToAction("ListUser", "User");
                }
                //-----------------------------------------------------    

                MUser ObjUser = new MUser();
                ObjUser = fnSearchUser(id);

                return View(ObjUser);
            }

        }
        // GET: User/Create
        public ActionResult AddUser()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
                var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListUser").First();
                if (ObjAcces != null)
                {
                    if (ObjAcces.Create == false)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                MUser ObjUser = new MUser();
                MMEnterprisesEntities db = new MMEnterprisesEntities();

                ObjUser.UserAccesPadre = (from result3 in db.GNListUserAcces(null, null, null).ToList()
                                          select new MUserAcces
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

                ObjUser.UserAcces = new List<MUserAcces>();
                ObjUser.Roles = new List<MRole>();

                if (TempData["Success"] != null)
                    ViewBag.SuccessSave = TempData["Success"];

                return View(ObjUser);
            }
        }

        [HttpPost]
        public ActionResult AddUser(MUser objUser)
        {
            string mensaje="";
            try
            {
                if (objUser.UserAcces == null)
                {
                    objUser.UserAcces = new List<MUserAcces>();
                }

                if (ModelState.IsValid)
                {
                    objUser.Status = true;
                    int IdUser = funGNTranuser(objUser, "I", ref mensaje);

                    if (IdUser > 0)
                    { 
                        TempData["Success"] = "Datos grabados exitosamente, Código de Usuario generado: (" + IdUser + ").";
                        return RedirectToAction("AddUser");
                    }
                    else
                    {
                        ViewBag.ErrorSave = "Error al grabar datos del Usuario " + (mensaje==""?"": ": " + mensaje);
                        return View(objUser);
                    }

                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objUser);
                }

            }
            catch (SqlException ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos del Usuario: " + ex.Message;
                return View(objUser);
            }

        }

        // GET: User/Edit/5int id
        public ActionResult EditUser(int? id) //string id / MUser objMuser, string id / FormCollection collection
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
                var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListUser").First();
                if (ObjAcces != null)
                {
                    if (ObjAcces.Edit == false)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
                if (TempData["DataUser"] != null)
                {
                    if (((MUser)TempData["DataUser"]) != null && ((MUser)TempData["DataUser"]).IdUser > 0)
                    {
                        id = ((MUser)TempData["DataUser"]).IdUser;
                    }
                    else
                    {
                        return RedirectToAction("ListUser", "User");
                    }
                }
                if (id == null)
                {
                    return RedirectToAction("ListUser", "User");
                }
                //-----------------------------------------------------

                MUser ObjUser = new MUser();
                ObjUser = fnSearchUser(id);

                if (TempData["Success"] != null)
                    ViewBag.SuccessSave = TempData["Success"];

                return View(ObjUser);
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult EditUser(MUser objUser)
        {
            string mensaje = "";
            try
            {
                if (objUser.UserAcces == null)
                {
                    objUser.UserAcces = new List<MUserAcces>();
                }

                if (ModelState.IsValid)
                {
                    int IdUser = funGNTranuser(objUser, "U", ref mensaje);

                    if (IdUser > 0)
                    {

                        TempData["Success"] = "Datos grabados exitosamente, Código de Usuario: (" + IdUser + ").";

                        //Para evitar que se vea el id en la Url------------
                        MUser objMUser = new MUser();
                        objMUser.IdUser = objUser.IdUser;
                        TempData["DataUser"] = objMUser;
                        return RedirectToAction("EditUser");
                        //---------------------------------------------------
                    }
                    else
                    {
                        ViewBag.ErrorSave = "Error al grabar datos del Usuario " + (mensaje == "" ? "" : ": " + mensaje);
                        return View(objUser);
                    }

                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objUser);
                }

            }
            catch (SqlException ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos del Usuario: " + ex.Message;
                return View(objUser);
            }
        }

        /// <summary>
        /// Lista usuarios con informacion basica Sin accesos.
        /// Se puede Filtrar por IdUser, Status, IdPerson.
        /// </summary>
        /// <param optional name="IdUser"></param> <param optional name="Status"></param> <param optional name="IdPerson"></param>
        /// <returns>Lista de tipo MUser</returns>
        public List<MUser> fnListUser(int? IdUser = null, bool? Status = null, int? IdPerson = null)
        {
            List<MUser> ListUser = new List<MUser>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser ObjUser = new MUser();

            ListUser = (from result in db.GNListUser(IdUser, Status, IdPerson).ToList()
                        select new MUser
                        {
                            IdUser = result.IdUser,
                            IdPerson = result.IdPerson,
                            Login = result.Login,
                            Status = result.Status,
                            StatusDesc = result.Status == true ? "Activo" : "Inactivo",
                            PersonEmployee = (MPerson)(from result2 in db.GNListPerson(result.IdPerson, null, null, null).ToList()
                                                       select new MPerson
                                                       {
                                                           //IdPerson = result2.IdPerson,
                                                           //IdPersonType = result2.IdPersonType,
                                                           //PersonType = result2.PersonType,
                                                           //IdIdentificationType = result2.IdIdentificationType,
                                                           //IdentificationType = result2.IdentificationType,
                                                           //NumIdentification = ObjUser.Desencriptar(result2.NumIdentification),
                                                           Name = result2.Name,
                                                           LastName = result2.LastName,
                                                           //Birthday = result2.Birthday,
                                                           //Address = ObjUser.Desencriptar(result2.Address),
                                                           //Email = result2.Email,
                                                           //IdContactType = result2.IdContactType,
                                                           //ContactType = result2.ContactType,
                                                           //IdPosition = result2.IdPosition,
                                                           Position = result2.Position //,
                                                           //ClientPermission = result2.ClientPermission,
                                                           //Status = result2.Status
                                                       }).ToList().First(),
                            UserRolesNames = String.Join(",", db.GNListUserRole(null, result.IdUser).Select(c => c.NameRole).ToList())
                            
                                                  
                        }).ToList();

            return ListUser;

        }

        /// <summary>
        /// Busca informacion completa de un usuario Con sus accesos.
        /// Se puede buscar Usuario por IdUser o por IdPerson
        /// </summary>
        /// <param optional name="IdUser"></param> <param optional name="Status"></param> <param optional name="IdPerson"></param>
        /// <returns>Objeto de tipo MUser</returns>
        public MUser fnSearchUser(int? IdUser = null, bool? Status = null, int? IdPerson = null)
        {
            MUser objMUser = new MUser();
            MMEnterprisesEntities db = new MMEnterprisesEntities();
            string listIdRoles = String.Join(",", db.GNListUserRole(null, IdUser).Select(c => c.IdRole).ToList());

            objMUser = (from result in db.GNListUser(IdUser, Status, IdPerson).ToList()
                        select new MUser
                        {
                            IdUser = result.IdUser,
                            IdPerson = result.IdPerson,
                            Login = result.Login,                            
                            Password = objMUser.Desencriptar(result.Password),
                            Status = result.Status,
                            StatusDesc = result.Status == true ? "Activo" : "Inactivo",
                            PersonEmployee = (MPerson)(from result2 in db.GNListPerson(result.IdPerson, null, null, null).ToList()
                                                       select new MPerson
                                                       {
                                                           IdPerson = result2.IdPerson,
                                                           IdPersonType = result2.IdPersonType,
                                                           PersonType = result2.PersonType,
                                                           IdIdentificationType = result2.IdIdentificationType,
                                                           IdentificationType = result2.IdentificationType,
                                                           NumIdentification = objMUser.Desencriptar(result2.NumIdentification),
                                                           Name = result2.Name,
                                                           LastName = result2.LastName,
                                                           Birthday = result2.Birthday,
                                                           Address = objMUser.Desencriptar(result2.Address),
                                                           Email = result2.Email,
                                                           IdContactType = result2.IdContactType,
                                                           ContactType = result2.ContactType,
                                                           IdPosition = result2.IdPosition,
                                                           Position = result2.Position,
                                                           ClientPermission = result2.ClientPermission,
                                                           Status = result2.Status
                                                       }).ToList().First(),
                            Roles = (from resultRoles in db.GNListUserRole(null, result.IdUser).ToList()
                                           select new MRole 
                                           {
                                               IdRole=resultRoles.IdRole,
                                               NameRole=resultRoles.NameRole
                                           }).ToList(),
                            UserAcces = (from result3 in db.GNListUserAcces(result.IdUser, null, listIdRoles).ToList()
                                         select new MUserAcces
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

                            UserAccesPadre = (from result3 in db.GNListUserAcces(null, null, null).ToList()
                                              select new MUserAcces
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
                        }).First();

            return objMUser;

        }

        /// <summary>
        /// Busca informacion basica minima de un usuario Con sus accesos.
        /// Se puede buscar Usuario por IdUser o por IdPerson
        /// </summary>
        /// <param optional name="IdUser"></param> <param optional name="Status"></param> <param optional name="IdPerson"></param>
        /// <returns>Objeto de tipo MUser</returns>
        public static MUser fnSearchUserSession(int? IdUser = null, bool? Status = null, int? IdPerson = null)
        {
            MUser objMUser = new MUser();
            MMEnterprisesEntities db = new MMEnterprisesEntities();
            
            objMUser = (from result in db.GNListUser(IdUser, Status, IdPerson).ToList()
                        select new MUser
                        {
                            IdUser = result.IdUser,
                            Login = result.Login,
                            PersonEmployee = (MPerson)(from result2 in db.GNListPerson(result.IdPerson, null, null, null).ToList()
                                                       select new MPerson
                                                       {
                                                           IdPerson = result2.IdPerson,
                                                           Name = result2.Name,
                                                           LastName = result2.LastName,                                                           
                                                           Email = result2.Email
                                                       }).ToList().First(),                            
                            UserAcces = (from result3 in db.GNListRoleUserPermission(result.IdUser, null).ToList()
                                         select new MUserAcces
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

                            UserAccesPadre = (from result3 in db.GNListRoleUserPermission(null, null).ToList()
                                              select new MUserAcces
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
                        }).First();

            return objMUser;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdUser"></param>
        /// /// <param optional name="IdAssociated"></param>
        /// <returns></returns>
        public ActionResult DetailMenuOption(int IdUser, int IdAssociated, string lsRoles=null)
        {
        
            List<MUserAcces> ObjUser = new List<MUserAcces>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ObjUser = (from result3 in db.GNListUserAcces(IdUser, IdAssociated, lsRoles).ToList()
                       select new MUserAcces
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

          return   Json(ObjUser, JsonRequestBehavior.AllowGet);

        }

        private int funGNTranuser(MUser objUser, string TransactionType, ref string Mensaje)
        {
            try
            {
                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int IdUser;
                int SqlResult;

                SqlParameter paramOutIdUsuario = new SqlParameter();
                paramOutIdUsuario.ParameterName = "@IdUser";
                paramOutIdUsuario.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdUsuario.Direction = System.Data.ParameterDirection.InputOutput;
                paramOutIdUsuario.Value = objUser.IdUser;

                SqlResult = db.Database.ExecuteSqlCommand("GNTranUser @IdPerson, @strLogin , @strPassword, @TransactionType, @IdUser OUT, @btStatus ",
                       new SqlParameter[]{
                                new SqlParameter("@TransactionType", TransactionType),
                                paramOutIdUsuario,
                                new SqlParameter("@IdPerson", objUser.IdPerson),
                                new SqlParameter("@strLogin",objUser.Login),
                                new SqlParameter("@strPassword", objUser.Encriptar(objUser.Password)),
                                new SqlParameter("@btStatus", objUser.Status)
                        }
                    );

                IdUser = Int32.Parse(paramOutIdUsuario.Value.ToString());
                if (IdUser != 0)
                {
                    //Accesos por usuario
                    if (objUser.UserAcces != null && objUser.UserAcces.Count() > 0) 
                    {
                        //si va a actualizar, se eliminan los accesos por usuario para volver a insertar
                        if (TransactionType == "U")
                        {
                            SqlResult = db.Database.ExecuteSqlCommand("GNTranUserAcces @TransactionType, @IdUser, @IdOption, @blnVisible " +
                                                                    ", @blnCreate, @blnSearch, @blnEdit, @blnDelete ",
                                        new SqlParameter[]{
                                                new SqlParameter("@TransactionType", TransactionType),
                                                new SqlParameter("@IdUser", IdUser),
                                                new SqlParameter("@IdOption", DBNull.Value),
                                                new SqlParameter("@blnVisible", DBNull.Value),
                                                new SqlParameter("@blnCreate", DBNull.Value),
                                                new SqlParameter("@blnSearch", DBNull.Value),
                                                new SqlParameter("@blnEdit", DBNull.Value),
                                                new SqlParameter("@blnDelete", DBNull.Value)
                                        }
                            );
                        }
                        foreach (var item in objUser.UserAcces)
                        {

                            SqlResult = db.Database.ExecuteSqlCommand("GNTranUserAcces @TransactionType, @IdUser, @IdOption, @blnVisible " +
                                                                        ", @blnCreate, @blnSearch, @blnEdit, @blnDelete ",
                                        new SqlParameter[]{
                                            new SqlParameter("@TransactionType", "I"),
                                            new SqlParameter("@IdUser", IdUser),
                                            new SqlParameter("@IdOption", item.IdOption),
                                            new SqlParameter("@blnVisible", item.Visible),
                                            new SqlParameter("@blnCreate", item.Create),
                                            new SqlParameter("@blnSearch", item.Search),
                                            new SqlParameter("@blnEdit", item.Edit),
                                            new SqlParameter("@blnDelete", item.Delete)
                                        }
                            );
                        }
                    }

                    //Roles del usuario
                    if (objUser.Roles != null && objUser.Roles.Count() > 0) 
                    {
                        //si va a actualizar, se eliminan los roles del usuario para volver a insertar
                        if (TransactionType == "U") 
                        {
                            SqlResult = db.Database.ExecuteSqlCommand("GNTranUserRole @TransactionType, @IdUser, @IdRole ",
                                new SqlParameter[]{
                                        new SqlParameter("@TransactionType", TransactionType),
                                        new SqlParameter("@IdUser", IdUser),
                                        new SqlParameter("@IdRole", DBNull.Value)
                                }
                            );
                        }
                        foreach (var item in objUser.Roles)
                        {
                            SqlResult = db.Database.ExecuteSqlCommand("GNTranUserRole @TransactionType, @IdUser, @IdRole ",
                                new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "I"),
                                        new SqlParameter("@IdUser", IdUser),
                                        new SqlParameter("@IdRole", item.IdRole)
                                }
                            );
                        }
                    }

                }
                return IdUser;
            }
            catch (SqlException ex)
            {
                Mensaje= ex.Message;
                return 0;
            }
        }

        public ActionResult GetListPersonWithoutUser()
        {
            List<MPerson> ListPerson = new List<MPerson>();
            ListPerson = PersonController.fnListPerson(null, 2, null, true);

            List<MUser> ListUser = new List<MUser>();
            ListUser = fnListUser();

            List<MPerson> ListPersonWithoutUser = new List<MPerson>();
            ListPersonWithoutUser = ListPerson.Where(p => ListUser.All(p2 => p2.IdPerson != p.IdPerson)).ToList();

            return Json(ListPersonWithoutUser, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Close()
        {
            Session.RemoveAll();

            return RedirectToAction("Login", "User");
        }
    }
}
