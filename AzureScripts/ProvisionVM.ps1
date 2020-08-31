# Login
Connect-AzAccount

$location = centralus

# 1. Creating a resource group
$rg = New-AzResourceGroup -Name script-rg -Location $location

# 2. Creating a vnet, subnet
$vnet = New-AzVirtualNetwork -ResourceGroupName $rg.ResourceGroupName -AddressPrefix 10.2.0.0/16 -Name script-vnet-1 -Location $location
# add option subnet

# 3. Add a subnet to existing vnet
$subnet = Add-AzVirtualNetworkSubnetConfig -Name script-vnet-1-subnet-1 -VirtualNetwork $vnet -AddressPrefix 10.2.1.0/24
Set-AzVirtualNetwork -VirtualNetwork $vnet

#4. Creating a nsg and setting a rule
$nsg = New-AzNetworkSecurityGroup -Name script-nsg -ResourceGroupName $rg.ResourceGroupName -Location $location

$nsg | Add-AzNetworkSecurityRuleConfig -Name 'ALLOW_SSH_PORT_22' -Description 'allow remote desktop' -Protocol Tcp -SourceAddressPrefix Internet -SourcePortRange * -DestinationAddressPrefix * -DestinationPortRange 22 -Access Allow -Direction Inbound -Priority 200 -Verbose
$nsg | Set-AzNetworkSecurityGroup

#5. Creating a public IP address
$pip = New-AzPublicIpAddress -Name script-linux-1-pip-1 -ResourceGroupName $rg.ResourceGroupName -Location $rg.Location -AllocationMethod Static

#5 - Create a virtual network card and associate with public IP address and NSG
#First, let's get an object representing our current subnet
$subnet = $vnet.Subnets | Where-Object { $_.Name -eq 'psdemo-subnet-2' }

$nic = New-AzNetworkInterface -ResourceGroupName script-rg -Location centralus -Name 'script-linux-1-nic-1' -Subnet $subnet -PublicIpAddress $pip -NetworkSecurityGroup $nsg -Verbose

$nic

#6a - Create a virtual machine configuration
$LinuxVmConfig = New-AzureRmVMConfig `
    -VMName 'script-linux-1' `
    -VMSize 'Standard_B1S'

#6b - set the comptuer name, OS type and, auth methods.
$username = 'minhdq'
$password = ConvertTo-SecureString '123' -AsPlainText -Force
$LinuxCred = New-Object System.Management.Automation.PSCredential ($username, $password)

$LinuxVmConfig = Set-AzVMOperatingSystem `
    -VM $LinuxVmConfig `
    -Linux `
    -ComputerName 'script-linux-1' `
    -DisablePasswordAuthentication `
    -Credential $LinuxCred

#6c - Read in our SSH Keys and add to the vm config
$sshPublicKey = Get-Content "~/.ssh/id_rsa.pub"
Add-AzVMSshPublicKey `
    -VM $LinuxVmConfig `
    -KeyData $sshPublicKey `
    -Path "/home/$username/.ssh/authorized_keys"

#6d - get the VM image name, and set it in the VM config in this case RHEL/latest
Get-AzVMImageSku -Location $rg.Location -PublisherName "Redhat" -Offer "rhel"
$LinuxVmConfig = Set-AzVMSourceImage `
    -VM $LinuxVmConfig `
    -PublisherName 'Redhat' `
    -Offer 'rhel' `
    -Skus '7.4' `
    -Version 'latest' 

#6e - assign the created network interface to the vm
$LinuxVmConfig = Add-AzVMNetworkInterface `
    -VM $LinuxVmConfig `
    -Id $nic.Id 

# Create a virtual machine, passing in the VM Configuration, network, image etc are in the config.
New-AzmVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Location $rg.Location `
    -VM $LinuxVmConfig


#Deallocate the virtual machine
Stop-AzVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name $vm.Name `
    -Force

#Check the status of the VM to see if it's deallocated
Get-AzVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name $vm.Name `
    -Status 

#Mark the virtual machine as "generalized"
Set-AzVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name $vm.Name `
    -Generalized

#Start an Image Configuration from our source Virtual Machine $vm
$image = New-AzImageConfig `
    -Location $rg.Location `
    -SourceVirtualMachineId $vm.ID

#Create a VM from the custom image config we just created, simply specify the image config as a source.
$imageci = 'script-linux-ci-1'
New-AzImage `
    -ResourceGroupName $rg.ResourceGroupName `
    -Image $image `
    -ImageName $imageci

#Summary image information. You'll see two images, one Linux and on Windows.
Get-AzImage `
    -ResourceGroupName $rg.ResourceGroupName

#Create user object, this will be used for the Windows username/password
$password = ConvertTo-SecureString '123' -AsPlainText -Force
$WindowsCred = New-Object System.Management.Automation.PSCredential ($username, $password)

#Let's create a VM from our new image, we'll use a more terse definition for this VM creation
New-AzVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name "script-linux-1c" `
    -ImageName $imageci `
    -Location 'centralus' `
    -Credential $WindowsCred `
    -VirtualNetworkName '' `
    -SubnetName '' `
    -SecurityGroupName '' `

#Check out the status of our provisioned VM from the Image
Get-AzureRmVm `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name ""

$images = Get-AzureRMResource -ResourceType Microsoft.Compute/images 
$images.name
    
Remove-AzureRmImage `
    -ImageName myOldImage `
    -ResourceGroupName myResourceGroup
