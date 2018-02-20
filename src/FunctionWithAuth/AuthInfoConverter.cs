using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace FunctionWithAuth
{
    public class AuthInfoConverter : 
        IAsyncConverter<BindAuthInfoAttribute, AuthInfo>,
        IAsyncConverter<HttpRequestMessage, AuthInfo>
    {
        async Task<AuthInfo> IAsyncConverter<BindAuthInfoAttribute, AuthInfo>.ConvertAsync(BindAuthInfoAttribute attribute, CancellationToken cancellationToken)
        {
            // How can I get more context here? (I.e. get the incoming HttpRequestMessage??)
            return null;
        }

        async Task<AuthInfo> IAsyncConverter<HttpRequestMessage, AuthInfo>.ConvertAsync(HttpRequestMessage input, CancellationToken cancellationToken)
        {
            return await input.GetAuthInfoAsync();
        }
    }
}
