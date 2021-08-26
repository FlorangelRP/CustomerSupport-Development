using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MRole
    {
        public MRole()
        {
            RoleAccesPadre = new List<MRoleAcces>();
            RoleAcces = new List<MRoleAcces>();
        }
        public int IdRole { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(100, ErrorMessage = "Nombre no puede tener mas de 60 caracteres.")]
        public string NameRole { get; set; }

        public bool Status { get; set; }
        public string StatusDesc { get; set; } //descripcion del estado (Activo/Inactivo)
        public List<MRoleAcces> RoleAccesPadre { get; set; }
        public List<MRoleAcces> RoleAcces { get; set; }
    }
}