﻿namespace Hydra.Common.Repository.IService
{
    public interface ICurrentUserService
    {
        public long UserId { get; }
        public string Email { get; }
        public string UserName { get; }

    }
}