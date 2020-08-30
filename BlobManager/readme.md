### Creating an Azure Storage Account
```bash
az storage account create --location eastus
	--name dqminh1
	--resource-group dqminhRG
	--sku Standard_RAGRS
	--kind BlobStorage
	--access-tier Hot
```

### Query list Storage Account keys
```bash
az storage account keys list --account-name 
	--resource-group
	--output table
```

### Adding a container to Storage Account
```bash
az storage container create --name
	--account-name
	--account-key
```
### Upload a file to blob storage
```bash
az storage blob upload --container-name --name --file
```
### Upload collection of files 
```bash
az storage blob upload-batch --destination "MyContainer" --source "MyFolder" --pattern *.bmp
```
