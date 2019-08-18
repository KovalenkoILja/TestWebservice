namespace TestWebservice.Models
{
    public class PersonCommunication : Person 
    {
        public string Phone { get; set; }
        
        public bool IsUsed { get; set; }
        
        public int PhoneTypeId { get; set; }
        
        public int PersonId { get; set; }
    }
}