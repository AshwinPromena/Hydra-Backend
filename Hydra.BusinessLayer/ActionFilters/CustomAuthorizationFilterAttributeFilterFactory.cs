using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.BusinessLayer.ActionFilters
{
    public class CustomAuthorizationFilterAttributeFilterFactory : Attribute, IFilterFactory
    {
        private readonly int _accessLevelPermission;
        private readonly int [] _roleId;

        public CustomAuthorizationFilterAttributeFilterFactory(int accessLevelPermission, int [] roleId)
        {
            _accessLevelPermission = accessLevelPermission;
            _roleId = roleId;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var currentUserService = serviceProvider.GetService<ICurrentUserService>();
            return new CustomAuthorizationFilterAttribute(_accessLevelPermission, _roleId, currentUserService);
        }

        public bool IsReusable => false;
    }
}
