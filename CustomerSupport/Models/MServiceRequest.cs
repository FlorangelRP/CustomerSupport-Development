using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MServiceRequest
    {
        public MServiceRequest()
        {
            listConstructionOption = new List<MServiceConstructionOption>();
            listTask = new List<MTask>();
            listServiceRequestAssets = new List<MServiceRequestAssets>();
        }

        public int IdServiceRequest { get; set; }

        [Min(1, ErrorMessage = "Indique el tipo de servicio.")]
        public int IdServiceType { get; set; }
        public string ServiceType { get; set; } //descripcion del tipo de servicio

        [Min(1, ErrorMessage = "Indique el Estado del servicio.")]
        public int IdServiceStatus { get; set; }
        public string ServiceStatus { get; set; } //descripcion del estatus del servicio

        [Required(ErrorMessage = "Indique el Cliente.")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Indique el Cliente.")]
        public int IdPerson { get; set; }
        public MPerson PersonClient { get; set; }

        [Min(1, ErrorMessage = "Indique la via de contacto.")]
        public Nullable<int> IdContactType { get; set; }
        public string ContactType { get; set; } //descripcion de la via de contacto

        public Nullable<int> IdPropertyType { get; set; }
        public string PropertyType { get; set; } //descripcion de tipo de propiedad

        [StringLength(300, ErrorMessage = "Dirección no puede tener mas de 300 caracteres.")]
        public string Address { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> Price { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> DownPayment { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> ClosingCost { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> MonthlyIncome { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> DebtPayment { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> Piti { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> Ratios { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> EstimatedValue { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> LoanAmount { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> CurrentDebt { get; set; }

        [StringLength(500, ErrorMessage = "La especificación de bienes no puede tener mas de 500 caracteres.")]
        public string Assets { get; set; }

        [StringLength(500, ErrorMessage = "La especificación del proceso no puede tener mas de 500 caracteres.")]
        public string Process { get; set; }

        [StringLength(500, ErrorMessage = "La especificación de deseos no puede tener mas de 500 caracteres.")]
        public string Wish { get; set; }

        public Nullable<bool> Plane { get; set; }
        public Nullable<bool> Financing { get; set; }

        [StringLength(4000, ErrorMessage = "Las notas no puede tener mas de 4000 caracteres.")]
        public string Note { get; set; }

        public int IdUser { get; set; }
        public string RegisterUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime RegisterDate { get; set; }

        [StringLength(1000, ErrorMessage = "La especificación de beneficiarios no puede tener mas de 1000 caracteres.")]
        public string LivT_Beneficiaries { get; set; }
        [StringLength(8000, ErrorMessage = "La condicion de herencia no puede tener mas de 8000 caracteres.")]
        public string LivT_InheritanceCondition { get; set; }
        public string LivT_ResponsibleAgent1 { get; set; }
        public string LivT_ResponsibleAgent2 { get; set; }
        public Nullable<bool> LivT_AuthorizeRespAgent { get; set; } 
        public Nullable<bool> LivT_ProlongLife { get; set; }
        public Nullable<bool> LivT_DonateOrgans { get; set; }
        public string LivT_Organs { get; set; }
        public bool LivT_OrganForTransplant { get; set; } //Nullable<bool>
        public bool LivT_OrganForTherapy { get; set; } //Nullable<bool>
        public bool LivT_OrganForEducation { get; set; } //Nullable<bool>
        public bool LivT_OrganForInvestigation { get; set; } //Nullable<bool>
        public string LivT_GpNames { get; set; }
        public string LivT_GpPhoneNumber { get; set; }
        public string LivT_GpAddress { get; set; }
        public List<MServiceRequestAssets> listServiceRequestAssets { get; set; } //lista de bienes del servicio living trust

        public List<MServiceConstructionOption> listConstructionOption { get; set; } //lista las opciones de construccion para tipo de servicios Construccion/Planos
        public List<MTask> listTask { get; set; } //para la cita, pero luego podrian ser varias actividades
    }
}