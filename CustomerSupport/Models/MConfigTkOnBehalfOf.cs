using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MConfigTkOnBehalfOf
    {
        public int IdConfig { get; set; }
        public int IdUserOnBehalfOf { get; set; }
        public string NumIdentificationOnBehalfOf { get; set; }
        public string NameOnBehalfOf { get; set; }
        public string LastNameOnBehalfOf { get; set; }
        public int IdUser { get; set; }
        public string NumIdentification { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> Status { get; set; } //opcional. Se usa para control de eliminacion en Datatable de la view.
    }
}