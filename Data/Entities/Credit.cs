namespace Bank_Web_App.Data.Entities
{
	public class Credit
	{
		public Credit(string credit_taken_date, string credit_toReturn_date, double credit_amount, double credit_interest, double credit_ToBePaid)
		{
			Credit_taken_date = credit_taken_date;
			Credit_toReturn_date = credit_toReturn_date;
			Credit_amount = credit_amount;
			Credit_interest = credit_interest;
			Credit_ToBePaid = credit_ToBePaid;
		}
		public string Credit_taken_date { get; set; }

		public string Credit_toReturn_date { get; set; }
		public double Credit_amount { get; set; }

		public double Credit_interest { get; set; }
		public double Credit_ToBePaid { get; set; }

	}
}
