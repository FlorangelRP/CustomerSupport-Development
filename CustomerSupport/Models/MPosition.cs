using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MPosition
    {
        public MPosition()
        {
            listAssociatePosition = new List<MAssociatePosition>();
        }

        public int IdPosition { get; set; }
        public string Description { get; set; }
        public int IdDepartment { get; set; }
        public string Department { get; set; } //descripcion del departamento
        public int IdPositionLevel { get; set; }
        public string PositionLevel { get; set; } //descripcion del nivel
        public bool Status { get; set; }
        public string StatusDesc { get; set; } //descripcion del estado (Activo/Inactivo)
        public List<MAssociatePosition> listAssociatePosition { get; set; }
    }
}