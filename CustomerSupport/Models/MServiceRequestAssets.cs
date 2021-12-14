using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerSupport.Models
{
    public class MServiceRequestAssets
    {
        public int IdAsset { get; set; }
        public int IdServiceRequest { get; set; }
        public int IdAssetsType { get; set; }
        public string AssetsType { get; set; } //Descripcion del tipo de bien
        public string Description { get; set; }
        public string Beneficiaries { get; set; }
        public string Administrators { get; set; }
        public Nullable<bool> Status { get; set; }

    }
}