using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MTask
    {
        public MTask()
        {
            listTaskPerson = new List<MTaskPerson>();
        }

        public int IdTask { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista
        public string UserLastName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista

        [StringLength(500, ErrorMessage = "La especificación de actividad no puede tener mas de 500 caracteres.")]
        public string Activity { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime DateIni { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public System.DateTime DateEnd { get; set; }

        public System.TimeSpan HourIni { get; set; }
        public System.TimeSpan HourEnd { get; set; }

        [StringLength(300, ErrorMessage = "La especificación del lugar no puede tener mas de 300 caracteres.")]
        public string Place { get; set; }

        public bool Status { get; set; }
        public List<MTaskPerson> listTaskPerson { get; set; } //Esta lista es para los involucrados en la actividad

        //Estos datos opcionales son para el caso de Cita en Solicitud de Servicio, que solo se puede incluir una persona
        public int? IdPersonEmployee { get; set; }
        public string PersonEmployeeName { get; set; } 
        public string PersonEmployeeLastName { get; set; } 
        //---------------------------------------------------------------------------------------------------------------

    }

    public class MTaskPerson
    {
        public int IdPersonEmployee { get; set; }
        public string PersonEmployeeName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista
        public string PersonEmployeeLastName { get; set; } //opcional, se susa solo cuando se vaya a mostrar los datos en la vista
    }

}