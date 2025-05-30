using System.ComponentModel.DataAnnotations;

namespace LifeBridge.Models
{
    public enum BloodGroup
    {
        [Display(Name = "A+")] A_Positive,
        [Display(Name = "A-")] A_Negative,
        [Display(Name = "B+")] B_Positive,
        [Display(Name = "B-")] B_Negative,
        [Display(Name = "AB+")] AB_Positive,
        [Display(Name = "AB-")] AB_Negative,
        [Display(Name = "O+")] O_Positive,
        [Display(Name = "O-")] O_Negative
    }

    public enum Role
    {
        Admin,
        User
    }

    public class User
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Role Role { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public string? PublicPhoneNumber { get; set; }
        public bool AvailableToDonateBlood { get; set; } = false;

        public ICollection<RequestBloodDonation> BloodDonationRequests { get; set; } = new List<RequestBloodDonation>();
        public ICollection<BloodDonationRecord> BloodDonationRecords { get; set; } = new List<BloodDonationRecord>();
        public ICollection<RequestOrganDonation> OrganDonationRequests { get; set; } = new List<RequestOrganDonation>();
        public ICollection<OrganDonationRecord> OrganDonationRecords { get; set; } = new List<OrganDonationRecord>();
    }
}