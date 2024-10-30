using Bank_Web_App.Data.Entities;
using Bank_Web_App.Services;
using BankApplication_Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bank_Web_App.Controllers
{
	public class BankController : Controller
	{
        private BankService bankService;
        private UserService userService;

        public BankController(BankService bankService, UserService userService)
        {
            this.bankService = bankService;
            this.userService = userService;

        }
        
        [Route("{card_number}/BankMenu")]
        public IActionResult BankMenu(string card_number)
        {
            UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
            UserInfo userInfo = this.userService.LogInUserInto2ndTable(userBankInfo.EGN);
            CreditBooleanInfo creditBooleanInfo = this.userService.LogInUserInto3rdTable(card_number);
            CreditDateInfo creditDateInfo = this.userService.LogInUserInto4thTable(card_number);
            CreditMoneyInfo creditMoneyInfo = this.userService.LogInUserInto5thTable(card_number);
            UserIBANInfo userIBANInfo = new UserIBANInfo(userBankInfo.IBAN);
            return View(nameof(BankMenu), userInfo);
        }
        [HttpPost]
        [Route("{card_number}/BankMenu")]
        public IActionResult BankMenu(string card_number, string optionButton)
        {
            if (optionButton == "showBalanceButton")
            {
                return RedirectToAction("Balance", card_number);
            }
            else if(optionButton == "withdrawMoneyButton")
            {
                return RedirectToAction("Withdraw", card_number);
            }
			else if (optionButton == "depositMoneyButton")
			{
				return RedirectToAction("Deposit", card_number);
			}
            else if (optionButton == "transferMoneyButton")
            {
                return RedirectToAction("Transfer", card_number);
            }
            else if (optionButton == "showCreditInfoButton")
            {
                return RedirectToAction("ShowCreditInfo", card_number);
            }
            else if (optionButton == "takeCreditButton")
            {
                return RedirectToAction("TakeCredit", card_number);
            }
            else if (optionButton == "payCreditButton")
            {
                return RedirectToAction("PayCredit", card_number);
            }
            else if (optionButton == "logoutButton")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        [Route("{card_number}/Balance")]
        public IActionResult Balance(string card_number)
        {
            UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
            return View(userBankInfo);
        }

		[HttpPost]
		[Route("{card_number}/Balance")]
		public IActionResult Balance(string optionButton, string card_number)
		{
	        return RedirectToAction("BankMenu", card_number);
		}

		[Route("{card_number}/Withdraw")]
		public IActionResult Withdraw(string card_number)
		{			
            return View();
		}

		[HttpPost]
		[Route("{card_number}/Withdraw")]
		public IActionResult Withdraw(string optionButton, string card_number, double withdrawAmount)
		{
			if (optionButton == "goBackButton")
			{
				return RedirectToAction("BankMenu", card_number);
			}
            else
            {
				UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
                if(withdrawAmount <= 0)
                {
					ViewData.Add("CantWithdrawUnderZeroError", "You can't withdraw 0$ or less than that!");
				}
				else if (withdrawAmount > userBankInfo.Balance)
				{
					ViewData.Add("CantWithdrawMoreThanYouHaveError", "You can't withrdaw more money than you have!");
				}
				else
				{
					userBankInfo.Balance -= withdrawAmount;
					this.bankService.WithdrawDeposit(userBankInfo);
					ViewData.Add("SuccesfulWithdrawalMessage", $"You Succesfuly Withdrew {withdrawAmount}$");
					return View(userBankInfo);
				}
			}
            return View();
		}
		[Route("{card_number}/Deposit")]
		public IActionResult Deposit(string card_number)
		{
			return View();
		}

		[HttpPost]
		[Route("{card_number}/Deposit")]
		public IActionResult Deposit(string optionButton, string card_number, double depositAmount)
		{
			if (optionButton == "goBackButton")
			{
				return RedirectToAction("BankMenu", card_number);
			}
			else
			{
				if (depositAmount <= 0)
				{
					ViewData.Add("CantDepositZeroOrLessError", $"You can't deposit 0$ or less than that!");
				}
				else
				{
					UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
					userBankInfo.Balance += depositAmount;
					this.bankService.WithdrawDeposit(userBankInfo);
					ViewData.Add("SuccesfulDepositMessage", $"You Succesfuly Deposited {depositAmount}$");
					return View(userBankInfo);
				}
			}
			return View();
		}

        [Route("{card_number}/Transfer")]
        public IActionResult Transfer(string card_number)
        {
            return View();
        }

        [HttpPost]
        [Route("{card_number}/Transfer")]
        public IActionResult Transfer(string optionButton, string card_number, string iban, double transferAmount)
        {
            if (optionButton == "goBackButton")
            {
                return RedirectToAction("BankMenu", card_number);
			}
			else
            {
				UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
				UserIBANInfo userIBANInfo = new UserIBANInfo(iban);
				bool ibanExists = this.bankService.DoesIBANExist(userIBANInfo);
				if (ibanExists == false)
				{
					ViewData.Add("IBANDoesntExistMessage", $"IBAN doesn't exist!");
				}
				else
				{
					if(userBankInfo.IBAN == iban)
					{
						ViewData.Add("CantTrasnferToYourselfError", $"You can't transfer money to yourself!");
					}
					else if(transferAmount <= 0)
					{
						ViewData.Add("CantTrasnferZeroOrLessError", $"You can't withdraw 0$ or less than that!");
					}
					else if (transferAmount > userBankInfo.Balance)
					{
						ViewData.Add("CantTrasnferMoreThanYouHaveError", $"You can't trasnfer more money than you have!");
					}
					else
					{
						userBankInfo.Balance = this.bankService.Transfer(userBankInfo, iban, transferAmount);
						ViewData.Add("SuccesfulTrasnferMessage", $"You Succesfuly Trasnfered {transferAmount}$ to {iban}");
						return View(userBankInfo);
					}
				}
				return View();
			}
        }
		[Route("{card_number}/ShowCreditInfo")]
		public IActionResult ShowCreditInfo(string card_number)
		{
			CreditBooleanInfo creditBooleanInfo = this.userService.LogInUserInto3rdTable(card_number);
			if (creditBooleanInfo.Has_taken_credit)
			{
				CreditDateInfo creditDateInfo = this.userService.LogInUserInto4thTable(card_number);
				CreditMoneyInfo creditMoneyInfo = this.userService.LogInUserInto5thTable(card_number);

				Credit credit = new Credit(creditDateInfo.Credit_taken_date, creditDateInfo.Credit_toReturn_date,
					creditMoneyInfo.Credit_amount, creditMoneyInfo.Credit_interest, creditMoneyInfo.Credit_ToBePaid);
				return View(credit);
			}
			else
			{
				ViewData.Add("YouHaveNoExistingCreditsMessage", "You have no existing credits!");
			}
			return View();
		}

		[HttpPost]
		[Route("{card_number}/ShowCreditInfo")]
		public IActionResult ShowCreditInfo(string optionButton, string card_number)
		{
			if (optionButton == "goBackButton")
			{
				return RedirectToAction("BankMenu", card_number);
			}
			return View();
		}

		[Route("{card_number}/TakeCredit")]
        public IActionResult TakeCredit(string card_number)
        {
            return View();
        }

        [HttpPost]
        [Route("{card_number}/TakeCredit")]
        public IActionResult TakeCredit(string optionButton, string card_number)
        {
            if (optionButton == "goBackButton")
            {
                return RedirectToAction("BankMenu", card_number);
            }

            UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
			CreditBooleanInfo creditBooleanInfo = this.userService.LogInUserInto3rdTable(card_number);
			if(creditBooleanInfo.Has_taken_credit)
			{
                ViewData.Add("AlreadyTakenCreditError", "You can only take one credit at a time!");
            }  
			else
			{
                creditBooleanInfo.Has_taken_credit = true;
                CreditDateInfo creditDateInfo = this.bankService.CalculateCreditDateInfos(optionButton, userBankInfo.Card_number);
                CreditMoneyInfo creditMoneyInfo = this.bankService.CalculateCreditMoneyInfos(optionButton, userBankInfo.Card_number);
                userBankInfo.Balance += creditMoneyInfo.Credit_amount;
                this.bankService.TakeCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);    
                return RedirectToAction("SuccesfulTakenCredit", card_number);
            }
            return View();
        }

		[Route("{card_number}/SuccesfulTakenCredit")]
        public IActionResult SuccesfulTakenCredit(string card_number)
        {
			UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
			ViewData.Add("CreditSuccessfulyTakenMessage", $"Your new balance: {userBankInfo.Balance}$");
			return View();
        }

        [HttpPost]
        [Route("{card_number}/SuccesfulTakenCredit")]
        public IActionResult SuccesfulTakenCredit(string optionButton, string card_number)
        {
            if (optionButton == "goBackButton")
            {
                return RedirectToAction("BankMenu", card_number);
            }
            return View();
        }

		[Route("{card_number}/PayCredit")]
		public IActionResult PayCredit(string card_number)
		{
			CreditBooleanInfo creditBooleanInfo = this.userService.LogInUserInto3rdTable(card_number);
			if (creditBooleanInfo.Has_taken_credit)
			{
				UserBankInfo userBankInfo = this.userService.LogInUserInto1stTable(card_number);
				CreditDateInfo creditDateInfo = this.userService.LogInUserInto4thTable(card_number);
				CreditMoneyInfo creditMoneyInfo = this.userService.LogInUserInto5thTable(card_number);

				if (userBankInfo.Balance < creditMoneyInfo.Credit_ToBePaid)
				{
					ViewData.Add("NotEnoughMoneyToPayCreditError", "Not enough balance to pay your credit!");
				}
				else
				{
					creditBooleanInfo.Has_taken_credit = false;
					userBankInfo.Balance -= creditMoneyInfo.Credit_ToBePaid;
					this.bankService.PayCredit(userBankInfo, creditBooleanInfo, creditDateInfo, creditMoneyInfo);
					ViewData.Add("CreditSuccessfulyPaidMessage", "Credit Successfuly paid!");
					ViewData.Add("CreditSuccessfulyPaidMessage2", $"Your new balance: {userBankInfo.Balance}$");
				}
			}
			else
			{
				ViewData.Add("YouHaveNoExistingCreditsError", $"You have no existing credits!");
			}
			return View();
		}

		[HttpPost]
		[Route("{card_number}/PayCredit")]
		public IActionResult PayCredit(string optionButton, string card_number)
		{
			if (optionButton == "goBackButton")
			{
				return RedirectToAction("BankMenu", card_number);
			}
			return View();
		}

	}
}
