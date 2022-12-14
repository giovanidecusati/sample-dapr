#Requires -Version 7.2.0
#Requires -Modules Az.Resources

[CmdletBinding(SupportsShouldProcess = $true)]
Param (
    [Parameter(Mandatory)] [String] $SubscriptionId,
    [Parameter(Mandatory)] [String] $ResourceGroupName,
    [String] $BicepFilePath = '.\templates\resourceGroupCleanup.bicep',
    $TemplateOutFile = '.\out\main.json',
    [switch] $Force
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 3

if ($VerbosePreference -eq "SilentlyContinue" -and $Env:SYSTEM_DEBUG) {
    #haven't passed -Verbose but do have SYSTEM_DEBUG set, so upgrade verbosity
    $VerbosePreference = "Continue"
}

$PSBoundParameters | Format-Table | Out-String | Write-Verbose

$context = Get-AzContext
if (-not $context) {
    throw "Execute Connect-AzAccount and try again"
}

if ($context.Subscription.Id -ne $SubscriptionId) {
    Set-AzContext -SubscriptionId $SubscriptionId
}


    $outDir = Split-Path $TemplateOutFile
    New-Item -Path $outDir -ItemType Directory -ErrorAction Ignore | Out-Null
    Get-ChildItem -Path $outDir -Include *.* -File -Recurse | ForEach-Object { $_.Delete() }

Write-Output 'Building bicep.'
az bicep build --file $BicepFilePath --outfile $TemplateOutFile --verbose

Write-Output 'Cleaning up resources.'
$decision = 0

if (-not $Force) {
    $title = 'WARNING: This action will DELETE all resources in the resource group. This is irreversible.'
    $question = 'Do you want to continue?'
    $choices = '&Yes', '&No'
    $decision = $Host.UI.PromptForChoice($title, $question, $choices, 1)
}

if ($decision -eq 0) {
    New-AzResourceGroupDeployment `
        -Name (Split-Path $TemplateOutFile -LeafBase) `
        -ResourceGroupName $ResourceGroupName `
        -TemplateFile $TemplateOutFile `
        -Force `
        -Mode Complete `
        -ErrorVariable errorMessages

    if ($errorMessages) {
        Write-Output '', 'Template deployment returned the following errors:', @(@($ErrorMessages) | ForEach-Object { $_.Exception.Message.TrimEnd("`r`n") })
    }
}
else {
    Write-Host 'The user cancelled the operation.'
}