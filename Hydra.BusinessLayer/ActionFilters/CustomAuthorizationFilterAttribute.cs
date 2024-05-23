using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Hydra.BusinessLayer.ActionFilters
{
    public class CustomAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int _accessLevelPermission;
        private readonly int _roleId;
        private readonly ICurrentUserService _currentUserService;

        public CustomAuthorizationFilterAttribute(int accessLevelPermission, int roleId, ICurrentUserService currentUserService)
        {
            _accessLevelPermission = accessLevelPermission;
            _roleId = roleId;
            _currentUserService = currentUserService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var accessLevelId = GetUserAccessLevelId();
            var roleId = GetUserRoleId();
            if (accessLevelId != _accessLevelPermission && roleId != _roleId)
            {
                var response = new
                {
                    statusCode = 403,
                    message = "You do not have permission to perform this action."
                };
                var jsonResult = JsonSerializer.Serialize(response);
                context.Result = new ContentResult
                {
                    Content = jsonResult,
                    ContentType = "application/json"
                };
                return;
            }
        }

        private int GetUserAccessLevelId()
        {
            return _currentUserService.AccessLevelId;
        }

        private int GetUserRoleId()
        {
            return _currentUserService.RoleId;
        }
    }
}
