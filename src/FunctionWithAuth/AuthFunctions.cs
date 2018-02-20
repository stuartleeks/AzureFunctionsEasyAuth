using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace FunctionWithAuth
{
    public static class AuthFunctions
    {
        [FunctionName("IsAuthenticated")]
        public static HttpResponseMessage IsAuthenticated(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage request,
            TraceWriter log)
        {
            // simple test to check whether we have an authenticated user
            bool authenticated = Thread.CurrentPrincipal.Identity.IsAuthenticated;

            return request.CreateResponse(HttpStatusCode.OK, authenticated, "application/json");
        }

        [FunctionName("GetClaims")]
        public static HttpResponseMessage GetClaims(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage request,
            TraceWriter log)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var claimsPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
                // Could use the claims here. For this sample, just return it!
                return request.CreateResponse(HttpStatusCode.OK, claims, "application/json");
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not Authorized");
            }
        }
        [FunctionName("GetAuthInfo")]
        public static async Task<HttpResponseMessage> GetAuthInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage request,
            TraceWriter log)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var authInfo = await request.GetAuthInfoAsync();
                // Could use the authInfo here. For this sample, just return it!
                return request.CreateResponse(HttpStatusCode.OK, authInfo, "application/json");
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not Authorized");
            }
        }

        [FunctionName("GetEmailClaim")]
        public static async Task<HttpResponseMessage> GetEmailClaim(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage request,
            TraceWriter log)
        {
            var id = Thread.CurrentPrincipal.Identity;
            bool auth = id.IsAuthenticated;
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var authInfo = await request.GetAuthInfoAsync();
                // look up specific claim type, in this case the email claim (http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress)
                var emailClaim = authInfo?.GetClaim(ClaimTypes.Email);
                return request.CreateResponse(HttpStatusCode.OK, emailClaim?.Value, "application/json");
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not Authorized");
            }
        }

        [FunctionName("GetAuthInfo_Binding")]
        public static async Task<HttpResponseMessage> GetAuthInfo_Binding(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage request,
            [BindAuthInfo]
            AuthInfo authInfo,
            TraceWriter log)
        {
            if (authInfo != null)
            {
                // Could use the authInfo here. For this sample, just return it!
                return request.CreateResponse(HttpStatusCode.OK, authInfo, "application/json");
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not Authorized");
            }
        }
    }
}
