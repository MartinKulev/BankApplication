using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankApplication_Web.Data.Entities
{
    public class CreditBooleanInfo
    {
        public CreditBooleanInfo()
        {

        }
        public CreditBooleanInfo(string card_number, bool has_taken_credit)
        {
            Card_number = card_number;
            Has_taken_credit = has_taken_credit;
        }
        [Key]
        [ForeignKey(nameof(CreditDateInfo))]
        public string Card_number { get; set; }

        public bool Has_taken_credit { get; set; }
    }
}
