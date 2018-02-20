using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;

namespace FunctionWithAuth
{
    public class AuthInfoExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var converter = new AuthInfoConverter();
            var bindingRule = context.AddBindingRule<BindAuthInfoAttribute>();
            bindingRule.AddConverter((IAsyncConverter<BindAuthInfoAttribute, AuthInfo>)converter);

            // If the BindToInput call below is omitted then I get the following errors:
            // The following 1 functions are in error:
            // GetAuthInfo_Binding: Microsoft.Azure.WebJobs.Host: Error indexing method 'AuthFunctions.GetAuthInfo_Binding'. Microsoft.Azure.WebJobs.Host: Cannot bind parameter 'authInfo' to type AuthInfo.Make sure the parameter Type is supported by the binding.If you're using binding extensions (e.g. ServiceBus, Timers, etc.) make sure you've called the registration method for the extension(s) in your startup code (e.g.config.UseServiceBus(), config.UseTimers(), etc.).
            bindingRule.BindToInput<AuthInfo>(converter);

            // The converter never seems to get called in this context:
            context.AddConverter<HttpRequestMessage, AuthInfo>(new AuthInfoConverter());
        }
    }

    [Binding]
    public class BindAuthInfoAttribute : Attribute
    {
    }
}
