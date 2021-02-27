using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using splendor.net5.core.commons;
using splendor.net5.mvc.contracts;

namespace splendor.net5.mvc.implementers
{
    public class DefaultMVCTracer : IMVCTracer
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IActionContextAccessor _actionContextAccessor;

        public DefaultMVCTracer(IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
        }
        public DTrace Trace(ClaimsPrincipal claims)
        {
            return new DTrace
            {
                UserName = claims.Identity.Name,
                IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                ActionName = _actionContextAccessor.ActionContext?.ActionDescriptor.RouteValues["action"],
                ControllerName = _actionContextAccessor.ActionContext?.ActionDescriptor.RouteValues["controller"]
            };
        }
    }
}