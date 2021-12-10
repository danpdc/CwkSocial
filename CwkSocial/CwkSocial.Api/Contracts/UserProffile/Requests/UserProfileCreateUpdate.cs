namespace CwkSocial.Api.Contracts.UserProffile.Requests
{
    public record UserProfileCreateUpdate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}
