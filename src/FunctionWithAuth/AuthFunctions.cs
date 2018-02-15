using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionWithAuth
{
    public static class AuthFunctions
    {
        [FunctionName("IsAuthenticated")]
        public static HttpResponseMessage IsAuthenticated(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage req, 
            TraceWriter log)
        {
            // simple test to check whether we have an authenticated user
            bool authenticated = Thread.CurrentPrincipal.Identity.IsAuthenticated;

            return req.CreateResponse(HttpStatusCode.OK, authenticated, "application/json");
        }
    }
}
