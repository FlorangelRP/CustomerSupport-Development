using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CustomerSupport.Models
{
    [Serializable]
    public class MSerUser
    {
        public int IdUser { get; set; }
        public string Login { get; set; } 
        public MSerPerson PersonEmployee { get; set; }
        public List<MSerAcces> UserAccesPadre { get; set; }
        public List<MSerAcces> UserAcces { get; set; }
        public List<MSerRole> Roles { get; set; }
        public string UserRolesNames { get; set; } //opcional, para convertir la lista de roles del usuario en un string de nombres de roles. 

    }

    [Serializable]
    public class MSerPerson
    {
        public int IdPerson { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    [Serializable]
    public class MSerRole
    {
        public int IdRole { get; set; }
        public string NameRole { get; set; }
        public bool Status { get; set; }
        public string StatusDesc { get; set; } 
        public List<MSerAcces> RoleAccesPadre { get; set; }
        public List<MSerAcces> RoleAcces { get; set; }
    }

    [Serializable]
    public class MSerAcces
    {
        public int? IdOption { get; set; }
        public string OptionName { get; set; }
        public bool Visible { get; set; }
        public bool Create { get; set; }
        public bool Search { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int? IdAssociated { get; set; }
    }
}