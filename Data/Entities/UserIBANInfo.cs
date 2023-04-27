using System.ComponentModel.DataAnnotations;

namespace BankApplication_Web.Data.Entities
{
    public class UserIBANInfo
    {
        public UserIBANInfo()
        {

        }
        public UserIBANInfo(string iban)
        {
            IBAN = iban;
        }
        [Key]
        public string IBAN { get; set; }
    }
}
