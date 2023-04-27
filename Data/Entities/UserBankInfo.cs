using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankApplication_Web.Data.Entities
{
    public class UserBankInfo
    {
        public UserBankInfo()
        {

        }
        public UserBankInfo(string card_number, string pin, string egn, string iban)
        {
            Card_number = card_number;
            PIN = pin;
            EGN = egn;
            IBAN = iban;

        }
        public UserBankInfo(string card_number)
        {
            Card_number = card_number;

        }
        [Key]
        [ForeignKey(nameof(CreditBooleanInfo))]
        public string Card_number { get; set; }

        public string PIN { get; set; }

        [ForeignKey(nameof(UserIBANInfo))]
        public string IBAN { get; set; }

        [ForeignKey(nameof(UserInfo))]
        public string EGN { get; set; }

        public double Balance { get; set; }
    }
}
