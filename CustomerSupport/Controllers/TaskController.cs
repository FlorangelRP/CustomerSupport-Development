using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        // GET: Client
        [HttpGet]
        public ActionResult ListTask()
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



            MTask objMTask = new MTask();
            return View(objMTask);
        }

        [HttpPost]
        public ActionResult ListTask(string submit, MTask objMTask)
        {

            if (objMTask == null || objMTask.IdTask == 0)
            {
                return View();
            }

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objMTask);
            TempData["DataTask"] = jsonString;

            switch (submit)
            {
                case "searchRow":
                    return RedirectToAction("DetailTask", "Task");
                case "editRow":
                    return RedirectToAction("EditTask", "Task");
                default:
                    return View();
            }

        }

        public ActionResult GetListTask(MTask objFilter)
        {

            List<MTask> objListTask = new List<MTask>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            int? IdTask = null;
            DateTime? dttDateIni = null;
            DateTime? dttDateEnd = null;
            int? IdResponsable = null;
            int? IdPriority = null;
            int? IdStatus = null;
            int? IdTypeTask = null;
            int? IdServiceRequest = null;
            int? IdUser = ((MSerUser)Session["Usuario"]).IdUser;
            int? IdFatherTask = null;
            string strtittle = "";

            if (objFilter.IdTask > 0)
                IdTask = objFilter.IdTask;

            if (objFilter.DateIni !=null)
                dttDateIni = objFilter.DateIni;

            if (objFilter.DateEnd != null)
                dttDateEnd = objFilter.DateEnd;

            if (objFilter.IdPersonEmployee != null && objFilter.IdPersonEmployee>0)
                IdResponsable = objFilter.IdPersonEmployee;

            if ( objFilter.IdPriority > 0)
                IdPriority = objFilter.IdPriority;

            if (objFilter.IdStatus >0)
                IdStatus = objFilter.IdStatus;

            if (objFilter.IdTypeTask > 0)
                IdTypeTask = objFilter.IdTypeTask;

            if (objFilter.IdServiceRequest!=null)
                IdServiceRequest = objFilter.IdServiceRequest;

            if (objFilter.IdUser > 0)
                IdUser = objFilter.IdUser;

            if (objFilter.Tittle != null)
                strtittle = objFilter.Tittle;

            if (objFilter.IdFatherTask != null)
                IdFatherTask = objFilter.IdFatherTask;


            objListTask = fnListTask(IdTask, dttDateIni, dttDateEnd, IdResponsable,strtittle, IdPriority, IdStatus, IdTypeTask, IdServiceRequest, IdUser,IdFatherTask);

            return Json(objListTask, JsonRequestBehavior.AllowGet);

        }


        public static List<MTask> fnListTask(int? IdTask = null, DateTime? dttDateIni = null, DateTime? dttDateEnd = null, int? IdResponsable = null, string strTittle = "",int? IdPriority = null,  int? IdStatus = null,int? IdTypeTask = null, int? IdServiceRequest = null, int? IdUser = null, int? IdFatherTask = null)
        {


            List<MTask> listTask = new List<MTask>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser objUser = new MUser();

            listTask = (List<MTask>)(from tsk in db.GNListTask(IdTask, dttDateIni, dttDateEnd, IdResponsable, strTittle, IdPriority, IdStatus, IdTypeTask, IdServiceRequest, IdUser, IdFatherTask).ToList()
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
                                             IdFatherTask= tsk.IdFatherTask,
                                             Tittle= tsk.Tittle,
                                             IdServiceRequest=tsk.IdServiceRequest,
                                             IdTypeTask = tsk.IdTypeTask,
                                             TypeTask = tsk.TypeTask,
                                             IdPriority = tsk.IdPriority,
                                             PriorityTask = tsk.PriorityTask,
                                             IdPersonEmployee = tsk.IdResponsable,
                                             PersonEmployeeName= tsk.Name, 
                                             PersonEmployeeLastName = tsk.LastName,
                                             Activity = tsk.Activity,
                                             Confidential = tsk.confidential==null?false: (bool)tsk.confidential,
                                             listTaskPerson = (List<MTaskPerson>)(from tp in db.GNListPersonTask(tsk.IdTask, null).ToList()
                                                                                  select new MTaskPerson
                                                                                  {
                                                                                      IdPersonEmployee = tp.IdPersonEmployee,
                                                                                      PersonEmployeeName = tp.PersonEmployeeName,
                                                                                      PersonEmployeeLastName = tp.PersonEmployeeLastName,
                                                                                      NumIdentification = objUser.Desencriptar(tp.NumIdentification)
                                                                                  }).ToList()
                                                                                  ,
                                             listMTaskComment = (List<MTaskComment>)(from tp in db.GNListBitacora(tsk.IdTask).ToList()
                                                                                  select new MTaskComment
                                                                                  {
                                                                                      IdComment = tp.IdComment,
                                                                                      IdTask = tp.IdTask,
                                                                                      Comment = tp.Comment,
                                                                                      IdUser = tp.IdUser,
                                                                                      UserName = tp.UserName,
                                                                                      DateOperation = tp.DateOperation,
                                                                                      Date = tp.Date,
                                                                                      New = 0,
                                                                                  }).ToList()

                                         }).ToList();
            return listTask;

        }


        public ActionResult DetailTask(int? id) //string id / MUser objMUser / MParameterUrl objMParameter
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
                var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTask").First();
                if (ObjAcces != null)
                {
                    if (ObjAcces.Search == false)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
                if (TempData["DataTask"] != null)
                {
                    var objTempData = Newtonsoft.Json.JsonConvert.DeserializeObject<MTask>((string)TempData["DataTask"]);
                    if (objTempData != null && objTempData.IdTask > 0)
                    {
                        id = objTempData.IdTask;
                    }
                    else
                    {
                        return RedirectToAction("ListTask", "Task");
                    }
                }
                if (id == null)
                {
                    return RedirectToAction("ListTask", "Task");
                }
                //-----------------------------------------------------    

                MUser ObjUser = new MUser();
                int? IdUser = ((MSerUser)Session["Usuario"]).IdUser;
                MMEnterprisesEntities db = new MMEnterprisesEntities();

                //Convert.ToInt32(id)
               var objListTask = fnListTask(id, null, null, null, null, null, null, null, null, IdUser, null);

                return View(objListTask.First());
            }

        }

        public ActionResult AddTask()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTask").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Create == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            MTask objMask = new MTask();
            objMask.listTaskPerson = new List<MTaskPerson>();

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objMask);
        }

        [HttpPost]
        public ActionResult AddTask(MTask objTask)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            try
            {
                if(objTask.DateEnd< objTask.DateIni)
                {
                    ViewBag.ErrorSave = "La fecha Fin no puede ser menor a la fecha inicial";
                    return View(objTask);
                }
                
                if (objTask.DateEnd == objTask.DateIni && objTask.HourIni == objTask.HourEnd)
                {
                    ViewBag.ErrorSave = "La Hora Inicio no puede ser igual a la Hora Fin";
                    return View(objTask);
                }

                if (ModelState.IsValid)
                {
                    //valores por defecto
                    objTask.IdUser = ((MSerUser)Session["Usuario"]).IdUser;

                    string mensaje = "";
                    int IdTask = 0;
                    int resultDb = fnGNTranTask(objTask, "I", ref IdTask, ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje + " N° de Actividad generado: " + IdTask + ".";
                        return RedirectToAction("AddTask");
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objTask);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objTask);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos de la solicitud de servicio: " + ex.Message;
                return View(objTask);
            }

        }

        // GET: User/Edit/5int id
        public ActionResult EditTask(int? id) //string id / MUser objMuser, string id / FormCollection collection
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListTask").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Edit == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataTask"] != null)
            {
                var objTempData = Newtonsoft.Json.JsonConvert.DeserializeObject<MTask>((string)TempData["DataTask"]);
                if (objTempData != null && objTempData.IdTask > 0)
                {
                    id = objTempData.IdTask;
                }
                else
                {
                    return RedirectToAction("ListTask", "Task");
                }
            }
            if (id == null)
            {
                return RedirectToAction("ListTask", "Task");
            }
            //-----------------------------------------------------

            MTask objMTask = new MTask();
            int? IdUser = ((MSerUser)Session["Usuario"]).IdUser;

            var ListT = fnListTask(id, null, null, null, null, null, null, null, null, IdUser);
            if(ListT.Count>0)
            objMTask = ListT.First();
            else
                return RedirectToAction("ListTask", "Task");



            //MTaskComment mTaskComment = new MTaskComment();
            //objMTask.listMTaskComment.Add(mTaskComment);
            //objMTask.listMTaskComment.OrderByDescending(x => x.IdTask);


            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objMTask);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult EditTask(MTask objTask)
        {
            try
            {
                objTask.IdUser = ((MSerUser)Session["Usuario"]).IdUser;

                if (objTask.DateEnd == objTask.DateIni && objTask.HourIni == objTask.HourEnd)
                {
                    ViewBag.ErrorSave = "La Hora Inicio no puede ser igual a la Hora Fin";
                    return View(objTask);
                }

                if (ModelState.IsValid)
                {

                    string mensaje = "";
                    int result = 0;
                    int resultDb = fnGNTranTask(objTask, "U", ref result, ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;

                        //Para evitar que se vea el id en la Url------------
                        MTask objMTask = new MTask();
                        objMTask.IdTask = objTask.IdTask;

                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objMTask);
                        TempData["DataTask"] = jsonString;
                        return RedirectToAction("EditTask");
                        //---------------------------------------------------

                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objTask);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objTask);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos la Actividad: " + ex.Message;
                return View(objTask);
            }

        }
        public static int fnGNTranTask(MTask objTask, string TransactionType, ref int IdTask, ref string Mensaje)
        {
            try
            {
                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int SqlResult;
                int SqlResultTask;

                SqlParameter paramOutIdTask = new SqlParameter();
                paramOutIdTask.ParameterName = "@IdTask";
                paramOutIdTask.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdTask.Direction = System.Data.ParameterDirection.InputOutput;
                paramOutIdTask.Value = objTask.IdTask;

                SqlParameter paramTransactionType = new SqlParameter();
                paramTransactionType.ParameterName = "@TransactionType";
                if (objTask.IdTask == 0 && TransactionType == "U")
                {
                    //la task no existe, debe ser creada.
                    paramTransactionType.Value = "I";
                }
                else
                {
                    paramTransactionType.Value = TransactionType;
                }

                SqlParameter paramIdFatherTask = new SqlParameter();
                paramIdFatherTask.ParameterName = "@IdFatherTask";
                paramIdFatherTask.SqlDbType = System.Data.SqlDbType.Int;
                paramIdFatherTask.Direction = System.Data.ParameterDirection.Input;
                paramIdFatherTask.IsNullable = true;
                if (objTask.IdFatherTask != null)
                {
                    paramIdFatherTask.Value = objTask.IdFatherTask;
                }
                else
                {
                    paramIdFatherTask.Value = DBNull.Value;
                }

                SqlParameter paramPlace = new SqlParameter();
                paramPlace.ParameterName = "@strPlace";

                if (objTask.Place != null)
                {
                    paramPlace.Value = objTask.Place;
                }
                else
                {
                    paramPlace.Value = "";
                }

                if (objTask.DateEnd == null)
                    objTask.DateEnd = DateTime.Today;

                if (objTask.DateIni == null)
                    objTask.DateIni = DateTime.Today;


                if (objTask.HourEnd == null)
                    objTask.HourEnd = TimeSpan.Zero;


                if (objTask.HourIni == null)
                    objTask.HourIni = TimeSpan.Zero;

                SqlParameter IdResponsable = new SqlParameter();
                IdResponsable.ParameterName = "@IdResponsable";
                IdResponsable.SqlDbType = System.Data.SqlDbType.Int;
                IdResponsable.Direction = System.Data.ParameterDirection.Input;
                IdResponsable.IsNullable = true;
                if (objTask.IdPersonEmployee != null)
                {
                    IdResponsable.Value = objTask.IdPersonEmployee;
                }
                else
                {
                    IdResponsable.Value = DBNull.Value;
                }

                //SqlParameter paramConfidential = new SqlParameter();
                //paramPlace.ParameterName = "@blnConfidential";
                //IdResponsable.SqlDbType = System.Data.SqlDbType.Bit;
                //IdResponsable.Direction = System.Data.ParameterDirection.Input;
                //IdResponsable.IsNullable = false;
                //if (objTask.Confidential != null)
                //{
                //    paramConfidential.Value = objTask.Confidential;
                //}
                //else
                //{
                //    paramConfidential.Value = false;
                //}

                MUser objUser = new MUser();


                SqlResultTask = db.Database.ExecuteSqlCommand("GNTranTask @TransactionType, @IdTask OUT, @IdUser " +
                                                        ", @dttDateIni, @dttDateEnd, @tHourIni, @tHourEnd, @strPlace " +
                                                        ", @IdFatherTask, @IdResponsable, @strTittle, @IdPriority, @IdStatus, @IdTypeTask,@strActivity,@blnConfidential ",
                        new SqlParameter[]{
                            new SqlParameter("@TransactionType", TransactionType),
                            paramOutIdTask,
                            new SqlParameter("@IdUser", objTask.IdUser),
                            new SqlParameter("@dttDateIni", objTask.DateIni),
                            new SqlParameter("@dttDateEnd", objTask.DateEnd),
                            new SqlParameter("@tHourIni",objTask.HourIni),
                            new SqlParameter("@tHourEnd", objTask.HourEnd),
                            paramPlace,
                            paramIdFatherTask,
                            IdResponsable,
                            new SqlParameter("@strTittle", objTask.Tittle),
                            new SqlParameter("@IdPriority", objTask.IdPriority),
                            new SqlParameter("@IdStatus", objTask.IdStatus),
                            new SqlParameter("@IdTypeTask", objTask.IdTypeTask),
                            new SqlParameter("@strActivity", objTask.Activity),
                            new SqlParameter("@blnConfidential", objTask.Confidential)
                        }
                    );

                IdTask = Int32.Parse(paramOutIdTask.Value.ToString());

                if (IdTask != 0)
                {
                    if (objTask.listMTaskComment != null)
                    {
                        if (objTask.listMTaskComment.Count() > 0)
                        {

                            ////si va a actualizar, se eliminan los Comentarios de las actividades para volver a insertar
                            //if (TransactionType == "U")
                            //{
                            //    SqlParameter paramOutIdComment = new SqlParameter("@IdComment", System.Data.SqlDbType.Int);
                            //    paramOutIdComment.Direction = System.Data.ParameterDirection.Output;


                            //        SqlResult = db.Database.ExecuteSqlCommand("GNTranCommentTask @TransactionType, @IdComment OUT, @IdTask " +
                            //                                                ", @strComment,@IdUser, @dttDate  ",
                            //        new SqlParameter[]{
                            //            new SqlParameter("@TransactionType", TransactionType),
                            //            paramOutIdComment,
                            //            new SqlParameter("@IdTask", IdTask),
                            //            new SqlParameter("@strComment", DBNull.Value),
                            //            new SqlParameter("@IdUser", DBNull.Value),
                            //            new SqlParameter("@dttDate", DBNull.Value)
                            //        }
                            //    );
                            //}

                            //Inserta los Comentarios
                            foreach (var item in objTask.listMTaskComment)
                            {
                                if (item.IdComment != null && item.Comment!=null)
                                { 
                                if (item.Date == null)
                                    item.Date = DateTime.Now;

                                if (item.IdUser == null)
                                    item.IdUser = objTask.IdUser;

                                int IdComment = (int)item.IdComment;
                                SqlParameter paramOutIdComment = new SqlParameter("@IdComment", System.Data.SqlDbType.Int);
                                paramOutIdComment.Direction = System.Data.ParameterDirection.InputOutput;
                                paramOutIdComment.Value = IdComment;

                                SqlResult = db.Database.ExecuteSqlCommand("GNTranCommentTask @TransactionType, @IdComment OUT, @IdTask " +
                                                                            ", @strComment,@IdUser, @dttDate ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "I"),
                                        paramOutIdComment,
                                        new SqlParameter("@IdTask", IdTask),
                                        new SqlParameter("@strComment", item.Comment),
                                        new SqlParameter("@IdUser", item.IdUser),
                                        new SqlParameter("@dttDate", item.Date),
                                    }
                                );
                                    IdComment = Int32.Parse(paramOutIdComment.Value.ToString());
                                    item.IdComment = IdComment;
                                }
                            }
                        }
                    }

                    if (objTask.listTaskPerson != null)
                    {
                        if (objTask.listTaskPerson.Count() > 0)
                        {

                            //si va a actualizar, se eliminan los Comentarios de las actividades para volver a insertar
                            if (TransactionType == "U")
                            {
                                int IdPerson = 0;
                                SqlResult = db.Database.ExecuteSqlCommand("GNTranPersonTask @TransactionType, @IdTask, @IdPerson ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "U"),
                                        new SqlParameter("@IdTask", IdTask),
                                        new SqlParameter("@IdPerson", IdPerson)
                                    }
                                );
                            }

                            //Inserta los Comentarios
                            foreach (var item in objTask.listTaskPerson)
                            {

                                SqlResult = db.Database.ExecuteSqlCommand("GNTranPersonTask @TransactionType, @IdTask, @IdPerson ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "I"),
                                        new SqlParameter("@IdTask", IdTask),
                                        new SqlParameter("@IdPerson", item.IdPersonEmployee)
                                    }
                                );

                            }
                        }
                    }

                    if(objTask.IdServiceRequest!=null)
                    {
                        //si va a actualizar, se eliminan los Comentarios de las actividades para volver a insertar
                        if (TransactionType == "U")
                        {
                            int IdTask0 = 0;
                            SqlResult = db.Database.ExecuteSqlCommand("GNTranServiceRequestTask @TransactionType, @IdTask, @IdServiceRequest ",
                            new SqlParameter[]{
                                                    new SqlParameter("@TransactionType", TransactionType), //TransactionType
                                                    new SqlParameter("@IdTask", IdTask0),
                                                    new SqlParameter("@IdServiceRequest", objTask.IdServiceRequest)
                                }
                            );
                        }

                        //Se asocia la actividad con el servicio si esta insertando
                        if (paramTransactionType.Value.ToString() == "I") //TransactionType == "I"
                        {
                            if(objTask.IdServiceRequest!=null)
                            { 
                                SqlResult = db.Database.ExecuteSqlCommand("GNTranServiceRequestTask @TransactionType, @IdTask, @IdServiceRequest ",
                                    new SqlParameter[]{
                                                        new SqlParameter("@TransactionType",TransactionType), //TransactionType
                                                        new SqlParameter("@IdTask", IdTask),
                                                        new SqlParameter("@IdServiceRequest", objTask.IdServiceRequest)
                                    }
                                );
                            }
                        }
                    }

                    //Mensaje = "Datos grabados exitosamente para el Código de empleado: (" + IdPerson + ").";
                    Mensaje = "Datos grabados exitosamente.";
                }
                else
                {
                    Mensaje = "No se pudo realizar la transaccion, intente nuevamente.";
                }

                return SqlResultTask;

            }
            catch (SqlException ex)
            {
                Mensaje = "Error al grabar datos: " + ex.Message;
                return 0;
            }
        }
    }
}