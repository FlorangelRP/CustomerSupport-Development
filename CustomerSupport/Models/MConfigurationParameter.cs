using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MConfigurationParameter
    {
        public int IdConfig { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Value { get; set; }
    }
}