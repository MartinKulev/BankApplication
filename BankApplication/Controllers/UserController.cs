using Bank_Web_App.Services;
using BankApplication_Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bank_Web_App.Controllers
{
    public class UserController : Controller
    {
        private UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {            
            return View();
        }
        [HttpPost]
        public IActionResult Register(string egn, string first_name, string last_name, string email, string pin)
        {
            UserInfo userInfo = new UserInfo(egn, first_name, last_name, email);
            string card_number = this.userService.CreateRandomCardNumber();
            string iban = this.userService.CreateRandomIBAN();
            UserBankInfo userBankInfo = new UserBankInfo(card_number, pin, userInfo.EGN, iban);
            CreditBooleanInfo creditBooleanInfo = new CreditBooleanInfo(card_number, false);
            UserIBANInfo userIBANInfo = new UserIBANInfo(iban);


			bool registrationSuccess = true;
			bool egnExists = this.userService.DoesEGNExist(userInfo);
			if (egnExists)
			{
				ViewData.Add("EGNExistsError", "User already registered!");
				registrationSuccess = false;
			}			
            if(registrationSuccess)
            {
                this.userService.RegisterUser(userInfo, userBankInfo, creditBooleanInfo, userIBANInfo);
				string toBeSent = card_number + " " + iban;
				
				return RedirectToAction("SuccesfulRegisterPage", toBeSent);
            }
            return View();
        }
		[Route("{toBeSent}/SuccesfulRegisterPage")]
		public IActionResult SuccesfulRegisterPage(string toBeSent)
        {
			List<string> infos = toBeSent.Split(" ").ToList();
			string card_number = infos[0];
			string iban = infos[1];
            ViewData.Add("Infos", $"Your card number is : {card_number}");
			ViewData.Add("Infos2", $"Your iban is : {iban}");
			return View();
        }

		public IActionResult Login()
        {         
            return View();
        }

        [HttpPost]		
		public IActionResult Login(string card_number, string pin)
        {
            UserBankInfo userBankInfo = new UserBankInfo(card_number);
            bool cardNumberExists = this.userService.DoesCardNumberExist(userBankInfo);
            if (cardNumberExists == false)
            {
				ViewData.Add("CardNumberDoesntExistError", "Card Doesn't exist!");
			}
            else
            {
                userBankInfo = this.userService.LogInUserInto1stTable(card_number);

                if (pin != userBankInfo.PIN)
                {
					ViewData.Add("WrongPINError", "Wrong PIN code!");
				}
                else
                {

					return RedirectToAction("BankMenu", card_number);
                }
            }
            return View();
        }

		
        
    }
}
