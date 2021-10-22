using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MAssociatePosition
    {
        public int IdPosition { get; set; }
        public int IdAssociate { get; set; }
        public string PositionDesc { get; set; } //descripcion del cargo principal
        public string AssociateDesc { get; set; } //descripcion del cargo asociado

    }
}