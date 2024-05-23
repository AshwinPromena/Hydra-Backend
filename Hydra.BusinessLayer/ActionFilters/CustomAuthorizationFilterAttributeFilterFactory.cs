using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.BusinessLayer.ActionFilters
{
    public class CustomAuthorizationFilterAttributeFilterFactory : Attribute, IFilterFactory
    {
        private readonly int _accessLevelPermission;

        public CustomAuthorizationFilterAttributeFilterFactory(int accessLevelPermission)
        {
            _accessLevelPermission = accessLevelPermission;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var currentUserService = serviceProvider.GetService<ICurrentUserService>();
            return new CustomAuthorizationFilterAttribute(_accessLevelPermission, currentUserService);
        }

        public bool IsReusable => false;
    }
}
