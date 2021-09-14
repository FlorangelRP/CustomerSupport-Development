using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MCatalog
    {
        public MCatalog()
        {
            TableDetails = new List<MCatalogDetail>();
        }

        public int IdCatalog { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(50, ErrorMessage = "Nombre de Tabla no puede tener mas de 50 caracteres.")]
        public string IdTable { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(200, ErrorMessage = "Descripcion de Tabla no puede tener mas de 200 caracteres.")]
        public string Description { get; set; }

        public List<MCatalogDetail> TableDetails { get; set; }
    }
}