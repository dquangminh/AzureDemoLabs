Connect-AzAccount -MSI
$context = New-AzStorageContext -StorageAccountName msidemostorage

New-AzStorageContainer -Name docs -Context $context

echo "Demo" > demo.txt

#upload demo.txt to docs container
Set-AzStorageBlobContent -File ./demo.txt -Container docs -Blob blobdemo.txt -BlobType Block -Context $context

