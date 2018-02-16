
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
    [Parameter()]
    [string]
    $FacebookAppId,
    # The App Secret for the facebook app to link this with
    [Parameter()]
    [string]
    $FacebookAppSecret,
    # The Client ID for the Google app to link this with
    [Parameter()]
    [string]
    $GoogleClientId,
    # The Client Secret for the Google app to link this with
    [Parameter()]
    [string]
    $GoogleClientSecret,
    # The URL to the webdeploy zip file for the 
    [Parameter()]
    [string]
    $FunctionAppZipUri = $null,
    # The URL to the webdeploy zip file for the 
    [Parameter()]
    [string]
    $WebAppZipUri = $null

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
$parameters = @{
    "functionAppName" = $FunctionAppName
    "webAppName" = $WebAppName
    "facebookAppId" = $FacebookAppId
    "facebookAppSecret" = $FacebookAppSecret
    "googleClientId" = $GoogleClientId
    "googleClientSecret" = $GoogleClientSecret
}
if($FunctionAppZipUri -ne $null){
    $parameters["functionAppZipUri"] = $FunctionAppZipUri
}
if($WebAppZipUri -ne $null){
    $parameters["webAppZipUri"] = $WebAppZipUri
}
New-AzureRmResourceGroupDeployment `
    -ResourceGroupName $ResourceGroupName `
    -Name $deploymentName `
    -TemplateFile "$PSScriptRoot/azuredeploy.json" `
    -TemplateParameterObject $parameters