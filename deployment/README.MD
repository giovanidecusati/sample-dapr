Connect-AzAccount -SubscriptionId c1bda0fc-c0e3-4b89-be00-0524b96b7b0f

$deploymentArgs = @{
    SubscriptionId    = 'c1bda0fc-c0e3-4b89-be00-0524b96b7b0f'
    ResourceGroupName = 'rg-giodapr-lab-ause'
    SolutionName      = 'giodapr'
    EnvironmentName   = 'lab'
}

.\Deploy-Infrastructure.ps1 @deploymentArgs -Verbose