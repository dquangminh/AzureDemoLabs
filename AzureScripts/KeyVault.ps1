Connect-AzureRmAccount

# creating a new Azure Key Vault

New-AzureRmKeyVault -VaultName 'PhoneBookPlusVault03' -ResourceGroupName 'test-rg' -Location 'East US'

# convert our secret value to a "secure string"
$secretvalue = 'https;AccountName=dquangminhblob;AccountKey=N6x8X7eXOTC4N+voJyaQn0lFoVmP6HZZRif1sfEZ8RWxygsyhg44IPVn8pjJG/AH/DwNKe9W2srMmDszslbv4A==;EndpointSuffix=core.windows.net'
$secretvalue =  ConvertTo-SecureString $secretvalue -asplaintext -Force

# add the secure string to our new Key Vault
$secret = Set-AzureKeyVaultSecret -VaultName 'PhoneBookPlusVault03' -Name 'StorageConnection' -SecretValue $secretvalue

$secret.Id

Set-AzureRmKeyVaultAccessPolicy -VaultName 'PhoneBookPlusVault03' -ServicePrincipalName 175bb359-4814-4fa4-800e-fa5226d0eae8 -PermissionsToSecrets decrypt,sign,get,unwrapKey #clientID

# enable soft delete on a key vault
($resource = Get-AzureRmResource -ResourceId (Get-AzureRmKeyVault -VaultName "AddressBookPlusVault03").ResourceId).Properties | Add-Member -MemberType "NoteProperty" -Name "enableSoftDelete" -Value "true"
Set-AzureRmResource -resourceid $resource.ResourceId -Properties $resource.Properties

# remove a key vault
Remove-AzureRmKeyVault -VaultName "AddressBookPlusVault03" -ResourceGroupName "Pluralsight"

# recover a "soft deleted vault"
Undo-AzureRmKeyVaultRemoval -VaultName "AddressBookPlusVault03" -ResourceGroupName "Pluralsight" -Location "East US"

#enable "Do Not Purge" on a key vault
($resource = Get-AzureRmResource -ResourceId (Get-AzureRmKeyVault -VaultName “AddressBookPlusVault03").ResourceId).Properties | Add-Member -MemberType NoteProperty -Name enablePurgeProtection  -Value "true"
Set-AzureRmResource -ResourceId $resource.ResourceId -Properties $resource.Properties

# permanantly delete a "soft deleted" key vault - does not work if "Do Not Purge" is enabled
Remove-AzureRmKeyVault -VaultName "AddressBookPlusVault03" -InRemovedState -Location "East US"
