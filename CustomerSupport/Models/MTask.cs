using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerSupport.Models
{
    public class MTask
    {
        public MTask()
        {
            listTaskPerson = new List<MTaskPerson>();
        }


        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int IdTask { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista
        public string UserLastName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista

        //[StringLength(500, ErrorMessage = "La especificación de actividad no puede tener mas de 500 caracteres.")]
        //public string Activity { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime? DateIni { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime? DateEnd { get; set; }

        public System.TimeSpan HourIni { get; set; }
        public System.TimeSpan HourEnd { get; set; }

        [StringLength(300, ErrorMessage = "La especificación del lugar no puede tener mas de 300 caracteres.")]
        public string Place { get; set; }

        public int? IdFatherTask { get; set; }
        public List<MTaskPerson> listTaskPerson { get; set; } //Esta lista es para los involucrados en la actividad

        //Estos datos opcionales son para el caso de Cita en Solicitud de Servicio, que solo se puede incluir una persona
        [Min(1, ErrorMessage = "*Requerido")]
        public int? IdPersonEmployee { get; set; }
        public string PersonEmployeeName { get; set; }
        public string PersonEmployeeLastName { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(500, ErrorMessage = "El título no puede tener mas de 500 caracteres.")]
        public string Tittle { get; set; }

        [Required(ErrorMessage = "*Requerido")]
        [StringLength(500, ErrorMessage = "El título no puede tener mas de 500 caracteres.")]
        public string Activity { get; set; }

        [Min(1, ErrorMessage = "*Requerido")]
        public int? IdPriority { get; set; }
        public string PriorityTask { get; set; }


        [Min(1, ErrorMessage = "*Requerido")]
        public int? IdTypeTask { get; set; }
        public string TypeTask { get; set; }


        [Min(1, ErrorMessage = "*Requerido")]
        public int? IdStatus { get; set; }
        public string Status { get; set; }

        public int? IdServiceRequest { get; set; }
      
        public List<MTaskComment> listMTaskComment { get; set; } //Esta lista es para los Comentarios en la actividad

        public bool Confidential { get; set; }
        //public List<MBitacora> listMBitacora { get; set; } //Solo para editar

        //---------------------------------------------------------------------------------------------------------------

    }

    public class MTaskPerson
    { 
        public int IdPersonEmployee { get; set; }
        public string PersonEmployeeName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista
        public string PersonEmployeeLastName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista
        public string NumIdentification { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista
    }

    public class MTaskComment
    {
        public int? IdComment { get; set; }
        public int? IdTask { get; set; }

        [AllowHtml]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        public int? IdUser { get; set; }
        public DateTime? Date { get; set; }
        public string DateOperation { get; set; }
        public string UserName { get; set; }
        public int New { get; set; }

    }


    //public class MTaskFilter
    //{
    //    public int IdTask { get; set; }
    //    public System.DateTime DateIni { get; set; }

    //    public System.DateTime DateEnd { get; set; }

    //    public int IdTypeTask { get; set; }

    //    public int IdPriority { get; set; }

    //    public int IdStatus { get; set; }

    //    public int IdPersonEmployee { get; set; }

    //    public string Tittle { get; set; }
    //}
}