$azureAccountName="duongminh97@hotmail.com"
$azurePassword=ConvertTo-SecureString "dMinhhd?97" -AsPlainText -Force

$psCred=New-Object System.Management.Automation.PSCredential($azureAccountName, $azurePassword)

Login-AzureRmAccount -Credential $psCred -Debug