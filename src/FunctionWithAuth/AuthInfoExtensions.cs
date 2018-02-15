using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunctionWithAuth
{
    public static class AuthInfoExtensions
    {
        public static AuthUserClaim GetClaim(this AuthInfo authInfo, string claimType)
        {
            return authInfo.UserClaims.FirstOrDefault(c => c.Type == claimType);
        }

        public static async Task<AuthInfo> GetAuthInfoAsync(this HttpRequestMessage request)
        {
            var client = new HttpClient(); // TODO - reuse. Need to send headers as part of request rather than using DefaultRequestHeaders
                                           //client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", GetRequestZumoAuthToken(req));
                                           //var response = await client.GetAsync(GetAuthMeEndpoint());

            string zumoAuthToken = request.GetZumoAuthToken();
            if (string.IsNullOrEmpty(zumoAuthToken))
            {
                return null;
            }
            var authMeRequest = new HttpRequestMessage(HttpMethod.Get, GetEasyAuthEndpoint())
            {
                Headers =
                        {
                            { "X-ZUMO-AUTH", zumoAuthToken }
                        }
            };
            var response = await client.SendAsync(authMeRequest);
            var authInfoArray = await response.Content.ReadAsAsync<AuthInfo[]>();
            return authInfoArray[0]; // The .auth/me content is a single item array
        }
        private static string GetEasyAuthEndpoint()
        {
            var hostname = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");

            string requestUri = $"https://{hostname}/.auth/me";
            return requestUri;
        }
        private static string GetZumoAuthToken(this HttpRequestMessage req)
        {
            return req.Headers.GetValues("X-ZUMO-AUTH").FirstOrDefault();
        }
    }
}
