
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
    $FunctionAppZipUri = "https://ci.appveyor.com/api/buildjobs/esy5k2mefus59kqd/artifacts/src%2FFunctionWithAuth%2Foutput%2FFunctionWithAuth.zip" # TODO move default to the template

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
        "functionAppName" = $FunctionAppName
        "facebookAppId" = $FacebookAppId
        "facebookAppSecret" = $FacebookAppSecret
        "functionAppZipUri" = $FunctionAppZipUri
    }