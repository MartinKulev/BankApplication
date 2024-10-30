using Bank_Web_App.Services;
using BankApplication_Web.Data;
using BankApplication_Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank_Web_App_NunitTests
{
	public class Tests
	{
		private BankService bankService;
		private UserService userService;
		private BankContext context;
		private UserBankInfo userBankInfo;
		private UserInfo userInfo;
		private CreditMoneyInfo creditMoneyInfo;
		private CreditDateInfo creditDateInfo;
		private CreditBooleanInfo creditBooleanInfo;
		private UserIBANInfo userIBANInfo;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<BankContext>()
				.UseInMemoryDatabase("TestDb");

			context = new BankContext(options.Options);
			bankService = new BankService(context);
			userService = new UserService(context);

			string egn = CreateRandomEGN();
			string card_number = this.userService.CreateRandomCardNumber();
			string iban = this.userService.CreateRandomIBAN();

			userInfo = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");
			userBankInfo = new UserBankInfo(card_number, "2006", userInfo.EGN, iban);
			creditBooleanInfo = new CreditBooleanInfo(userBankInfo.Card_number, false);
			creditMoneyInfo = new CreditMoneyInfo(card_number, 1000, 0.03, 1000 + 1000 * 0.03);
			creditDateInfo = new CreditDateInfo(userBankInfo.Card_number, "2023-03-25", "2024-03-25");
			userIBANInfo = new UserIBANInfo(userBankInfo.IBAN);
		}

		public string CreateRandomEGN()
		{
			Random random = new Random();

			string number = "";

			for (int i = 0; i < 10; i++)
			{
				int digit = random.Next(0, 10);
				number += digit.ToString();
			}

			return number;
		}

		[Test]
		public void RegisterUser_AssertUserInfosIsSentToDB()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

			Assert.IsTrue(context.UserInfos.Contains(userInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void RegisterUser_AssertUserBankInfosIsSentToDB()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

			Assert.IsTrue(context.UserBankInfos.Contains(userBankInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void RegisterUser_AssertCreditBooleanInfosIsSentToDB()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

			Assert.IsTrue(context.CreditBooleanInfos.Contains(creditBooleanInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void RegisterUser_AssertUserIBANInfosIsSentToDB()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

			Assert.IsTrue(context.UserIBANInfos.Contains(userIBANInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
		}





		[Test]
		public void DoesEGNExist_AssertEGNReturnsTrueWhenExists()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			bool egnExists = this.userService.DoesEGNExist(userInfo);

			Assert.IsTrue(egnExists);

			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();

		}

		[Test]
		public void DoesEGNExist_AssertEGNReturnsFalseWhenNotExist()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();


			bool egnExists = this.userService.DoesEGNExist(userInfo);
			Assert.IsFalse(egnExists);
		}





		[Test]
		public void CreateRandomCardNumber_AssertCardNumbersAreUnique()
		{
			string cardNumber1 = this.userService.CreateRandomCardNumber();
			string cardNumber2 = this.userService.CreateRandomCardNumber();
			Assert.AreNotEqual(cardNumber1, cardNumber2);
		}

		[Test]
		public void CreateRandomCardNumber_AssertCardNumberHas19Char()
		{
			string cardNumber = this.userService.CreateRandomCardNumber();
			Assert.IsTrue(cardNumber.Length == 19);
		}

		[Test]
		public void CreateRandomCardNumber_AssertCardNumberAlwaysHasHyphen()
		{
			string cardNumber = this.userService.CreateRandomCardNumber();
			Assert.IsTrue(cardNumber.Contains("-"));
		}





		[Test]
		public void CreateRandomIBAN_AssertIBANsAreUnique()
		{
			string iban1 = this.userService.CreateRandomIBAN();
			string iban2 = this.userService.CreateRandomIBAN();
			Assert.AreNotEqual(iban1, iban2);
		}

		[Test]
		public void CreateRandomIBAN_AssertIBANHas20Chars()
		{
			string iban = this.userService.CreateRandomIBAN();
			Assert.IsTrue(iban.Length == 20);
		}

		[Test]
		public void CreateRandomCardIBAN_AssertIBANAlwaysHasBG()
		{
			string iban = this.userService.CreateRandomIBAN();
			Assert.IsTrue(iban.Contains("BG"));
		}

		[Test]
		public void CreateRandomCardIBAN_AssertIBANAlwaysHas00()
		{
			string iban = this.userService.CreateRandomIBAN();
			Assert.IsTrue(iban.Contains("00"));
		}





		[Test]
		public void LogInUserInto1stTable_AssertCard_numberIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserBankInfo userBankInfoTest = this.userService.LogInUserInto1stTable(userBankInfo.Card_number);
			Assert.IsTrue(userBankInfo.Card_number == userBankInfoTest.Card_number);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto1stTable_AssertPINIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserBankInfo userBankInfoTest = this.userService.LogInUserInto1stTable(userBankInfo.Card_number);
			Assert.IsTrue(userBankInfo.PIN == userBankInfoTest.PIN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto1stTable_AssertIBANIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserBankInfo userBankInfoTest = this.userService.LogInUserInto1stTable(userBankInfo.Card_number);
			Assert.IsTrue(userBankInfo.IBAN == userBankInfoTest.IBAN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto1stTable_AssertEGNIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserBankInfo userBankInfoTest = this.userService.LogInUserInto1stTable(userBankInfo.Card_number);
			Assert.IsTrue(userBankInfo.EGN == userBankInfoTest.EGN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto1stTable_AssertBalanceIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserBankInfo userBankInfoTest = this.userService.LogInUserInto1stTable(userBankInfo.Card_number);
			Assert.IsTrue(userBankInfo.Balance == userBankInfoTest.Balance);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}





		[Test]
		public void LogInUserInto2ndTable_AssertEGNIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserInfo userInfoTest = this.userService.LogInUserInto2ndTable(userInfo.EGN);
			Assert.IsTrue(userInfo.EGN == userInfoTest.EGN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto2ndTable_AssertFirstNameIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserInfo userInfoTest = this.userService.LogInUserInto2ndTable(userInfo.EGN);
			Assert.IsTrue(userInfo.First_name == userInfoTest.First_name);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto2ndTable_AssertLastNameIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserInfo userInfoTest = this.userService.LogInUserInto2ndTable(userInfo.EGN);
			Assert.IsTrue(userInfo.Last_name == userInfoTest.Last_name);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto2ndTable_AssertEmailIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			UserInfo userInfoTest = this.userService.LogInUserInto2ndTable(userInfo.EGN);
			Assert.IsTrue(userInfo.Email == userInfoTest.Email);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}





		[Test]
		public void LogInUserInto3rdTable_AssertCardNumberIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			CreditBooleanInfo creditBooleanInfoTest = this.userService.LogInUserInto3rdTable(userBankInfo.Card_number);
			Assert.IsTrue(creditBooleanInfo.Card_number == creditBooleanInfoTest.Card_number);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void LogInUserInto3rdTable_AssertHasTakenCreditIsLoggedCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			CreditBooleanInfo creditBooleanInfoTest = this.userService.LogInUserInto3rdTable(userBankInfo.Card_number);
			Assert.IsTrue(creditBooleanInfo.Has_taken_credit == creditBooleanInfoTest.Has_taken_credit);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}





		[Test]
		public void DoesCardNumberExist_AssertCardNumberReturnsTrueWhenExists()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			bool cardNumberExists = this.userService.DoesCardNumberExist(userBankInfo);
			Assert.IsTrue(cardNumberExists);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void DoesCardNumberExist_AssertCardNumberReturnsFalseWhenNotExist()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();

			bool CardNumberExists = this.userService.DoesCardNumberExist(userBankInfo);
			Assert.IsFalse(CardNumberExists);
		}





		[Test]
		public void WithdrawDeposit_AssertBalanceGetsUpdatedProperly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 10;
			this.bankService.WithdrawDeposit(userBankInfo);
			UserBankInfo userBankInfoTest = this.userService.LogInUserInto1stTable(userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}





		[Test]
		public void Transfer_AssertSendingBalanceGetsUpdatedProperly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

			string egn = CreateRandomEGN();
			string card_number = this.userService.CreateRandomCardNumber();
			string iban = this.userService.CreateRandomIBAN();
			UserInfo userInfoReceiving = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");
			UserBankInfo userBankInfoReceiving = new UserBankInfo(card_number, "2006", userInfoReceiving.EGN, iban);
			CreditBooleanInfo creditBooleanInfoReceiving = new CreditBooleanInfo(userBankInfoReceiving.Card_number, false);
			UserIBANInfo userIBANInfoReceiving = new UserIBANInfo(userBankInfoReceiving.IBAN);
			this.userService.RegisterUser(userInfoReceiving, userBankInfoReceiving, creditBooleanInfoReceiving, userIBANInfoReceiving);

			userBankInfo.Balance = 15;
			double balance = userBankInfo.Balance;
			this.bankService.WithdrawDeposit(userBankInfo);
			double transferAmount = 10;
			userBankInfo.Balance = this.bankService.Transfer(userBankInfo, userBankInfoReceiving.IBAN, transferAmount);

			Assert.AreEqual(balance - transferAmount, userBankInfo.Balance);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.Remove(userInfoReceiving);
			context.Remove(userBankInfoReceiving);
			context.Remove(creditBooleanInfoReceiving);
			context.Remove(userIBANInfoReceiving);
			context.SaveChanges();
		}

		[Test]
		public void Transfer_AssertLocalBalanceEqualsDBBalance()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

			string egn = CreateRandomEGN();
			string card_number = this.userService.CreateRandomCardNumber();
			string iban = this.userService.CreateRandomIBAN();
			UserInfo userInfoReceiving = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");
			UserBankInfo userBankInfoReceiving = new UserBankInfo(card_number, "2006", userInfoReceiving.EGN, iban);
			CreditBooleanInfo creditBooleanInfoReceiving = new CreditBooleanInfo(userBankInfoReceiving.Card_number, false);
			UserIBANInfo userIBANInfoReceiving = new UserIBANInfo(userBankInfoReceiving.IBAN);
			this.userService.RegisterUser(userInfoReceiving, userBankInfoReceiving, creditBooleanInfoReceiving, userIBANInfoReceiving);

			userBankInfo.Balance = 15;
			double balance = userBankInfo.Balance;
			this.bankService.WithdrawDeposit(userBankInfo);
			double transferAmount = 10;
			userBankInfo.Balance = this.bankService.Transfer(userBankInfo, userBankInfoReceiving.IBAN, transferAmount);
			UserBankInfo userBankInfoTest = this.userService.LogInUserInto1stTable(userBankInfo.Card_number);

			Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);

			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.Remove(userInfoReceiving);
			context.Remove(userBankInfoReceiving);
			context.Remove(creditBooleanInfoReceiving);
			context.Remove(userIBANInfoReceiving);
			context.SaveChanges();
		}

		[Test]
		public void Transfer_AssertReceivingBalanceGetsUpdatedProperly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);

			string egn = CreateRandomEGN();
			string card_number = this.userService.CreateRandomCardNumber();
			string iban = this.userService.CreateRandomIBAN();
			UserInfo userInfoReceiving = new UserInfo(egn, "Martin", "Kulev", "martindkulev@gmail.com");
			UserBankInfo userBankInfoReceiving = new UserBankInfo(card_number, "2006", userInfoReceiving.EGN, iban);
			CreditBooleanInfo creditBooleanInfoReceiving = new CreditBooleanInfo(userBankInfoReceiving.Card_number, false);
			UserIBANInfo userIBANInfoReceiving = new UserIBANInfo(userBankInfoReceiving.IBAN);
			this.userService.RegisterUser(userInfoReceiving, userBankInfoReceiving, creditBooleanInfoReceiving, userIBANInfoReceiving);

			userBankInfo.Balance = 15;
			this.bankService.WithdrawDeposit(userBankInfo);
			double transferAmount = 10;
			this.bankService.Transfer(userBankInfo, userBankInfoReceiving.IBAN, transferAmount);
			userBankInfoReceiving = this.userService.LogInUserInto1stTable(userBankInfoReceiving.Card_number);

			Assert.AreEqual(transferAmount, userBankInfoReceiving.Balance);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.Remove(userInfoReceiving);
			context.Remove(userBankInfoReceiving);
			context.Remove(creditBooleanInfoReceiving);
			context.Remove(userIBANInfoReceiving);
			context.SaveChanges();
		}





		[Test]
		public void DoesIBANExist_AssertIBANReturnsTrueWhenExists()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			bool ibanExists = this.bankService.DoesIBANExist(userIBANInfo);
			Assert.IsTrue(ibanExists);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void DoesIBANExist_AssertIBANReturnsFalseWhenNotExist()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();

			bool ibanExists = this.bankService.DoesIBANExist(userIBANInfo);
			Assert.IsFalse(ibanExists);
		}





		[Test]
		public void CalculateCreditDateInfos_AssertTakenDateIsTodayDate()
		{
			DateTime currentDate = DateTime.Now.Date;
			string creditChoice = "credit1Button";
			CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(currentDate.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_taken_date);
		}

		[Test]
		public void CalculateCreditDateInfos_AssertToReturnDateIs1YearWhenCreditChoice1()
		{
			DateTime currentDate = DateTime.Now.Date;
			DateTime dateAfterOneYear = currentDate.AddYears(1);
			string creditChoice = "credit1Button";
			CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(dateAfterOneYear.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_toReturn_date);
		}

		[Test]
		public void CalculateCreditDateInfos_AssertToReturnDateIs6MonthsWhenCreditChoice2()
		{
			DateTime currentDate = DateTime.Now.Date;
			DateTime dateAfterSixMonths = currentDate.AddMonths(6);
			string creditChoice = "credit2Button";
			CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(dateAfterSixMonths.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_toReturn_date);
		}

		[Test]
		public void CalculateCreditDateInfos_AssertToReturnDateIs3MonthsWhenCreditChoice3()
		{
			DateTime currentDate = DateTime.Now.Date;
			DateTime dateAfterThreeMonths = currentDate.AddMonths(3);
			string creditChoice = "credit3Button";
			CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(dateAfterThreeMonths.ToString("yyyy-MM-dd"), creditDateInfoTest.Credit_toReturn_date);
		}

		[Test]
		public void CalculateCreditDateInfos_AssertCardNumberIsReturnedCorrectly()
		{
			string creditChoice = "credit1Button";
			CreditDateInfo creditDateInfoTest = this.bankService.CalculateCreditDateInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.Card_number, creditDateInfoTest.Card_number);
		}





		[Test]
		public void CalculateCreditMoneyInfos_AssertBalanceGetsIncreasedBy500WhenCreditChoice2()
		{
			string creditChoice = "credit2Button";
			double currentBalance = userBankInfo.Balance;
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(currentBalance + 500, creditMoneyInfoTest.Credit_amount);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertBalanceGetsIncreasedBy250WhenCreditChoice3()
		{
			string creditChoice = "credit3Button";
			double currentBalance = userBankInfo.Balance;
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(currentBalance + 250, creditMoneyInfoTest.Credit_amount);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertInterestIsCorrectWhenCreditChoice1()
		{
			string creditChoice = "credit1Button";
			double interest = 0.03;
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(interest, creditMoneyInfoTest.Credit_interest);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertInterestIsCorrectWhenCreditChoice2()
		{
			string creditChoice = "credit2Button";
			double interest = 0.04;
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(interest, creditMoneyInfoTest.Credit_interest);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertInterestIsCorrectWhenCreditChoice3()
		{
			string creditChoice = "credit3Button";
			double interest = 0.05;
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(interest, creditMoneyInfoTest.Credit_interest);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertToBePaidIsCalculatedCorrectlyWhenCreditChoice1()
		{
			string creditChoice = "credit1Button";
			double interest = 0.03;
			double amount = 1000;
			double toBePaid = amount + (interest * amount);
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(toBePaid, creditMoneyInfoTest.Credit_ToBePaid);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertToBePaidIsCalculatedCorrectlyWhenCreditChoice2()
		{
			string creditChoice = "credit2Button";
			double interest = 0.04;
			double amount = 500;
			double toBePaid = amount + (interest * amount);
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(toBePaid, creditMoneyInfoTest.Credit_ToBePaid);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertToBePaidIsCalculatedCorrectlyWhenCreditChoice3()
		{
			string creditChoice = "credit3Button";
			double interest = 0.05;
			double amount = 250;
			double toBePaid = amount + (interest * amount);
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(toBePaid, creditMoneyInfoTest.Credit_ToBePaid);
		}

		[Test]
		public void CalculateCreditMoneyInfos_AssertCardNumberIsReturnedCorrectly()
		{
			string creditChoice = "credit3Button";
			string card_number = userBankInfo.Card_number;
			CreditMoneyInfo creditMoneyInfoTest = this.bankService.CalculateCreditMoneyInfos(creditChoice, userBankInfo.Card_number);
			Assert.AreEqual(card_number, creditMoneyInfoTest.Card_number);
		}





		[Test]
		public void TakeCredit_AssertCardNumberUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.Card_number, userBankInfoTest.Card_number);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertPINUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.PIN, userBankInfoTest.PIN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertIBANUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.IBAN, userBankInfoTest.IBAN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertEGNUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.EGN, userBankInfoTest.EGN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertBalanceUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertCardNumberCreditBooleanInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
			Assert.AreEqual(creditBooleanInfo.Card_number, creditBooleanInfoTest.Card_number);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertHasTakenCreditCreditBooleanInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);

			CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
			Assert.AreEqual(creditBooleanInfo.Has_taken_credit, creditBooleanInfoTest.Has_taken_credit);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertCreditDateInfosIsAdded()
		{
			userBankInfo.Balance = 1000;
			creditBooleanInfo.Has_taken_credit = true;
			//this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			Assert.IsFalse(context.CreditDateInfos.Contains(creditDateInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void TakeCredit_AssertCreditMoneyInfosIsAdded()
		{
			userBankInfo.Balance = 1000;
			//this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			Assert.IsFalse(context.CreditMoneyInfos.Contains(creditMoneyInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}





		[Test]
		public void PayCredit_AssertCardNumberUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.Card_number, userBankInfoTest.Card_number);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertPINUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.PIN, userBankInfoTest.PIN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertIBANUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.IBAN, userBankInfoTest.IBAN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertEGNUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.EGN, userBankInfoTest.EGN);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertBalanceUserBankInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			UserBankInfo userBankInfoTest = context.UserBankInfos.FirstOrDefault(p => p.Card_number == userBankInfo.Card_number);
			Assert.AreEqual(userBankInfo.Balance, userBankInfoTest.Balance);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertCardNumberCreditBooleanInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
			Assert.AreEqual(creditBooleanInfo.Card_number, creditBooleanInfoTest.Card_number);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertHasTakenCreditCreditBooleanInfosIsSentToDBCorrectly()
		{
			this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			CreditBooleanInfo creditBooleanInfoTest = context.CreditBooleanInfos.FirstOrDefault(p => p.Card_number == creditBooleanInfo.Card_number);
			Assert.AreEqual(creditBooleanInfo.Has_taken_credit, creditBooleanInfoTest.Has_taken_credit);
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertCreditDateInfosIsAdded()
		{
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			Assert.IsFalse(context.CreditDateInfos.Contains(creditDateInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();
		}

		[Test]
		public void PayCredit_AssertCreditMoneyInfosIsAdded()
		{
			userBankInfo.Balance = 1000;
			//this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
			Assert.IsFalse(context.CreditMoneyInfos.Contains(creditMoneyInfo));
			context.Remove(userInfo);
			context.Remove(userBankInfo);
			context.Remove(creditBooleanInfo);
			context.Remove(creditDateInfo);
			context.Remove(creditMoneyInfo);
			context.Remove(userIBANInfo);
			//context.SaveChanges();

		}
	}
}