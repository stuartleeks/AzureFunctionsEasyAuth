# AzureFunctionsEasyAuth

Contents

* [Intro](#intro)
* [Projects](#projects)
* [Outline](#outline)
* [Deployment](#deployment)

## Intro
A repo with sample code for using Azure Functions with App Service Authentication ("EasyAuth")

EasyAuth provides integration with a [number of authentication providers](https://docs.microsoft.com/en-us/azure/app-service/app-service-authentication-overview#documentation-and-additional-resources), but for simplicity this sample uses facebook integration.



## Projects

There are two projects in the solution:

 * src/FunctionWithAuth - this is the Azure Functions implementation
 * src/SampleWebClient - this is a sample client to make it easy to test the Azure Functions in an interactive way

## Outline
The rough outline for the sample is shown in the diagram below.

The browser loads the HTML/JS/CSS for the Web App. The Web App uses the Facebook/Google JavaScript SDKs to authenticate with the identity provider. Once it has the token from the provider, it passes that to the EasyAuth endpoint on the Function App which translates it into an EasyAuth token. This EasyAuth token is then passed on calls to the Azure Function endpoints to call them as the authenticated user.

```
+--------------------+            +--------------------+
|                    |            |                    |
|                    |            |                    |
|                    +------------>  Azure             |
|  Browser           |            |  Web App           |
|                    |            |                    |
|                    |            |                    |
+--------------------+            +--------------------+
    |             |
    |             |
    |             |
    |             |               +--------------------+
    |             |               |                    |
    |             |               |                    |
    |             +--------------->  Azure             |
    |                             |  Function          |
    |                             |                    |
    |                             |                    |
    |                             +--------------------+
    |
+---v----------------+
|                    |
|                    |
|  Identity provider |
|  e.g. Facebook,    |
|  Google            |
|                    |
+--------------------+

```

## Deployment

If you want a quick way to see the sample in action without deploying it yourself then visit https://easyauthweb.azurewebsites.net/.

To deploy the sample with Facebook and/or Google integration:

1. Create applications with one or both of the the identity providers

   1. Create a Facebook application at https://developers.facebook.com and make a note of the App ID and App Secret
   2. Create a Google application at https://developers.google.com/identity/sign-in/web/sign-in and make a note of the Client ID and Client Secret

2. Deploy the [template](deploy/azuredeploy.json) (you can use the big deploy button below).

    You can enter the ID + Secret for Facebook, Google, or both, dependending on what you configured above

    When the template has finished deploying, make note of the web app url.

3. Update the configuration with the identity providers
   1. Configure the Facebook application:
      * set the AppDomains to the domain for the web app
      * add a website and set the Site Url to the URL of the web app
   2. Configure the Google application to set the Authorised JavaScript Origin to the URL of the web app

4. Go to the URL of the web app and test the application!

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fstuartleeks%2FAzureFunctionsEasyAuth%2Fmaster%2Fdeploy%2Fazuredeploy.json)