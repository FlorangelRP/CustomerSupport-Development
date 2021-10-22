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
    public class PersonController : Controller
    {
        public ActionResult ListPerson()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        public ActionResult GetListPerson(int? IdPersonType = null, bool? PersonStatus = null,int? IdDepartment =null, bool? blnUser = false)
        {
            List<MPerson> ListPerson = new List<MPerson>();

            int? Iduser  = null;



            if (blnUser==true)
            {
                Iduser  = ((MSerUser)Session["Usuario"]).IdUser;
            }

            ListPerson = fnListPerson(null, IdPersonType, IdDepartment, PersonStatus, Iduser); 

            return Json(ListPerson, JsonRequestBehavior.AllowGet); 
        }


        public static List<MPerson> fnListPerson(int? idPerson, int? PersonType, int? Department=null, bool? PersonStatus=null, int? IdUser = null)
        {
            List<MPerson> ListPerson = new List<MPerson>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser objUser = new MUser();

            ListPerson = (from result in db.GNListPerson(idPerson, PersonType, PersonStatus, Department, IdUser).ToList()
                          select new MPerson
                          {
                              IdPerson = result.IdPerson,
                              IdPersonType = result.IdPersonType,
                              PersonType = result.PersonType,
                              IdIdentificationType = result.IdIdentificationType,
                              IdentificationType = result.IdentificationType,
                              NumIdentification = objUser.Desencriptar(result.NumIdentification) ,
                              Name = result.Name,
                              LastName = result.LastName,
                              Birthday = result.Birthday,
                              Address = objUser.Desencriptar( result.Address),
                              Email = result.Email,
                              IdContactType = result.IdContactType,
                              ContactType = result.ContactType,
                              IdPosition = result.IdPosition,
                              Position = result.Position,
                              ClientPermission = result.ClientPermission,
                              Status = result.Status,
                              StatusDesc = result.Status == true ? "Activo" : "Inactivo",
                              IdDepartment=result.IdDepartment,
                              Department=result.Department,
                              listPersonContact = (List<MPersonContact>)(from result2 in db.GNListPersonContact(result.IdPerson, result.IdPersonType, null).ToList()
                                                                         select new MPersonContact
                                                                         {
                                                                             IdContact = result2.IdContact,
                                                                             IdPerson = result2.IdPerson,
                                                                             IdPhoneNumberType = result2.IdPhoneNumberType,
                                                                             PhoneNumberType = result2.PhoneNumberType,
                                                                             IdIsoCountry = result2.IdIsoCountry,
                                                                             CountryAreaCode = result2.CountryAreaCode,
                                                                             PhoneNumber = objUser.Desencriptar(result2.PhoneNumber),
                                                                             Status = result2.Status
                                                                         }).ToList()

                          }).ToList();



            return ListPerson;

        }

        public static int fnGNTranPerson(MPerson objPerson, string TransactionType, ref string Mensaje)
        {
            try
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo; //para capitalizar textos

                MMEnterprisesEntities db = new MMEnterprisesEntities();

                int IdPerson;
                int IdContact;
                int SqlResultPerson;
                int SqlResult;

                SqlParameter paramOutIdPerson = new SqlParameter();
                paramOutIdPerson.ParameterName = "@IdPerson";
                paramOutIdPerson.SqlDbType = System.Data.SqlDbType.Int;
                paramOutIdPerson.Direction = System.Data.ParameterDirection.InputOutput;
                paramOutIdPerson.Value = objPerson.IdPerson;

                SqlParameter paramIdContactType = new SqlParameter();
                paramIdContactType.ParameterName = "@IdContactType";
                paramIdContactType.SqlDbType = System.Data.SqlDbType.Int;
                paramIdContactType.Direction = System.Data.ParameterDirection.Input;
                paramIdContactType.IsNullable = true;
                if (objPerson.IdContactType != null)
                {
                    paramIdContactType.Value = objPerson.IdContactType;
                }
                else
                {
                    paramIdContactType.Value = DBNull.Value;
                }

                SqlParameter paramIdPosition = new SqlParameter();
                paramIdPosition.ParameterName = "@IdPosition";
                paramIdPosition.SqlDbType = System.Data.SqlDbType.Int;
                paramIdPosition.Direction = System.Data.ParameterDirection.Input;
                paramIdPosition.IsNullable = true;
                if (objPerson.IdPosition != null)
                {
                    paramIdPosition.Value = objPerson.IdPosition;
                }
                else
                {
                    paramIdPosition.Value = DBNull.Value;
                }

                SqlParameter paramIdDepartment = new SqlParameter();
                paramIdDepartment.ParameterName = "@IdDepartment";
                paramIdDepartment.SqlDbType = System.Data.SqlDbType.Int;
                paramIdDepartment.Direction = System.Data.ParameterDirection.Input;
                paramIdDepartment.IsNullable = true;
                if (objPerson.IdDepartment != null)
                {
                    paramIdDepartment.Value = objPerson.IdDepartment;
                }
                else
                {
                    paramIdDepartment.Value = DBNull.Value;
                }

                MUser objUser = new MUser();

                SqlResultPerson = db.Database.ExecuteSqlCommand("GNTranPerson @TransactionType, @IdPerson OUT, @IdPersonType " +
                                                        ", @IdIdentificationType, @strNumIdentification, @strName, @strLastName, @dttBirthday " +
                                                        ", @strAddress, @strEmail, @IdContactType, @IdPosition, @btClientPermission, @btStatus, @IdDepartment ",
                        new SqlParameter[]{
                            new SqlParameter("@TransactionType", TransactionType),
                            paramOutIdPerson,
                            new SqlParameter("@IdPersonType", objPerson.IdPersonType),
                            new SqlParameter("@IdIdentificationType", objPerson.IdIdentificationType),
                            new SqlParameter("@strNumIdentification",objUser.Encriptar(objPerson.NumIdentification)),
                            new SqlParameter("@strName", ti.ToTitleCase(objPerson.Name)),
                            new SqlParameter("@strLastName", ti.ToTitleCase(objPerson.LastName)),
                            new SqlParameter("@dttBirthday", objPerson.Birthday),
                            new SqlParameter("@strAddress", objUser.Encriptar(objPerson.Address)),
                            new SqlParameter("@strEmail", objPerson.Email),
                            paramIdContactType,
                            paramIdPosition,
                            new SqlParameter("@btClientPermission", objPerson.ClientPermission),
                            new SqlParameter("@btStatus", objPerson.Status),
                            paramIdDepartment
                        }
                    );

                IdPerson = Int32.Parse(paramOutIdPerson.Value.ToString());

                if (IdPerson != 0)
                {
                    if (objPerson.listPersonContact != null)
                    {
                        if (objPerson.listPersonContact.Count()>0)
                        {

                            //si va a actualizar, se eliminan los telefonos de contacto para volver a insertar
                            if (TransactionType == "U")
                            {
                                SqlParameter paramOutIdContact = new SqlParameter("@IdContact", System.Data.SqlDbType.Int);
                                paramOutIdContact.Direction = System.Data.ParameterDirection.Output;

                                SqlResult = db.Database.ExecuteSqlCommand("GNTranPersonContact @TransactionType, @IdContact OUT, @IdPerson " +
                                                                            ", @IdPhoneNumberType, @strIdIsoCountry, @strPhoneNumber, @btStatus ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", TransactionType),
                                        paramOutIdContact,
                                        new SqlParameter("@IdPerson", IdPerson),
                                        new SqlParameter("@IdPhoneNumberType", DBNull.Value),
                                        new SqlParameter("@strIdIsoCountry", DBNull.Value),
                                        new SqlParameter("@strPhoneNumber", DBNull.Value),
                                        new SqlParameter("@btStatus", DBNull.Value)
                                    }
                                );
                            }

                            //Inserta los telefonos de contacto
                            foreach (var item in objPerson.listPersonContact)
                            {

                                SqlParameter paramOutIdContact = new SqlParameter("@IdContact", System.Data.SqlDbType.Int);
                                paramOutIdContact.Direction = System.Data.ParameterDirection.Output;

                                SqlResult = db.Database.ExecuteSqlCommand("GNTranPersonContact @TransactionType, @IdContact OUT, @IdPerson " +
                                                                            ", @IdPhoneNumberType, @strIdIsoCountry, @strPhoneNumber, @btStatus ",
                                    new SqlParameter[]{
                                        new SqlParameter("@TransactionType", "I"),
                                        paramOutIdContact,
                                        new SqlParameter("@IdPerson", IdPerson),
                                        new SqlParameter("@IdPhoneNumberType", item.IdPhoneNumberType),
                                        new SqlParameter("@strIdIsoCountry", item.IdIsoCountry),
                                        new SqlParameter("@strPhoneNumber", objUser.Encriptar(item.PhoneNumber)),
                                        new SqlParameter("@btStatus", true)
                                    }
                                );
                                IdContact = Int32.Parse(paramOutIdContact.Value.ToString());
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

                return SqlResultPerson;

            }
            catch (SqlException ex)
            {
                Mensaje = "Error al grabar datos: " + ex.Message;
                return 0;
            }
        }


    }
}
