using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankApplication_Web.Data.Entities
{
    public class CreditDateInfo
    {
        public CreditDateInfo()
        {

        }
        public CreditDateInfo(string card_number, string credit_taken_date, string credit_toReturn_date)
        {
            Card_number = card_number;
            Credit_taken_date = credit_taken_date;
            Credit_toReturn_date = credit_toReturn_date;
        }
        [Key]
        [ForeignKey(nameof(CreditMoneyInfo))]
        public string Card_number { get; set; }


        public string Credit_taken_date { get; set; }

        public string Credit_toReturn_date { get; set; }
    }
}
