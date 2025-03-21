﻿namespace Hydra.DatbaseLayer.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IUserRoleRepository UserRoleRepository { get; }

        IRoleRepository RoleRepository { get; }

        IDepartmentRepository DepartmentRepository { get; }

        ILearnerRepository LearnerRepository { get; }

        IAccessLevelRepository AccessLevelRepository { get; }

        IBadgeSequenceRepository BadgeSequenceRepository { get; }

        IBadgeRepository BadgeRepository { get; }

        IBadgeFieldRepository BadgeFieldRepository { get; }

        ILearnerBadgeRepository LearnerBadgeRepository { get; }

        IVerificationRepository VerificationRepository { get; }

        IPasswordResetTokenRepository PasswordResetTokenRepository { get; }

        IDeletedUserRepository DeletedUserRepository { get; }

        IBadgeTypeRepository BadgeTypeRepository { get; }

        IContactSupportFormRepository ContactSupportFormRepository { get; }
    }
}
