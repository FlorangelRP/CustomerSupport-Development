using System;
using System.Collections.Generic;
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
            return View();
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

        // GET: TableCatalog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TableCatalog/Create
        public ActionResult AddTableCatalog()
        {
            return View();
        }

        // POST: TableCatalog/Create
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

        // GET: TableCatalog/Edit/5int id
        public ActionResult EditTableCatalog()
        {

            return View();
        }

        // POST: TableCatalog/Edit/5
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

        // GET: TableCatalog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TableCatalog/Delete/5
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
