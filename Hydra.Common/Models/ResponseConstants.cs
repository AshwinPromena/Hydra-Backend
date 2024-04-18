namespace Hydra.Common.Models
{
    public class ResponseConstants
    {
        #region common
        public const string Success = "Success";
        public const string InvalidCredential = "Invalid credentials";
        public const string Exists = "User with these credentials exists";
        public const string NotExists = "User with these credentials does not exists";
        public const string NotFound = "No content";
        public const string BadRequest = "Bad request";
        public const string Password = "Password has been changed Successfully";
        #endregion

        #region LearnerResponseContants
        public const string LearnerExists = "Learners with credentials already exists";
        public const string LearnerAdded = "Learner added successfully";
        public const string LearnersAdded = "Learners added successfully";
        #endregion
    }
}
