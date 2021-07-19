using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CustomerSupport.Models
{
    public class MPersonContact
    {
        public int IdContact { get; set; }
        public int IdPerson { get; set; }
        public int IdPhoneNumberType { get; set; }
        public string PhoneNumberType { get; set; } //descripcion del tipo de telefono
        public string IdIsoCountry { get; set; }
        public string CountryAreaCode { get; set; } //codigo de area segun el pais seleccionado
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }

        //public string Desencriptar(string input)
        //{
        //    try
        //    {
        //        byte[] iv = ASCIIEncoding.ASCII.GetBytes("qualityi");
        //        byte[] encryptionKey = Convert.FromBase64String("rpadftlyhorfdertghyujki8765rgyhj");
        //        byte[] buffer = Convert.FromBase64String(input);
        //        TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        //        des.Key = encryptionKey;
        //        des.IV = iv;
        //        return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        //    }
        //    catch (Exception)
        //    {
        //        return input;
        //    }

        //}

    }
}