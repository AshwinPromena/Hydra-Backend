﻿namespace Hydra.Common.Repository.IService
{
    public interface ICurrentUserService
    {
        public long UserId { get; }
        public string Email { get; }
        public string UserName { get; }
        public string Name { get; }
        public int RoleId { get; }
        public int AccessLevelId { get; }
        public int UniversityId { get; }
    }
}
