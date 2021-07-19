using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class CountryController : Controller
    {
        // GET: Country
        public ActionResult ListCountry()
        {
            return View();
        }

        public ActionResult GetListCountry()
        {
            List<MCountry> ListCountry = new List<MCountry>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            ListCountry = (from result in db.GNListCountry(null,null).ToList()
                        select new MCountry
                        {
                            IdCountry = result.IdCountry,
                            IdIsoCountry = result.IdIsoCountry,
                            Country = result.Country,
                            CountryAreaCode = result.CountryAreaCode,
                            Status = result.Status,
                            StatusDesc = result.Status == true ? "Activo" : "Inactivo"
                        }).ToList();

            return Json(ListCountry, JsonRequestBehavior.AllowGet);

        }

        // GET: Country/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Country/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Country/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Country/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Country/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Country/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Country/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
