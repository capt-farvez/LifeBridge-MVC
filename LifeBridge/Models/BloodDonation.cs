namespace LifeBridge.Models
{
    public class RequestBloodDonation
    {
        public Guid Id { get; set; }
        
        // Foreign key to User
        public Guid UserId { get; set; }
        public User? User { get; set; }

        // Additional properties
        public string Bloodgroup {get; set;} = string.Empty;
        public string Perpose {get; set;} = string.Empty;
        public string Location {get; set;} = string.Empty;
        public string Phone {get; set;} = string.Empty;
        public DateTime RequestDate { get; set; }
    }

    public class BloodDonationRecord
    {
        public Guid Id { get; set; }

        // Foreign key to User
        public Guid UserId { get; set; }
        public User? User { get; set; }

        // Additional properties
        public DateTime DonationDate { get; set; }
        public int AmountDonatedMl { get; set; }
        public string Location { get; set; } = string.Empty;
    }

}
