using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MPerformance
    {
        public string Nombre { get; set; }
        public string StatusTask { get; set; }
        public int Cantidad { get; set; }
    }

    public class MEstadistica
    {
        public List<string> Nombre { get; set; }

        public DataTable Data { get; set; }
    }
}