
param(
    # Name of the resource group to use/create
    [Parameter(Mandatory=$true)]
    [string]
    $ResourceGroupName,
    # Location to create resource group in if it doesn't exist
    [Parameter(Mandatory=$true)]
    [string]
    $Location,
    # Name to use for the Function app
    [Parameter(Mandatory=$true)]
    [string]
    $FunctionAppName,
    # Name to use for the Web app
    [Parameter(Mandatory=$true)]
    [string]
    $WebAppName,
    # The App ID for the facebook app to link this with
    [Parameter(Mandatory=$true)]
    [string]
    $FacebookAppId,
    # The App Secret for the facebook app to link this with
    [Parameter(Mandatory=$true)]
    [string]
    $FacebookAppSecret,
    # The URL to the webdeploy zip file for the 
    [Parameter()]
    [string]
    $FunctionAppZipUri = "https://ci.appveyor.com/api/projects/stuartleeks/azurefunctionseasyauth/artifacts/src%2FFunctionWithAuth%2Foutput%2FFunctionWithAuth.zip", # TODO move default to the template
    # The URL to the webdeploy zip file for the 
    [Parameter()]
    [string]
    $WebAppZipUri = "https://ci.appveyor.com/api/projects/stuartleeks/azurefunctionseasyauth/artifacts/src%2FSimpleWebClient%2Foutput%2FSimpleWebClient.zip" # TODO move default to the template

)

$resourceGroup = Get-AzureRmResourceGroup -Name $ResourceGroupName -ErrorAction SilentlyContinue
if ($resourceGroup -eq $null){
    Write-Host "Creating resource group $ResourceGroupName in $Location"
    $resourceGroup = New-AzureRmResourceGroup -Name $ResourceGroupName -Location $Location
} else {
    Write-Host "Resource group $ResourceGroupName already exists"
}

$deploymentName = "Deployment-$(Get-Date -f "yyyyMMdd_HHmmss")" # Name the deployments for ease of referring back/debugging
Write-Host "Creating deployment '$deploymentName'"
New-AzureRmResourceGroupDeployment `
    -ResourceGroupName $ResourceGroupName `
    -Name $deploymentName `
    -TemplateFile "$PSScriptRoot/azuredeploy.json" `
    -TemplateParameterObject @{
        "facebookAppId" = $FacebookAppId
        "facebookAppSecret" = $FacebookAppSecret
        "functionAppName" = $FunctionAppName
        "functionAppZipUri" = $FunctionAppZipUri
        "webAppName" = $WebAppName
        "webAppZipUri" = $WebAppZipUri
    }