using System;

namespace TestWebservice.Models
{
    public class PersonOperation : Person
    {
        public string Phone { get; set; }
        
        public DateTime Date { get; set; }
        
        public int PersonId { get; set; }
        
        public long Account { get; set; }
        
        public string OperationType { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal AmountEUR { get; set; }

        public void ConvertRubToEur(string currencyCode, decimal currencyRate)
        {
            if (currencyCode == null) 
                throw new ArgumentNullException(nameof(currencyCode));
            if(!currencyCode.Equals("RUB"))
                throw new Exception("Unknown currency!");

            AmountEUR = Amount * currencyRate;
        }
    }
}