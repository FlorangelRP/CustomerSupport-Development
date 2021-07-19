using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MServiceConstructionOption
    {
        public int IdServiceRequest { get; set; }
        public int IdConstructionOption { get; set; }
        public string ConstructionOption { get; set; } //descripcion de la opcion construccion

    }
}