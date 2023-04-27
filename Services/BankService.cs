using BankApplication_Web.Data;
using BankApplication_Web.Data.Entities;

namespace Bank_Web_App.Services
{
    public class BankService
    {
        private BankContext context;

        public BankService(BankContext context)
        {
            this.context = context;
        }
       
        public void WithdrawDeposit(UserBankInfo userBankInfo)
        {
            context.UserBankInfos.Update(userBankInfo);
            context.SaveChanges();
        }
        public double Transfer(UserBankInfo userBankInfo, string ibanReceiving, double transferAmount)
        {
            userBankInfo = context.UserBankInfos.FirstOrDefault(p => p.IBAN == userBankInfo.IBAN);
            userBankInfo.Balance -= transferAmount;
            context.UserBankInfos.Update(userBankInfo);
            context.SaveChanges();

            UserBankInfo userReceiving = context.UserBankInfos.FirstOrDefault(p => p.IBAN == ibanReceiving);
            userReceiving.Balance += transferAmount;
            context.UserBankInfos.Update(userReceiving);
            context.SaveChanges();
            return userBankInfo.Balance;
        }
        public bool DoesIBANExist(UserIBANInfo userIBANInfo)
        {
            return context.UserIBANInfos.Contains(userIBANInfo);
        }
        public CreditDateInfo CalculateCreditDateInfos(string creditChoice, string card_number)//changed
        {

            DateTime currentDate = DateTime.Now.Date;
            DateTime dateAfterOneYear = currentDate.AddYears(1);
            DateTime dateAfterSixMonths = currentDate.AddMonths(6);
            DateTime dateAfterThreeMonths = currentDate.AddMonths(3);
            string credit_taken_date = currentDate.ToString("yyyy-MM-dd");
            string credit_ToReturn_date = string.Empty;

            if (creditChoice == "credit1Button")
            {
                credit_ToReturn_date = dateAfterOneYear.ToString("yyyy-MM-dd");
            }
            else if (creditChoice == "credit2Button")
            {
                credit_ToReturn_date = dateAfterSixMonths.ToString("yyyy-MM-dd");
            }
            else if (creditChoice == "credit3Button")
            {
                credit_ToReturn_date = dateAfterThreeMonths.ToString("yyyy-MM-dd");
            }

            return new CreditDateInfo(card_number, credit_taken_date, credit_ToReturn_date);
        }
        public CreditMoneyInfo CalculateCreditMoneyInfos(string creditChoice, string card_number)//changed
        {
            double creditAmount = 0;
            double creditInterest = 0;
            if (creditChoice == "credit1Button")
            {
                creditAmount = 1000;
                creditInterest = 0.03;
            }
            else if (creditChoice == "credit2Button")
            {
                creditAmount = 500;
                creditInterest = 0.04;
            }
            else if (creditChoice == "credit3Button")
            {
                creditAmount = 250;
                creditInterest = 0.05;
            }
            double creditToBePaid = creditAmount + (creditAmount * creditInterest);
            return new CreditMoneyInfo(card_number, creditAmount, creditInterest, creditToBePaid);
        }
        public void TakeCredit(UserBankInfo userBankInfo, CreditBooleanInfo creditBooleanInfo, CreditDateInfo creditDateInfo, CreditMoneyInfo creditMoneyInfo)
        {
            context.UserBankInfos.Update(userBankInfo);
            context.CreditBooleanInfos.Update(creditBooleanInfo);
            context.CreditDateInfos.Add(creditDateInfo);
            context.CreditMoneyInfos.Add(creditMoneyInfo);
            context.SaveChanges();
        }
        public void PayCredit(UserBankInfo userBankInfo, CreditBooleanInfo creditBooleanInfo, CreditDateInfo creditDateInfo, CreditMoneyInfo creditMoneyInfo)
        {
            context.UserBankInfos.Update(userBankInfo);
            context.CreditBooleanInfos.Update(creditBooleanInfo);
            context.CreditDateInfos.Remove(creditDateInfo);
            context.CreditMoneyInfos.Remove(creditMoneyInfo);
            context.SaveChanges();
        }




    }
}
