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
}