#custermanagedkey01

#associate the above key with an existing storage account using the following PowerShell commands
$storageAccount = Get-AzureRmStorageAccount -ResourceGroupName "Pluralsight" -AccountName "myaddressbookplus"

$keyVault = Get-AzureRmKeyVault -VaultName $vaultName

$key = Get-AzureKeyVaultKey -VaultName $keyVault.VaultName -Name "custermanagedkey01"

Set-AzureRmKeyVaultAccessPolicy -VaultName $keyVault.VaultName -ObjectId $storageAccount.Identity.PrincipalId `
-PermissionsToKeys wrapkey,unwrapkey,get

Set-AzureRmStorageAccount -ResourceGroupName $storageAccount.ResourceGroupName -AccountName $storageAccount.StorageAccountName `
-KeyvaultEncryption -KeyName $key.Name -KeyVersion $key.Version -KeyVaultUri $keyVault.VaultUri