## Create a Docker container image and deploy it to Container Registry
<br>
1. Open a new Cloud Shell instance in the Azure portal.
<br>
<br>
2. At the Cloud Shell command prompt, change the active directory to ~/clouddrive.
---
**NOTE**

Note: The command to change directory in Bash is cd path.

---
<br>
3. At the Cloud Shell command prompt, create a new directory named ipcheck in the ~/clouddrive directory.

---
**NOTE**

Note: The command to create a new directory in Linux is mkdir directory name.

---
<br>
4. Change the active directory to ~/clouddrive/ipcheck.
<br>
<br>
5. Use the dotnet new console –output . –name ipcheck command to create a new .NET console application in the current directory.
<br>
<br>
6. Create a new file in the ~/clouddrive/ipcheck directory named Dockerfile.

---
**NOTE**

Note: The command to create a new file in Bash is touch file name. The file name Dockerfile is case sensitive

---
<br>
<br>
7. Open the embedded graphical editor in the context of the current directory.

---
**NOTE**

Note: You can open the editor by using the code . command or by selecting the editor button.

---


```bash
dotnet run
```


## Open Azure Cloud Shell and store Container Registry metadata
<br>
1. Open a new Cloud Shell instance.
<br>
<br>
2. At the Cloud Shell command prompt, use the az acr list command to get a list of all container registries in your subscription.
<br>
<br>
3. Use the following command to output the name of the most recently created container registry:

```bash
az acr list --query "max_by([], &creationDate).name" --output tsv
```

<br>
4. Use the following command to save the name of the most recently created container registry in a Bash shell variable named acrName:

```bash
acrName=$(az acr list --query "max_by([], &creationDate).name" --output tsv)
```
<br>
5. Use the following script to render the value of the Bash shell variable acrName:

```bash
echo $acrName
```

<br>
<br>
## Deploy a Docker container image to Container Registry

Change the active directory to ~/clouddrive/ipcheck.

Use the dir command to get the contents of the current directory.

Use the following command to upload the source code to your container registry and build the container image as a Container Registry task:

```bash
az acr build --registry $acrName --image ipcheck:latest .
```
