using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CustomerSupport.Models
{
    public class MUser
    {
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Debe indicar la Persona.")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Debe seleccionar la Persona.")]
        public int IdPerson { get; set; }

        [Required(ErrorMessage = "Debe indicar el Login.")]
        [StringLength(15, ErrorMessage = "El Login solo es de 15 Caracteres.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Debe indicar la Contraseña.")]
        [StringLength(15, ErrorMessage = "La Contraseña es de 15 caracteres.")]
        public string Password { get; set; }
        public bool Status { get; set; }
        public string StatusDesc { get; set; } //descripcion del estado (Activo/Inactivo)

        //[Required(ErrorMessage = "Debe indicar la Persona.")]
        public MPerson PersonEmployee { get; set; }

        public List<MUserAcces> UserAccesPadre { get; set; }

        public List<MUserAcces> UserAcces { get; set; }

        public string Encriptar(string input)
        {
            byte[] iv = ASCIIEncoding.ASCII.GetBytes("qualityi");
            byte[] encryptionKey = Convert.FromBase64String("rpadftlyhorfdertghyujki8765rgyhj");
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = encryptionKey;
            des.IV = iv;
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public string Desencriptar(string input)
        {
            try
            {
                byte[] iv = ASCIIEncoding.ASCII.GetBytes("qualityi");
                byte[] encryptionKey = Convert.FromBase64String("rpadftlyhorfdertghyujki8765rgyhj");
                byte[] buffer = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.Key = encryptionKey;
                des.IV = iv;
                return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception)
            {
                return input;
            }

        }

    }
}