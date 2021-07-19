using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MUserAcces
    {
        public int? IdOption { get; set; }
        public string OptionName { get; set; }
        //public int? IdAssociated { get; set; }
        public bool Visible { get; set; }
        public bool Create { get; set; }
        public bool Search { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int? IdAssociated { get; set; }
    }
}