//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomerSupport.BDContext
{
    using System;
    
    public partial class GNListTask_Result
    {
        public int IdTask { get; set; }
        public int IdUser { get; set; }
        public System.DateTime DateIni { get; set; }
        public System.DateTime DateEnd { get; set; }
        public System.TimeSpan HourIni { get; set; }
        public System.TimeSpan HourEnd { get; set; }
        public string Place { get; set; }
        public Nullable<int> IdStatus { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public Nullable<int> IdFatherTask { get; set; }
        public string Tittle { get; set; }
        public Nullable<int> IdServiceRequest { get; set; }
        public Nullable<int> IdTypeTask { get; set; }
        public string TypeTask { get; set; }
        public Nullable<int> IdPriority { get; set; }
        public string PriorityTask { get; set; }
        public Nullable<int> IdResponsable { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Activity { get; set; }
        public Nullable<bool> confidential { get; set; }
    }
}
