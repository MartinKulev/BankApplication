using BankApplication_Web.Data.Entities;
using BankApplication_Web.Data;

namespace Bank_Web_App.Services
{
    public class UserService
    {
        private BankContext context;

        public UserService(BankContext context)
        {
            this.context = context;
        }

        public void RegisterUser(UserInfo userInfo, UserBankInfo userBankInfo, CreditBooleanInfo creditBooleanInfo, UserIBANInfo userIBANInfo)
        {
            context.UserInfos.Add(userInfo);
            context.UserBankInfos.Add(userBankInfo);
            context.CreditBooleanInfos.Add(creditBooleanInfo);
            context.UserIBANInfos.Add(userIBANInfo);
            context.SaveChanges();
        }
        public bool DoesEGNExist(UserInfo userInfo)
        {
            return context.UserInfos.Contains(userInfo);
        }
        public string CreateRandomCardNumber()
        {
            Random random = new Random();
            string card_number1 = random.Next(1000, 9999).ToString();
            string card_number2 = random.Next(1000, 9999).ToString();
            string card_number3 = random.Next(1000, 9999).ToString();
            string card_number4 = random.Next(1000, 9999).ToString();

            string card_number = card_number1 + "-" + card_number2 + "-" + card_number3 + "-" + card_number4;
            return card_number;
        }
        public string CreateRandomIBAN()
        {
            string ibanCountry = "BG";
            Random random = new Random();
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string word = "";

            for (int i = 0; i < 16; i++)
            {
                int index = random.Next(alphabet.Length);
                char letter = alphabet[index];
                word += letter;
            }

            return ibanCountry + word + "00";
        }
        public UserBankInfo LogInUserInto1stTable(string card_number)
        {
            return context.UserBankInfos.FirstOrDefault(p => p.Card_number == card_number);
        }
        public UserInfo LogInUserInto2ndTable(string egn)
        {
            return context.UserInfos.FirstOrDefault(p => p.EGN == egn);
        }
        public CreditBooleanInfo LogInUserInto3rdTable(string card_number)
        {
            return context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == card_number);
        }
        public CreditDateInfo LogInUserInto4thTable(string card_number)
        {
            return context.CreditDateInfos.FirstOrDefault(p => p.Card_number == card_number);
        }
        public CreditMoneyInfo LogInUserInto5thTable(string card_number)
        {
            return context.CreditMoneyInfos.FirstOrDefault(p => p.Card_number == card_number);
        }       
        public bool DoesCardNumberExist(UserBankInfo userBankInfo)
        {
            return context.UserBankInfos.Contains(userBankInfo);
        }


    }
}
