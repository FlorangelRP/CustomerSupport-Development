using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MCatalogDetail
    {
        public int IdCatalogDetail { get; set; }
        public string IdTableDetail { get; set; }
        public int IdCatalog { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string StatusDesc { get; set; } //descripcion del estado (Activo/Inactivo)
    }
}