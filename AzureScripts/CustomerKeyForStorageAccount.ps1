#custermanagedkey01

#associate the above key with an existing storage account using the following PowerShell commands
$storageAccount = Get-AzStorageAccount -ResourceGroupName "" -AccountName "dquangminhblob"

$keyVault = Get-AzKeyVault -VaultName $vaultName

$key = Get-AzKeyVaultKey -VaultName $keyVault.VaultName -Name "custermanagedkey01"

Set-AzKeyVaultAccessPolicy -VaultName $keyVault.VaultName -ObjectId $storageAccount.Identity.PrincipalId ` # App registration ObjectID
-PermissionsToKeys wrapkey,unwrapkey,get

Set-AzStorageAccount -ResourceGroupName $storageAccount.ResourceGroupName -AccountName $storageAccount.StorageAccountName `
-KeyvaultEncryption -KeyName $key.Name -KeyVersion $key.Version -KeyVaultUri $keyVault.VaultUri