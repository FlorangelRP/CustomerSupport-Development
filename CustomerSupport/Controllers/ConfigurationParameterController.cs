using CustomerSupport.BDContext;
using CustomerSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Controllers
{
    public class ConfigurationParameterController : Controller
    {
        // GET: ConfigurationParameter
        public ActionResult Index()
        {
            return View();
        }


        public static List<MConfigurationParameter> fnListMConfigurationParameter(int? IdConfig = null, string strAbbreviation = null)
        {


            List<MConfigurationParameter> listMConfigurationParameter = new List<MConfigurationParameter>();
            MMEnterprisesEntities db = new MMEnterprisesEntities();

            MUser objUser = new MUser();


            listMConfigurationParameter = (List<MConfigurationParameter>)(from CP in db.GNLisConfigurationParameter(IdConfig, strAbbreviation).ToList()
                                              select new MConfigurationParameter
                                              {
                                                  IdConfig=CP.IdConfig,
                                                  Description=CP.Description,
                                                  Abbreviation = CP.Abbreviation,
                                                  Value = CP.Value
                                              }).ToList();
            return listMConfigurationParameter;

        }

    }
}