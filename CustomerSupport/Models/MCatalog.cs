using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MCatalog
    {
        public int IdCatalog { get; set; }
        public string IdTable { get; set; }
        public string Description { get; set; }
        public List<MCatalogDetail> TableDetails { get; set; }
    }
}