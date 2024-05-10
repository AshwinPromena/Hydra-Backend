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
}
