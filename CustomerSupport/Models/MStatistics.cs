using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MStatistics
    {
        public int NroEmployee { get; set; }
        public int NroClient { get; set; }
        public int ServicesProcess { get; set; }
        public int servicesCompleted { get; set; }
    }

    public class Mperformace
    {
        public bool XEmployee { get; set; }
        public int? IdEmployee { get; set; }
        public bool XDepartament { get; set; }
        public int? IdDepartment { get; set; }
        public bool XMonth { get; set; }
        public int? Year { get; set; }
        public bool XYear { get; set; }
        public int YearIni { get; set; }
        public int YearEnd { get; set; }
        public bool XDate { get; set; }
        public DateTime? DateIni { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool Xtype { get; set; }

        public int? IdTypeTask { get; set; }

    }
}