using System.ComponentModel.DataAnnotations;

namespace BankApplication_Web.Data.Entities
{
    public class UserInfo
    {
        public UserInfo()
        {

        }
        public UserInfo(string egn, string first_name, string last_name, string email)
        {
            EGN = egn;
            First_name = first_name;
            Last_name = last_name;
            Email = email;
        }
        [Key]
        public string EGN { get; set; }

        public string First_name { get; set; }

        public string Last_name { get; set; }

        public string Email { get; set; }
    }
}
