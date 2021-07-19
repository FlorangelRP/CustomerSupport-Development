using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CustomerSupport.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        [HttpGet]
        public ActionResult ListClient()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListClient").First();
            if(ObjAcces!=null)
            {
                if(ObjAcces.Visible ==false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            MPerson objMPerson = new MPerson();
            return View(objMPerson);
        }

        [HttpPost]
        public ActionResult ListClient(string submit, MPerson objMPerson)
        {
            if (objMPerson == null || objMPerson.IdPerson == 0)
            {
                return View();
            }

            TempData["DataPersonClient"] = objMPerson;

            switch (submit)
            {
                case "searchRow":
                    return RedirectToAction("DetailClient", "Client");
                case "editRow":
                    return RedirectToAction("EditClient", "Client");
                default:
                    return View();
            }

        }

        public ActionResult GetListClient()
        {
            List<MPerson> ListPerson = new List<MPerson>();
            ListPerson = PersonController.fnListPerson(null, 1); //1-cliente

            return Json(ListPerson, JsonRequestBehavior.AllowGet); 
        }

        // GET: Client/DetailClient/5
        public ActionResult DetailClient(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListClient").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Search == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataPersonClient"] != null)
            {
                if (((MPerson)TempData["DataPersonClient"]) != null && ((MPerson)TempData["DataPersonClient"]).IdPerson > 0)
                {
                    id = ((MPerson)TempData["DataPersonClient"]).IdPerson;
                }
                else
                {
                    return RedirectToAction("ListClient", "Client");
                }
            }
            if (id == null) 
            {
                return RedirectToAction("ListClient", "Client");
            }
            //-----------------------------------------------------

            MPerson objPersonClient = new MPerson();
            objPersonClient = PersonController.fnListPerson(id, 1).First(); //1-cliente
            return View(objPersonClient);
        }

        // GET: Client/AddClient
        public ActionResult AddClient()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListClient").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Create == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            MPerson objPersonClient = new MPerson();
            objPersonClient.Birthday = DateTime.Now.Date;
            objPersonClient.listPersonContact = new List<MPersonContact>();

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objPersonClient);
        }

        // POST: Client/AddClient
        [HttpPost]
        public ActionResult AddClient(MPerson objPersonClient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //valores por defecto
                    objPersonClient.Status = true; //activo
                    objPersonClient.IdPersonType = 1; //tipo cliente

                    string mensaje = "";
                    int resultDb = PersonController.fnGNTranPerson(objPersonClient, "I", ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;
                        return RedirectToAction("AddClient");
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objPersonClient);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objPersonClient);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos del cliente: " + ex.Message;
                return View(objPersonClient);
            }
        }


        // GET: Client/EditClient/5
        public ActionResult EditClient(int? id)
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var ObjAccesUser = ((MUser)Session["Usuario"]).UserAcces;
            var ObjAcces = ObjAccesUser.Where(p => p.Action == "ListClient").First();
            if (ObjAcces != null)
            {
                if (ObjAcces.Edit == false)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            //Aqui se trae el modelo enviado por POST desde la Lista, para que no se vea en la Url
            if (TempData["DataPersonClient"] != null)
            {
                if (((MPerson)TempData["DataPersonClient"]) != null && ((MPerson)TempData["DataPersonClient"]).IdPerson > 0)
                {
                    id = ((MPerson)TempData["DataPersonClient"]).IdPerson;
                }
                else
                {
                    return RedirectToAction("ListClient", "Client");
                }
            }
            if (id == null)
            {
                return RedirectToAction("ListClient", "Client");
            }
            //-----------------------------------------------------

            MPerson objPersonClient = new MPerson();
            objPersonClient = PersonController.fnListPerson(id, 1).First(); //1-cliente

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessSave = TempData["Success"];
            }

            return View(objPersonClient);
        }

        // POST: Client/EditClient
        [HttpPost]
        public ActionResult EditClient(MPerson objPersonClient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //valores por defecto
                    objPersonClient.IdPersonType = 1; //tipo cliente

                    string mensaje = "";
                    int resultDb = PersonController.fnGNTranPerson(objPersonClient, "U", ref mensaje);

                    if (resultDb != 0)
                    {
                        TempData["Success"] = mensaje;

                        //Para evitar que se vea el id en la Url------------
                        MPerson objMPerson = new MPerson();
                        objMPerson.IdPerson = objPersonClient.IdPerson;
                        TempData["DataPersonClient"] = objMPerson;
                        return RedirectToAction("EditClient");
                        //---------------------------------------------------

                        //return RedirectToAction("EditClient", new { id = objPersonClient.IdPerson });
                    }
                    else
                    {
                        ViewBag.ErrorSave = mensaje;
                        return View(objPersonClient);
                    }
                }
                else
                {
                    ViewBag.ErrorSave = "Error al grabar, Por favor verifique los datos ingresados.";
                    return View(objPersonClient);
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorSave = "Error al grabar datos del cliente: " + ex.Message;
                return View(objPersonClient);
            }

        }


    }
}
