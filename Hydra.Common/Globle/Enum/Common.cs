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
        IssuedDate = 1,
        ExpirationDate = 2,
    }

    public enum StaffSortBy
    {
        All = 0,
        Email = 1,
        Name = 2,
        AccessLevel = 3,
    }
}
