using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hydra.BusinessLayer.ActionFilters
{
    public class CustomAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int _accessLevelPermission;
        private readonly ICurrentUserService _currentUserService;

        public CustomAuthorizationFilterAttribute(int accessLevelPermission, ICurrentUserService currentUserService)
        {
            _accessLevelPermission = accessLevelPermission;
            _currentUserService = currentUserService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (GetUserAccessLevelId() != _accessLevelPermission)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "You do not have permission to perform this action.",
                    ContentType = "application/json",
                };
                return;
            }
        }

        private int GetUserAccessLevelId()
        {
            var accessLevelId = _currentUserService.AccessLevelId;
            return accessLevelId;
        }
    }
}
