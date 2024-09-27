using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Hydra.Common.Repository.Service
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public long UserId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public string Email => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

        public string UserName => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

        public string Name => _httpContextAccessor.HttpContext.User.FindFirst("name").Value;

        public int RoleId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("roleId").Value);

        public int AccessLevelId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("accessLevelId").Value);

        public int UniversityId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("universityId").Value);
    }
}
