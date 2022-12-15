function Format-ValidationOutput {
    param ($ValidationOutput, [int] $Depth = 0)
    Set-StrictMode -Off
    return @($ValidationOutput | Where-Object { $_ -ne $null } | ForEach-Object { @('  ' * $Depth + ': ' + $_.Message) + @(Format-ValidationOutput @($_.Details) ($Depth + 1)) })
}

function Get-CurrentUserObjectID {
    $ctx = Get-AzContext

    #This is different for users that are internal vs external
    #We can use Mail for users and guests
    $User = Get-AzADUser -Mail $ctx.Account.id
    if (-not $user) {  #Try UPN
        $User = Get-AzADUser -UserPrincipalName $ctx.Account.Id
    }
    if (-not $User) { #User was not found by mail or UPN, try MailNick
        $mail = ($ctx.Account.id -replace "@","_" ) + "#EXT#"
        $User = Get-AzADUser | Where-Object { $_MailNick -EQ $Mail}
    }

    Return $User.id
}

Export-ModuleMember -Function *