using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunctionWithAuth
{
    public static class AuthInfoExtensions
    {
        private static HttpClient _httpClient = new HttpClient();

        public static AuthUserClaim GetClaim(this AuthInfo authInfo, string claimType)
        {
            return authInfo.UserClaims.FirstOrDefault(c => c.Type == claimType);
        }

        public static async Task<AuthInfo> GetAuthInfoAsync(this HttpRequestMessage request)
        {
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
            var response = await _httpClient.SendAsync(authMeRequest);
            var authInfoArray = await response.Content.ReadAsAsync<AuthInfo[]>();
            return authInfoArray.Length > 1 ? authInfoArray[0] : null; // The .auth/me content is a single item array if it is populated
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
