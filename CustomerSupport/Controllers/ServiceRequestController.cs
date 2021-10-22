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
    public class ServiceRequestController : Controller
    {
        // GET: ServiceRequest
        [HttpGet]
        public ActionResult ListServiceRequest()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListServiceRequest").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Visible == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            MServiceRequest objMServiceRequest = new MServiceRequest();
            return View(objMServiceRequest);
        }

        [HttpPost]
        public ActionResult ListServiceRequest(string submit, MServiceRequest objMServiceRequest)
        {
            if (objMServiceRequest == null || objMServiceRequest.IdServiceRequest == 0)
            {
                return View();
            }

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objMServiceRequest);
            TempData["DataServiceRequest"] = jsonString;

            switch (submit)
            {
                case "searchRow":
                    return RedirectToAction("DetailServiceRequest", "ServiceRequest");
                case "editRow":
                    return RedirectToAction("EditServiceRequest", "ServiceRequest");
                default:
                    return View();
            }

        }
        public ActionResult GetListServiceRequest()
        {

            List<MServiceRequest> ListServiceRequest = new List<MServiceRequest>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ListServiceRequest = fnListServiceRequest(null, null,null,null,null);

            return Json(ListServiceRequest, JsonRequestBehavior.AllowGet);

        }

        // GET: ServiceRequest/AddServiceRequest
        public ActionResult AddServiceRequest()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListServiceRequest").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Create == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            MServiceRequest objServiceRequest = new MServiceRequest();
            objServiceRequest.listConstructionOption = new List<MServiceConstructionOption>();

            objServiceRequest.listTask = new List<MTask>();
            objServiceRequest.listTask.Add(new MTask());
            objServiceRequest.listTask[0].DateIni = DateTime.Now.Date;


            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objServiceRequest);
        }

        // POST: ServiceRequest/AddServiceRequest
        [HttpPost]
        public ActionResult AddServiceRequest(MServiceRequest objServiceRequest)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //valores por defecto
                    objServiceRequest.IdUser = ((MSerUser)Session["Usuario"]).IdUser;
                    objServiceRequest.listTask.RemoveAll(r => r.IdPersonEmployee == null); //si empleado esta null, no hay cita
                    //Hay servicios que no requieren propiedad
                    if (objServiceRequest.IdPropertyType != null && objServiceRequest.IdPropertyType == 0)
                    {
                        objServiceRequest.IdPropertyType = null;
                    }
                    //------------------

                    string mensaje = "";
                    int IdService = 0;
                    int resultDb = fnGNTranServiceRequest(objServiceRequest, "I", ref IdService, ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje + " N° de Servicio generado: " + IdService + ".";
                        return RedirectToAction("AddServiceRequest");
                    }
                    else
                    {

                        if (objServiceRequest.listTask != null) 
                        {
                            if (objServiceRequest.listTask.Count()==0)
                            {
                                objServiceRequest.listTask.Add(new MTask());
                                objServiceRequest.listTask[0].DateIni = DateTime.Now.Date;
                            }
                        }

                        ViewBag.ErrorSave = mensaje;
                        return View(objServiceRequest);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objServiceRequest);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos de la solicitud de servicio: " + ex.Message;
                return View(objServiceRequest);
            }

        }

        // GET: ServiceRequest/EditServiceRequest/5
        public ActionResult EditServiceRequest(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListServiceRequest").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Edit == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataServiceRequest"] != null)
            {
                var objTempData = Newtonsoft.Json.JsonConvert.DeserializeObject<MServiceRequest>((string)TempData["DataServiceRequest"]);
                if (objTempData != null && objTempData.IdServiceRequest > 0)
                {
                    id = objTempData.IdServiceRequest;
                }
                else
                {
                    return RedirectToAction("ListServiceRequest", "ServiceRequest");
                }
            }
            if (id == null)
            {
                return RedirectToAction("ListServiceRequest", "ServiceRequest");
            }
            //-----------------------------------------------------

            MServiceRequest objServiceRequest = new MServiceRequest();
            objServiceRequest = fnListServiceRequest(id,null).First();

            if (objServiceRequest.listConstructionOption == null) 
            {
                objServiceRequest.listConstructionOption = new List<MServiceConstructionOption>();
            }            

            //----
            if (objServiceRequest.listTask!=null && objServiceRequest.listTask.Count()>0) 
            {
                if (objServiceRequest.listTask[0].listTaskPerson != null && objServiceRequest.listTask[0].listTaskPerson.Count() > 0)
                {
                    objServiceRequest.listTask[0].IdPersonEmployee = objServiceRequest.listTask[0].listTaskPerson[0].IdPersonEmployee;
                    objServiceRequest.listTask[0].PersonEmployeeName = objServiceRequest.listTask[0].listTaskPerson[0].PersonEmployeeName;
                    objServiceRequest.listTask[0].PersonEmployeeLastName = objServiceRequest.listTask[0].listTaskPerson[0].PersonEmployeeLastName;
                }
            }
            else
            {
                objServiceRequest.listTask = new List<MTask>();
                objServiceRequest.listTask.Add(new MTask());
                objServiceRequest.listTask[0].DateIni = DateTime.Now.Date;
            }
            //----

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objServiceRequest);
        }

        // POST: ServiceRequest/EditServiceRequest
        [HttpPost]
        public ActionResult EditServiceRequest(MServiceRequest objServiceRequest)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //valores por defecto
                    objServiceRequest.IdUser = ((MSerUser)Session["Usuario"]).IdUser;
                    objServiceRequest.listTask.RemoveAll(r => r.IdPersonEmployee == null); //si empleado esta null, no hay cita
                    //Hay servicios que no requieren propiedad
                    if (objServiceRequest.IdPropertyType!=null && objServiceRequest.IdPropertyType == 0) 
                    {
                        objServiceRequest.IdPropertyType = null;
                    }
                    //------------------

                    string mensaje = "";
                    int IdService = 0;
                    int resultDb = fnGNTranServiceRequest(objServiceRequest, "U", ref IdService, ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;

                        //Para evitar que se vea el id en la Url------------
                        MServiceRequest objMServiceRequest = new MServiceRequest();
                        objMServiceRequest.IdServiceRequest = objServiceRequest.IdServiceRequest;

                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objMServiceRequest);
                        TempData["DataServiceRequest"] = jsonString;
                        return RedirectToAction("EditServiceRequest");
                        //---------------------------------------------------

                        //return RedirectToAction("EditServiceRequest", new { id = objServiceRequest.IdServiceRequest });
                    }
                    else
                    {

                        if (objServiceRequest.listTask != null)
                        {
                            if (objServiceRequest.listTask.Count() == 0)
                            {
                                objServiceRequest.listTask.Add(new MTask());
                                objServiceRequest.listTask[0].DateIni = DateTime.Now.Date;
                            }
                        }

                        ViewBag.ErrorSave = mensaje;
                        return View(objServiceRequest);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objServiceRequest);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos de la solicitud de servicio: " + ex.Message;
                return View(objServiceRequest);
            }

        }

        public static List<MServiceRequest> fnListServiceRequest(int? IdServiceRequest, int? IdServiceType, int? IdServiceStatus = null, int? IdPerson = null, int? IdUser = null)
        {
            List<MServiceRequest> ListServiceRequest = new List<MServiceRequest>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser objUser = new MUser();

            ListServiceRequest = (from d in db.GNListServiceRequest(IdServiceRequest, IdServiceType, IdServiceStatus, IdPerson, IdUser).ToList()
                                    select new MServiceRequest
                                    {
                                        IdServiceRequest = d.IdServiceRequest,
                                        IdServiceType = d.IdServiceType,
                                        ServiceType = d.ServiceType,
                                        IdServiceStatus = d.IdServiceStatus,
                                        ServiceStatus = d.ServiceStatus,
                                        IdPerson = d.IdPerson,
                                        PersonClient = (MPerson)(from result2 in db.GNListPerson(d.IdPerson, null, null, null,null).ToList()
                                                        select new MPerson
                                                        {
                                                            IdPerson = result2.IdPerson,
                                                            IdPersonType = result2.IdPersonType,
                                                            PersonType = result2.PersonType,
                                                            IdIdentificationType = result2.IdIdentificationType,
                                                            IdentificationType = result2.IdentificationType,
                                                            NumIdentification = objUser.Desencriptar(result2.NumIdentification),
                                                            Name = result2.Name,
                                                            LastName = result2.LastName,
                                                            Birthday = result2.Birthday,
                                                            Address = objUser.Desencriptar(result2.Address),
                                                            Email = result2.Email,
                                                            IdContactType = result2.IdContactType,
                                                            ContactType = result2.ContactType,
                                                            IdPosition = result2.IdPosition,
                                                            Position = result2.Position,
                                                            ClientPermission = result2.ClientPermission,
                                                            Status = result2.Status
                                                        }).ToList().First(),
                                        IdContactType=d.IdContactType,
                                        ContactType=d.ContactType,
                                        IdPropertyType=d.IdPropertyType,
                                        PropertyType=d.PropertyType,
                                        Address=d.Address,
                                        Price=d.Price,
                                        DownPayment=d.DownPayment,
                                        ClosingCost=d.ClosingCost,
                                        MonthlyIncome=d.MonthlyIncome,
                                        DebtPayment=d.DebtPayment,
                                        Piti=d.Piti,
                                        Ratios=d.Ratios,
                                        EstimatedValue=d.EstimatedValue,
                                        LoanAmount=d.LoanAmount,
                                        CurrentDebt=d.CurrentDebt,
                                        Assets=d.Assets,
                                        Beneficiaries=d.Beneficiaries,
                                        Process=d.Process,
                                        Wish=d.Wish,
                                        Plane=d.Plane,
                                        Financing=d.Financing,
                                        Note=d.Note,
                                        IdUser=d.IdUser,
                                        RegisterUser=d.RegisterUser,
                                        RegisterDate = d.RegisterDate,
                                        listConstructionOption = (List<MServiceConstructionOption>)(from co in db.GNListServiceConstructionOption(d.IdServiceRequest, null).ToList()
                                                                    select new MServiceConstructionOption
                                                                    {
                                                                        IdServiceRequest=co.IdServiceRequest,
                                                                        IdConstructionOption=co.IdConstructionOption,
                                                                        ConstructionOption=co.ConstructionOption
                                                                    }).ToList(),
                                        listTask = (List<MTask>)(from tsk in db.GNListTask(null,null,null,null,"",null,null,null,d.IdServiceRequest,null,null).ToList()
                                                    select new MTask
                                                    {
                                                        IdTask = tsk.IdTask,
                                                        IdUser = tsk.IdUser,
                                                        UserName = tsk.UserName,
                                                        UserLastName = tsk.UserLastName,
                                                        DateIni = tsk.DateIni,
                                                        DateEnd = tsk.DateEnd,
                                                        HourIni = tsk.HourIni,
                                                        HourEnd = tsk.HourEnd,
                                                        Place = tsk.Place,
                                                        Status = tsk.Status,
                                                        IdStatus = tsk.IdStatus,
                                                        IdFatherTask = tsk.IdFatherTask,
                                                        Tittle = tsk.Tittle,
                                                        IdServiceRequest = tsk.IdServiceRequest,
                                                        IdTypeTask = tsk.IdTypeTask,
                                                        TypeTask = tsk.TypeTask,
                                                        IdPriority = tsk.IdPriority,
                                                        PriorityTask = tsk.PriorityTask,
                                                        IdPersonEmployee = tsk.IdResponsable,
                                                        PersonEmployeeName = tsk.Name,
                                                        PersonEmployeeLastName = tsk.LastName,
                                                        listTaskPerson =(List<MTaskPerson>)(from tp in db.GNListPersonTask(tsk.IdTask, null).ToList()
                                                                        select new MTaskPerson
                                                                        {
                                                                            IdPersonEmployee=tp.IdPersonEmployee,
                                                                            PersonEmployeeName=tp.PersonEmployeeName,
                                                                            PersonEmployeeLastName=tp.PersonEmployeeLastName,
                                                                            NumIdentification = tp.NumIdentification
                                                                        }).ToList()
                                                    }).ToList()
                                    }).ToList();

            return ListServiceRequest;

        }

        public static int fnGNTranServiceRequest(MServiceRequest objServiceRequest, string TransactionType, ref int IdServiceRequest, ref string Mensaje)
        {
            try
            {
                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int SqlResultService;
                int SqlResult;

                SqlParameter paramOutIdServiceRequest = new SqlParameter();
                paramOutIdServiceRequest.ParameterName = "@IdServiceRequest";
                paramOutIdServiceRequest.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdServiceRequest.Direction = System.Data.ParameterDirection.InputOutput;
                paramOutIdServiceRequest.Value = objServiceRequest.IdServiceRequest;

                SqlParameter paramIdContactType = new SqlParameter();
                paramIdContactType.ParameterName = "@IdContactType";
                if (objServiceRequest.IdContactType != null)
                {
                    paramIdContactType.Value = objServiceRequest.IdContactType;
                }
                else
                {
                    paramIdContactType.Value = DBNull.Value;
                }
                SqlParameter paramIdPropertyType = new SqlParameter();
                paramIdPropertyType.ParameterName = "@IdPropertyType";
                if (objServiceRequest.IdPropertyType != null)
                {
                    paramIdPropertyType.Value = objServiceRequest.IdPropertyType;
                }
                else
                {
                    paramIdPropertyType.Value = DBNull.Value;
                }
                SqlParameter paramPrice = new SqlParameter();
                paramPrice.ParameterName = "@Price";
                if (objServiceRequest.Price != null)
                {
                    paramPrice.Value = objServiceRequest.Price;
                }
                else
                {
                    paramPrice.Value = DBNull.Value;
                }
                SqlParameter paramDownPayment = new SqlParameter();
                paramDownPayment.ParameterName = "@DownPayment";
                if (objServiceRequest.DownPayment != null)
                {
                    paramDownPayment.Value = objServiceRequest.DownPayment;
                }
                else
                {
                    paramDownPayment.Value = DBNull.Value;
                }
                SqlParameter paramClosingCost = new SqlParameter();
                paramClosingCost.ParameterName = "@ClosingCost";
                if (objServiceRequest.ClosingCost != null)
                {
                    paramClosingCost.Value = objServiceRequest.ClosingCost;
                }
                else
                {
                    paramClosingCost.Value = DBNull.Value;
                }
                SqlParameter paramMonthlyIncome = new SqlParameter();
                paramMonthlyIncome.ParameterName = "@MonthlyIncome";
                if (objServiceRequest.MonthlyIncome != null)
                {
                    paramMonthlyIncome.Value = objServiceRequest.MonthlyIncome;
                }
                else
                {
                    paramMonthlyIncome.Value = DBNull.Value;
                }
                SqlParameter paramDebtPayment = new SqlParameter();
                paramDebtPayment.ParameterName = "@DebtPayment";
                if (objServiceRequest.DebtPayment != null)
                {
                    paramDebtPayment.Value = objServiceRequest.DebtPayment;
                }
                else
                {
                    paramDebtPayment.Value = DBNull.Value;
                }
                SqlParameter paramPiti = new SqlParameter();
                paramPiti.ParameterName = "@Piti";
                if (objServiceRequest.Piti != null)
                {
                    paramPiti.Value = objServiceRequest.Piti;
                }
                else
                {
                    paramPiti.Value = DBNull.Value;
                }
                SqlParameter paramRatios = new SqlParameter();
                paramRatios.ParameterName = "@Ratios";
                if (objServiceRequest.Ratios != null)
                {
                    paramRatios.Value = objServiceRequest.Ratios;
                }
                else
                {
                    paramRatios.Value = DBNull.Value;
                }
                SqlParameter paramEstimatedValue = new SqlParameter();
                paramEstimatedValue.ParameterName = "@EstimatedValue";
                if (objServiceRequest.EstimatedValue != null)
                {
                    paramEstimatedValue.Value = objServiceRequest.EstimatedValue;
                }
                else
                {
                    paramEstimatedValue.Value = DBNull.Value;
                }
                SqlParameter paramLoanAmount = new SqlParameter();
                paramLoanAmount.ParameterName = "@LoanAmount";
                if (objServiceRequest.LoanAmount != null)
                {
                    paramLoanAmount.Value = objServiceRequest.LoanAmount;
                }
                else
                {
                    paramLoanAmount.Value = DBNull.Value;
                }
                SqlParameter paramCurrentDebt = new SqlParameter();
                paramCurrentDebt.ParameterName = "@CurrentDebt";
                if (objServiceRequest.CurrentDebt != null)
                {
                    paramCurrentDebt.Value = objServiceRequest.CurrentDebt;
                }
                else
                {
                    paramCurrentDebt.Value = DBNull.Value;
                }
                SqlParameter paramPlane = new SqlParameter();
                paramPlane.ParameterName = "@Plane";
                if (objServiceRequest.Plane != null)
                {
                    paramPlane.Value = objServiceRequest.Plane;
                }
                else
                {
                    paramPlane.Value = DBNull.Value;
                }
                SqlParameter paramFinancing = new SqlParameter();
                paramFinancing.ParameterName = "@Financing";
                if (objServiceRequest.Financing != null)
                {
                    paramFinancing.Value = objServiceRequest.Financing;
                }
                else
                {
                    paramFinancing.Value = DBNull.Value;
                }

                SqlParameter paramAddress = new SqlParameter();
                paramAddress.ParameterName = "@Address";
                if (objServiceRequest.Address != null)
                {
                    paramAddress.Value = objServiceRequest.Address;
                }
                else
                {
                    paramAddress.Value = DBNull.Value;
                }
                SqlParameter paramAssets = new SqlParameter();
                paramAssets.ParameterName = "@Assets";
                if (objServiceRequest.Assets != null)
                {
                    paramAssets.Value = objServiceRequest.Assets;
                }
                else
                {
                    paramAssets.Value = DBNull.Value;
                }
                SqlParameter paramBeneficiaries = new SqlParameter();
                paramBeneficiaries.ParameterName = "@Beneficiaries";
                if (objServiceRequest.Beneficiaries != null)
                {
                    paramBeneficiaries.Value = objServiceRequest.Beneficiaries;
                }
                else
                {
                    paramBeneficiaries.Value = DBNull.Value;
                }
                SqlParameter paramProcess = new SqlParameter();
                paramProcess.ParameterName = "@Process";
                if (objServiceRequest.Process != null)
                {
                    paramProcess.Value = objServiceRequest.Process;
                }
                else
                {
                    paramProcess.Value = DBNull.Value;
                }
                SqlParameter paramWish = new SqlParameter();
                paramWish.ParameterName = "@Wish";
                if (objServiceRequest.Wish != null)
                {
                    paramWish.Value = objServiceRequest.Wish;
                }
                else
                {
                    paramWish.Value = DBNull.Value;
                }
                SqlParameter paramNote = new SqlParameter();

                paramNote.ParameterName = "@Note";
                if (objServiceRequest.Note != null)
                {
                    paramNote.Value = objServiceRequest.Note;
                }
                else
                {
                    paramNote.Value = DBNull.Value;
                }

                SqlResultService = db.Database.ExecuteSqlCommand("GNTranServiceRequest @TransactionType, @IdServiceRequest OUT, @IdServiceType, @IdServiceStatus, @IdPerson " +
                                                        " ,@IdContactType, @IdPropertyType ,@Address, @Price, @DownPayment, @ClosingCost, @MonthlyIncome, @DebtPayment " +
                                                        " ,@Piti, @Ratios, @EstimatedValue, @LoanAmount, @CurrentDebt, @Assets, @Beneficiaries " +
                                                        " ,@Process, @Wish, @Plane, @Financing, @Note, @IdUser ",
                        new SqlParameter[]{
                            new SqlParameter("@TransactionType", TransactionType),
                            paramOutIdServiceRequest,
                            new SqlParameter("@IdServiceType", objServiceRequest.IdServiceType),
                            new SqlParameter("@IdServiceStatus", objServiceRequest.IdServiceStatus),
                            new SqlParameter("@IdPerson", objServiceRequest.IdPerson),
                            paramIdContactType,
                            paramIdPropertyType,
                            paramAddress,
                            paramPrice,
                            paramDownPayment,
                            paramClosingCost,
                            paramMonthlyIncome,
                            paramDebtPayment,
                            paramPiti,
                            paramRatios,
                            paramEstimatedValue,
                            paramLoanAmount,
                            paramCurrentDebt,
                            paramAssets,
                            paramBeneficiaries,
                            paramProcess,
                            paramWish,
                            paramPlane,
                            paramFinancing,
                            paramNote,
                            new SqlParameter("@IdUser", objServiceRequest.IdUser)
                        }
                    );

                IdServiceRequest = Int32.Parse(paramOutIdServiceRequest.Value.ToString());

                if (IdServiceRequest != 0)
                {
                    //OPCIONES DE CONSTRUCCION
                    if (objServiceRequest.listConstructionOption != null)
                    {
                        if (objServiceRequest.listConstructionOption.Count()>0)
                        {
                            //si va a actualizar, se eliminan las opciones de construccion, para volverlas a insertar
                            if (TransactionType == "U")
                            {
                                SqlResult = db.Database.ExecuteSqlCommand("GNTranServiceConstructionOption @TransactionType, @IdServiceRequest, @IdConstructionOption ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", TransactionType),
                                        new SqlParameter("@IdServiceRequest", IdServiceRequest),
                                        new SqlParameter("@IdConstructionOption", DBNull.Value)
                                    }
                                );
                            }

                            //Inserta las opciones de construccion
                            foreach (var item in objServiceRequest.listConstructionOption)
                            {
                                SqlResult = db.Database.ExecuteSqlCommand("GNTranServiceConstructionOption @TransactionType, @IdServiceRequest, @IdConstructionOption ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "I"),
                                        new SqlParameter("@IdServiceRequest", IdServiceRequest),
                                        new SqlParameter("@IdConstructionOption", item.IdConstructionOption)
                                    }
                                );
                            }
                        }
                    }

                    //ACTIVIDAD/CITA
                    if (objServiceRequest.listTask != null)
                    {
                        if (objServiceRequest.listTask.Count()>0)
                        {
                            int IdTask=0;

                            //INVOLUCRADOS EN LA TASK
                            //Si esta actualizando, elimina los involucrados para volver a insertar (Por ahora 1 Solo)
                            if (TransactionType == "U")
                            {
                                SqlResult = db.Database.ExecuteSqlCommand("GNTranPersonTask @TransactionType, @IdTask, @IdPerson ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", TransactionType),
                                        new SqlParameter("@IdTask", ((MTask)objServiceRequest.listTask.First()).IdTask),
                                        new SqlParameter("@IdPerson", DBNull.Value)
                                    }
                                );

                            }

                            //(Inserta/Actualiza) las actividades del Servicio
                            foreach (var item in objServiceRequest.listTask)
                            {
                                if(item.IdServiceRequest==null)
                                item.IdServiceRequest = IdServiceRequest;
                                item.DateEnd = item.DateIni;
                                item.HourEnd = item.HourIni;
                                if(item.IdUser==0)
                                item.IdUser = objServiceRequest.IdUser;
                                int intresul =   TaskController.fnGNTranTask(item, TransactionType, ref IdTask, ref Mensaje);

                                if(intresul==0)
                                {
                                    return intresul;
                                }
                            }
                        }
                    }

                    Mensaje = "Datos grabados exitosamente.";
                }
                else
                {
                    Mensaje = "No se pudo realizar la transaccion, intente nuevamente.";
                }

                return SqlResultService;

            }
            catch (SqlException ex)
            {
                Mensaje = "Error al grabar datos: " + ex.Message;
                return 0;
            }
        }


        // GET: ServiceRequest/DetailServiceRequest/5
        public ActionResult DetailServiceRequest(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListServiceRequest").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Search == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataServiceRequest"] != null)
            {
                var objTempData = Newtonsoft.Json.JsonConvert.DeserializeObject<MServiceRequest>((string)TempData["DataServiceRequest"]);
                if (objTempData != null && objTempData.IdServiceRequest > 0)
                {
                    id = objTempData.IdServiceRequest;
                }
                else
                {
                    return RedirectToAction("ListServiceRequest", "ServiceRequest");
                }
            }
            if (id == null)
            {
                return RedirectToAction("ListServiceRequest", "ServiceRequest");
            }
            //-----------------------------------------------------

            MServiceRequest objServiceRequest = new MServiceRequest();
            objServiceRequest = fnListServiceRequest(id,null,null,null,null).First();

            if (objServiceRequest.listConstructionOption == null)
            {
                objServiceRequest.listConstructionOption = new List<MServiceConstructionOption>();
            }

            //----
            if (objServiceRequest.listTask != null && objServiceRequest.listTask.Count() > 0)
            {
                if (objServiceRequest.listTask[0].listTaskPerson != null && objServiceRequest.listTask[0].listTaskPerson.Count() > 0)
                {
                    objServiceRequest.listTask[0].IdPersonEmployee = objServiceRequest.listTask[0].listTaskPerson[0].IdPersonEmployee;
                    objServiceRequest.listTask[0].PersonEmployeeName = objServiceRequest.listTask[0].listTaskPerson[0].PersonEmployeeName;
                    objServiceRequest.listTask[0].PersonEmployeeLastName = objServiceRequest.listTask[0].listTaskPerson[0].PersonEmployeeLastName;
                }
            }
            else
            {
                objServiceRequest.listTask = new List<MTask>();
                objServiceRequest.listTask.Add(new MTask());
                objServiceRequest.listTask[0].DateIni = DateTime.Now.Date;
            }
            //----

            return View(objServiceRequest);
        }

        #region "Vistas Parciales"
        public ActionResult PartialAddServiceBuy()
        {
            return PartialView("_PartialAddServiceBuy");
        }
        public ActionResult PartialAddServiceBuilding()
        {
            return PartialView("_PartialAddServiceBuilding");
        }
        public ActionResult PartialAddRefinancing()
        {
            return PartialView("_PartialAddRefinancing");
        }
        public ActionResult PartialAddLivingTrust()
        {
            return PartialView("_PartialAddLivingTrust");
        }
        #endregion

    }
}