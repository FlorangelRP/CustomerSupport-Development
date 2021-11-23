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
    public class NotificationSettingsController : Controller
    {
        [HttpGet]
        public ActionResult ListNotificationSettings()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MSerUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListNotificationSettings").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Visible == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var Iduser = ((MSerUser)Session["Usuario"]).IdUser;

            MNotificationSettings objMNotificationSettings = new MNotificationSettings();
             var objLisMNotificationSettings = fnListMNotificationSettings(Iduser);
            if (objLisMNotificationSettings.Count > 0)
                objMNotificationSettings = objLisMNotificationSettings.First();

            return View(objMNotificationSettings);

        }

        [HttpPost]
        public ActionResult ListNotificationSettings(MNotificationSettings objMNotificacion)
        {

            if (objMNotificacion == null)
            {
                return View();
            }
            var Iduser = ((MSerUser)Session["Usuario"]).IdUser;

            int IdNotificacion=0;
            string strMensaj = "";

            var resultDb = fnGNTranNotificacionSettings(objMNotificacion, Iduser, "I",ref IdNotificacion, ref strMensaj);
            if (resultDb != 0)
            {
                MNotificationSettings objMNotificationSettings = new MNotificationSettings();
                var objLisMNotificationSettings = fnListMNotificationSettings(Iduser);
                if (objLisMNotificationSettings.Count > 0)
                    objMNotificacion = objLisMNotificationSettings.First();

                ViewBag.SuccessSave = strMensaj + " identificacion generada: " + IdNotificacion + ".";
                
            }
            else
            {
                ViewBag.ErrorSave = strMensaj;
                
            }

            return View(objMNotificacion);

        }

        public static  List<MNotificationSettings> fnListMNotificationSettings(int? IdUser = null,int? IdPerson =null)
        {


            List<MNotificationSettings> listNotificacionSetting= new List<MNotificationSettings>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser objUser = new MUser();


            listNotificacionSetting = (List<MNotificationSettings>)(from Notif in db.GNLisNotificationSettings(IdUser, IdPerson).ToList()
                                     select new MNotificationSettings
                                     {
                                         IdSetting = Notif.IdSetting,
                                         IdUser = Notif.IdUser,
                                         SendAddComment = Notif.SendAddComment,
                                         SendColaborator = Notif.SendColaborator,
                                         SendEditComment = Notif.SendEditComment,
                                         SendFollower = Notif.SendFollower,
                                         SendResponsable = Notif.SendResponsable,
                                         Email = Notif.Email,
                                         LisMNotificationSettingsPriority = (List<MNotificationSettingsPriority>)(from NP in db.GNLisNotificationSettingsPriority(Notif.IdSetting).ToList()
                                                                                                                  select new MNotificationSettingsPriority
                                                                                                                  {
                                                                                                                      IdPriority = NP.IdPriority,
                                                                                                                      IdSetting = NP.IdSetting,
                                                                                                                      Priority = NP.Priority
                                                                                                                  }).ToList(),
                                         LisNotificationSettingsStatus = (List<MNotificationSettingsStatus>)(from NS in db.GNLisNotificationSettingsStatus(Notif.IdSetting).ToList()
                                                                                 select new MNotificationSettingsStatus
                                                                                 {IdSetting =NS.IdSetting,
                                                                                  IdStatus = NS.IdStatus,
                                                                                  Status = NS.Status}).ToList(),

                                     }).ToList();
            return listNotificacionSetting;

        }

        public static int fnGNTranNotificacionSettings(MNotificationSettings objMNotificacion, int IdUser, string TransactionType, ref int IdNotificacion, ref string Mensaje)
        {
            try
            {
                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int SqlResult;
                int SqlResultTask;

                SqlParameter paramOutIdSetting = new SqlParameter();
                paramOutIdSetting.ParameterName = "@IdSetting";
                paramOutIdSetting.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdSetting.Direction = System.Data.ParameterDirection.InputOutput;
                paramOutIdSetting.Value = objMNotificacion.IdSetting;

                SqlParameter paramTransactionType = new SqlParameter();
                paramTransactionType.ParameterName = "@TransactionType";
                if (objMNotificacion.IdSetting == 0 && TransactionType == "U")
                {
                    //la task no existe, debe ser creada.
                    paramTransactionType.Value = "I";
                }
                else
                {
                    paramTransactionType.Value = TransactionType;
                }


                MUser objUser = new MUser();


                SqlResultTask = db.Database.ExecuteSqlCommand("GNTranNotificationSettings @blnSendResponsable, @blnSendColaborator, @blnSendFollower, @blnSendAddComment, @blnSendEditComment " +
                                                        " , @IdUser  , @TransactionType, @IdSetting OUT ",
                        new SqlParameter[]{
                            new SqlParameter("@blnSendResponsable", objMNotificacion.SendResponsable),
                            new SqlParameter("@blnSendColaborator", objMNotificacion.SendColaborator),
                            new SqlParameter("@blnSendFollower", objMNotificacion.SendFollower),
                            new SqlParameter("@blnSendAddComment",objMNotificacion.SendAddComment),
                            new SqlParameter("@blnSendEditComment", objMNotificacion.SendEditComment),
                            new SqlParameter("@IdUser",IdUser),
                            new SqlParameter("@TransactionType", TransactionType),
                            paramOutIdSetting
                        }
                    );

                IdNotificacion = Int32.Parse(paramOutIdSetting.Value.ToString());

                if (IdNotificacion != 0)
                {
                    if (objMNotificacion.LisMNotificationSettingsPriority != null)
                    {
                        if (objMNotificacion.LisMNotificationSettingsPriority.Count() > 0)
                        {

                            //si va a actualizar, se eliminan los Comentarios de las actividades para volver a insertar
                            int intPriority = 0;

                            SqlResult = db.Database.ExecuteSqlCommand("GNTranNotificationSettingsPriority @TransactionType, @IdSetting , @IdPriority",
                                new SqlParameter[]{
                                                                    new SqlParameter("@TransactionType", "U"),
                                                                    new SqlParameter("@IdSetting",IdNotificacion),
                                                                    new SqlParameter("@IdPriority", intPriority)

                                }
                            );

  
                            //Inserta los Comentarios
                            foreach (var item in objMNotificacion.LisMNotificationSettingsPriority)
                            {

                                SqlResult = db.Database.ExecuteSqlCommand("GNTranNotificationSettingsPriority @TransactionType, @IdSetting , @IdPriority",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "I"),
                                        new SqlParameter("@IdSetting",IdNotificacion),
                                        new SqlParameter("@IdPriority", item.IdPriority)

                                    }
                                );

                            }         
                        }
                    }


                    if (objMNotificacion.LisNotificationSettingsStatus != null)
                    {
                        if (objMNotificacion.LisNotificationSettingsStatus.Count() > 0)
                        {
                            int intIdStatus = 0;
                            //si va a actualizar, se eliminan los Comentarios de las actividades para volver a insertar

                            SqlResult = db.Database.ExecuteSqlCommand("GNTraNotificationSettingsStatus @TransactionType, @IdStatus, @IdSetting ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "U"),
                                        new SqlParameter("@IdStatus", intIdStatus),
                                        new SqlParameter("@IdSetting",IdNotificacion)
                                    }
                                );

                            //Inserta los Comentarios
                            foreach (var item in objMNotificacion.LisNotificationSettingsStatus)
                            {

                                SqlResult = db.Database.ExecuteSqlCommand("GNTraNotificationSettingsStatus @TransactionType, @IdStatus, @IdSetting ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "I"),
                                        new SqlParameter("@IdStatus", item.IdStatus),
                                        new SqlParameter("@IdSetting",IdNotificacion)
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
