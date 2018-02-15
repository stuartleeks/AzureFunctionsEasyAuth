# AzureFunctionsEasyAuth
A repo with sample code for using Azure Functions with App Service Authentication ("EasyAuth")

EasyAuth provides integration with a [number of authentication providers](https://docs.microsoft.com/en-us/azure/app-service/app-service-authentication-overview#documentation-and-additional-resources), but for simplicity this sample uses facebook integration.

## Projects

There are two projects in the solution:

 * src/FunctionWithAuth - this is the Azure Functions implementation
 * src/SampleWebClient - this is a sample client to make it easy to test the Azure Functions in an interactive way

## Deployment

If you want a quick way to see the sample in action without deploying it yourself then visit https://easyauthweb.azurewebsites.net/.

To deploy the sample

1. Create a Facebook application at https://developers.facebook.com and make a note of the App ID and App Secret
2. Deploy the [template](deploy/azuredeploy.json) (you can use the big deploy button below). When it has finished deploying, make note of the web app url
3. Configure the Facebook application:
    * set the AppDomains to the domain for the web app
    * add a website and set the Site Url to the URL of the web app


[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fstuartleeks%2FAzureFunctionsEasyAuth%2Fmaster%2Fdeploy%2Fazuredeploy.json)