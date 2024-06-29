namespace Hydra.Common.Globle.Enum
{
    public enum Roles
    {
        Admin = 1,
        UniversityAdmin = 2,
        Staff = 3,
        Learner = 4,
    }

    public enum FieldType
    {
        LearningOutcomes = 1,
        Competencies = 2,
    }

    public enum GetLearnerType
    {
        All = 0,
        Assigned = 1,
        UnAssigned = 2,
    }

    public enum AccessLevelType
    {
        ViewOnly = 1,
        ViewAndEdit = 2,
        ViewEditAndDelete = 3,
    }

    public enum BadgeSortBy
    {
        All = 0,
        Badge = 1,
        Certificate = 2,
        License = 3,
        Miscellaneous = 4,
    }

    public enum StaffSortBy
    {
        All = 0,
        Email = 1,
        UserName = 2,
        AccessLevel = 3,
    }

    public enum DeletedUserOptions
    {
        All = 0,
        Learner = 1,
        Staff = 2
    }

    public enum StaffSortType
    {
        All = 0,
        Active = 1,
        Archived = 2,
        
    }
}
