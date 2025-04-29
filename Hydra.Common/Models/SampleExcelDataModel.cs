namespace Hydra.Common.Models
{
    //public class SampleExcelDataModel
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Email { get; set; }
    //    public string Email2 { get; set; }
    //    public string Email3 { get; set; }
    //    public string MobileNumber { get; set; }
    //    public string LearnerId { get; set; }
    //}

    public class SampleExcelDataModel
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentEmail { get; set; }
        public string DegreeName { get; set; }
        public string CredentialType { get; set; }
        public string RegistrationMonth { get; set; }
        public string RegistrationYear { get; set; }
        public string StartMonth { get; set; }
        public string StartYear { get; set; }
        public string ConferredMonth { get; set; }
        public string ConferredYear { get; set; }
        public string StudentName { get; set; }
        public string PrincipalSign { get; set; }
        public string StudentUSN { get; set; }
        public string FullName { get; set; }

        // Existing fields (optional)
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string MobileNumber { get; set; }
        public string LearnerId { get; set; }
    }

}
