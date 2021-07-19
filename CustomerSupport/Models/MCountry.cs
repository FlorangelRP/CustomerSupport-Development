using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MCountry
    {
        public int IdCountry { get; set; }
        public string IdIsoCountry { get; set; }
        public string Country { get; set; }
        public string CountryAreaCode { get; set; }
        public bool Status { get; set; }
        public string StatusDesc { get; set; }

    }
}