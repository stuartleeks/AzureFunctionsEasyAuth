
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionWithAuthv2
{
    public static class AuthFunctions
    {
        [FunctionName("IsAuthenticated")]
        public static IActionResult IsAuthenticated(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest request,
            TraceWriter log)
        {
            // simple test to check whether we have an authenticated user
            var userInfo = request.GetUserInfo();
            bool authenticated = userInfo != null;

            return new OkObjectResult(authenticated);
        }

        [FunctionName("GetClaims")]
        public static ActionResult GetClaims(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest request,
            TraceWriter log)
        {
            var userInfo = request.GetUserInfo();
            if (userInfo == null)
            {
                return new UnauthorizedResult();
            }
            return new OkObjectResult(userInfo);
        }
        [FunctionName("GetAuthInfo")]
        public static async Task<ActionResult> GetAuthInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest request,
            TraceWriter log)
        {
            var authInfo = await request.GetAuthInfoAsync();
            if (authInfo != null)
            {
                // Could use the authInfo here. For this sample, just return it!
                return new OkObjectResult(authInfo);
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [FunctionName("GetEmailClaim")]
        public static async Task<ActionResult> GetEmailClaim(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest request,
            TraceWriter log)
        {
            var userInfo = request.GetUserInfo();
            if (userInfo == null)
            {
                return new UnauthorizedResult();
            }
                var authInfo = await request.GetAuthInfoAsync();
                // look up specific claim type, in this case the email claim (http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress)
                var emailClaim = authInfo?.GetClaim(ClaimTypes.Email);
                return new OkObjectResult(emailClaim?.Value);
        }
    }

    public class UserInfo
    {
        public string UserId { get; }
        public string Provider { get; set; }
        public UserInfo(string userId, string provider)
        {
            UserId = userId;
            Provider = provider;
        }
    }
    public static class UserInfoExtensions
    {
        public static UserInfo GetUserInfo(this HttpRequest request)
        {
            if (request.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-ID", out var idValues))
            {
                var id = idValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(id))
                {
                    return new UserInfo(id,
                        request.Headers["X-MS-CLIENT-PRINCIPAL-IDP"].FirstOrDefault());
                }
            }
            return null;
        }
    }
}
