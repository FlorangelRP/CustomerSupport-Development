using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MPerson
    {
        public MPerson() 
        {
            listPersonContact = new List<MPersonContact>();
        }
        public int IdPerson { get; set; }
        public int IdPersonType { get; set; }
        public string PersonType { get; set; } //descripcion del tipo de persona

        [Min(1, ErrorMessage = "*Requerido")]
        public int IdIdentificationType { get; set; }        
        public string IdentificationType { get; set; } //descripcion del tipo de identificacion

        [Required(ErrorMessage = "*Requerido")]
        //[StringLength(15, ErrorMessage = "Número identificación no puede tener mas de 15 caracteres.")]
        public string NumIdentification { get; set; }


        [Required(ErrorMessage = "*Requerido")]
        [StringLength(100, ErrorMessage = "Nombres no puede tener mas de 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(100, ErrorMessage = "Apellidos no puede tener mas de 100 caracteres.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime? Birthday { get; set; } //Nullable<System.DateTime>

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(300, ErrorMessage = "Dirección no puede tener mas de 300 caracteres.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(100, ErrorMessage = "Correo electrónico no puede tener mas de 100 caracteres.")]
        [EmailAddress(ErrorMessage = "Dirección de correo electrónico no válida")]
        public string Email { get; set; }

        [Min(1, ErrorMessage = "*Requerido")]
        public int? IdContactType { get; set; } //Nullable<int>
        public string ContactType { get; set; } //descripcion de la via de contacto solo si es tipo de cliente

        [Min(1, ErrorMessage = "*Requerido")]
        public int? IdPosition { get; set; } //Nullable<int>
        public string Position { get; set; } //descripcion del cargo, solo si es tipo de persona Empleado

        public bool ClientPermission { get; set; } //debe ser visible solo para tipo persona Empleado

        public bool Status { get; set; }
        public string StatusDesc { get; set; } //descripcion del estado (Activo/Inactivo)

        [Min(1, ErrorMessage = "*Requerido")]
        public int? IdDepartment { get; set; } //Nullable<int>
        public string Department { get; set; } //descripcion del departamento, solo si es tipo de persona Empleado

        public List<MPersonContact> listPersonContact { get; set; } //Numeros de telefono de contacto de la persona
    }
}