#Requires -Version 7.2.0

[CmdletBinding(SupportsShouldProcess = $true)]
Param (
    [String] $SubscriptionId = 'c1bda0fc-c0e3-4b89-be00-0524b96b7b0f',
    [String] $ResourceGroupName = 'rg-dapr',
    [String] $ContainerAppEnvironment = 'cape-dapr-giovani',
    [String] $Location = 'australiaeast',
    [string] $BuildId = ((Get-Date).ToUniversalTime()).ToString('MMddHHmm'),
    [string] $ComponentsDirecory = '.\components'
)

try {
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.AddUserAgent("azdoetch$UI$($host.name)".replace(" ", "_"), "1.0")
}
catch { }

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 3

if ($VerbosePreference -eq "SilentlyContinue" -and $Env:SYSTEM_DEBUG) {
    #haven't passed -Verbose but do have SYSTEM_DEBUG set, so upgrade verbosity
    $VerbosePreference = "Continue"
}

$PSBoundParameters | Format-Table | Out-String | Write-Verbose

$components = Get-ChildItem -Path $ComponentsDirecory
ForEach ($componentPath in $components) {
    $componentNaming = $componentPath.Name.Split('.')
    $componentNamespace = $componentNaming[0]
    $componentName  = $componentNamespace + '.' + $componentNaming[1].ToLower()
    $componentType  = $componentNaming[2]

    Write-Host "Deploying Dapr component ${componentPath}."
    Write-Host "Namespace: ${componentNamespace}."
    Write-Host "Name: ${componentName}."
    Write-Host "Type: ${componentType}."
    Write-Host "`n"

    $output = `
        az containerapp env dapr-component set `
            --name $ContainerAppEnvironment `
            --resource-group $ResourceGroupName `
            --dapr-component-name $componentName `
            --yaml $componentPath
}