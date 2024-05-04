namespace Hydra.Common.Globle
{
    public class ResponseConstants
    {
        #region common Responses

        public const string Success = "Success";
        public const string InvalidCredential = "Invalid credentials";
        public const string Exists = "User with these credentials exists";
        public const string NotExists = "User with these credentials does not exists";
        public const string NotFound = "No content";
        public const string BadRequest = "Bad request";

        #endregion

        #region Document Responses

        public const string Mediapath = "hydra/media";

        #endregion

        #region Account Service Responses

        public const string UserNameExists = "UserName already exists";
        public const string InvalidUserName = "Invalid username";
        public const string InvalidPassword = "Invalid password";
        public const string Password = "Password has been changed successfully";
        public const string PasswordResetOtpSent = "Password reset OTP sent to email";
        public const string InvalidOtp = "Invalid OTP";
        public const string OtpExpired = "OTP expired";

        #endregion

        #region Department Service Responses 

        public const string DepartmentExists = "Department already exits.";
        public const string DepartmentAdded = "Department added successfully.";
        public const string DepartmentUpdated = "Department updated successfully.";
        public const string DepartmentDeleted = "Department removed successfully.";
        public const string InvalidDepartmentId = "Invalid department id";

        #endregion

        #region Badge Sequence Service Responses

        public const string BadgeSequenceExists = "Badge sequence already exists.";
        public const string BadgeSequenceAdded = "Badge sequence added successfully.";
        public const string BadgeSequenceUpdated = "Badge sequence updated successfully.";
        public const string BadgeSequenceDeleted = "Badge sequence removed successfully.";
        public const string InvalidBadgeSequenceId = "Invalid badge sequence id.";

        #endregion

        #region Staff Service Responses

        public const string StaffAdded = "Staff member added successfully.";
        public const string StaffUpdated = "Staff member updated successfully.";
        public const string StaffDeleted = "Staff member removed successfully.";
        public const string InvalidUserId = "Invalid user id";

        #endregion

        #region Badge Service Responses 

        public const string BadgeExists = "Badge already exists.";
        public const string ApprovedBadgeAdded = "{BadgeName} badge is now live and ready to issue. Issue this badge to learner now.";
        public const string RequiresApprovalBadgeAdded = "{BadgeName} badge will be live once other admin approve it.";
        public const string BadgeUpdated = "Badge updated successfully.";
        public const string BadgeDeleted = "Badge removed successfully.";
        public const string InvalidBadgeId = "Invalid badge id";

        #endregion

        #region LearnerResponseContants
        public const string LearnerExists = "Learner with these credentials already exists";
        public const string LearnersExists = "Learners with these credentials already exists";
        public const string LearnerAdded = "Learner has been created";
        public const string LearnersAdded = "{count} Learners added successfully";
        #endregion
    }
}
