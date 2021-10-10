using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MCalendar
    {
        public int IdTask { get; set; }
        public string Tittle { get; set; }
        public string Activity { get; set; }
        public string DateIni { get; set; }
        public string DateEnd { get; set; }
        public string HourIni { get; set; }
        public string HourEnd { get; set; }
        public string StatusTask { get; set; }

    }
}
