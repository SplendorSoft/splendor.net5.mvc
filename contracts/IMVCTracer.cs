using System.Security.Claims;
using splendor.net5.core.commons;

namespace splendor.net5.mvc.contracts
{
    public interface IMVCTracer
    {

        DTrace Trace(ClaimsPrincipal claims);
    }
}