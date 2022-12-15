#Requires -Version 7.2.0
#Requires -Modules Az.Resources
#Requires -Modules Az.KeyVault

[CmdletBinding(SupportsShouldProcess = $true)]
Param (
    [Parameter(Mandatory)] [String] $SubscriptionId,
    [Parameter(Mandatory)] [String] $ResourceGroupName,
    [System.Object] $TemplateArgs,
    [string] $BuildId = ((Get-Date).ToUniversalTime()).ToString('MMddHHmm'),
    [switch] $ValidateOnly,
    $BicepFilePath = '.\templates\main.prereqs.bicep',
    $TemplateOutFile = '.\out\main.json'
)

Import-Module .\Function.psm1 -Force

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

$context = Get-AzContext
if (-not $context) {
    throw "Execute Connect-AzAccount and try again."
}

if ($context.Subscription.Id -ne $SubscriptionId) {
    Set-AzContext -SubscriptionId $SubscriptionId
}

try {
    $outDir = Split-Path $TemplateOutFile
    New-Item -Path $outDir -ItemType Directory | Out-Null
    Get-ChildItem -Path $outDir -Include *.* -File -Recurse | ForEach-Object { $_.Delete() }
}
catch {}

Write-Output 'Building bicep.'
az bicep build --file $BicepFilePath --outfile $TemplateOutFile --verbose

Write-Output 'Validating ARM Template.'
$errorMessages = Format-ValidationOutput (Test-AzResourceGroupDeployment `
        -ResourceGroupName $ResourceGroupName `
        -SkipTemplateParameterPrompt `
        -TemplateParameterObject $TemplateArgs `
        -TemplateFile $TemplateOutFile)
    
if ($errorMessages) {
    Write-Output '', 'Validation returned the following errors:', @($errorMessages), '', 'Template is invalid.'
    Write-Error -Message 'Template is invalid.'
}
else {
    Write-Output 'Template is valid.'
}

if (-not $ValidateOnly) {
    New-AzResourceGroupDeployment `
        -Name ((Split-Path $TemplateOutFile -LeafBase) + '-' + $BuildId) `
        -ResourceGroupName $ResourceGroupName `
        -SkipTemplateParameterPrompt `
        -TemplateFile $TemplateOutFile `
        -Force `
        -ErrorVariable errorMessages `
        -TemplateParameterObject $TemplateArgs `
        -OutVariable deploymentResult
    
    if ($errorMessages) {
        Write-Output '', 'Template deployment returned the following errors:', @(@($ErrorMessages) | ForEach-Object { $_.Exception.Message.TrimEnd("`r`n") })
    }

    $deploymentResult | Format-Table | Out-String | Write-Verbose
}