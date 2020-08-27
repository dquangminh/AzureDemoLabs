using System;
using Azure.Storage;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.Threading.Tasks;


namespace BlobManager
{
    class Program
    {
        private const string blobServiceEndpoint = "";
        private const string storageAccountName = "";
        private const string storageAccountKey = "";
        
        static async Task Main(string[] args)
        {
            StorageSharedKeyCredential accountCreds = new StorageSharedKeyCredential(
                storageAccountName,
                storageAccountKey
            );

            BlobServiceClient serviceClient = new BlobServiceClient(
                new Uri(blobServiceEndpoint),
                accountCreds
            );

            AccountInfo accountInfo = await serviceClient.GetAccountInfoAsync();
            
            await Console.Out.WriteLineAsync($"Account kind:\t{accountInfo?.AccountKind}");
            
            await EnumerateContainersAsync(serviceClient);

            BlobContainerClient container = await GetContainerAsync(serviceClient, "tsa");

            await EnumerateBlobsAsync(container);

            BlobClient blobClient = await GetBlobAsync(container, "crop-7.svg");
            await Console.Out.WriteLineAsync($"Blob:\t{blobClient.Uri}");


        }

        private static async Task EnumerateContainersAsync(BlobServiceClient client)
        {        
            await foreach (BlobContainerItem container in client.GetBlobContainersAsync()) {
                await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
            }
        }

        private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
        {
            BlobContainerClient container = client.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            return container;
        }

        private static async Task EnumerateBlobsAsync(BlobContainerClient container)
        {
            await foreach (BlobItem blob in container.GetBlobsAsync()) {
                await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
            }
        }

        private static async Task<BlobClient> GetBlobAsync(BlobContainerClient container, string blobName)
        {
            BlobClient blob = container.GetBlobClient(blobName);
            await Console.Out.WriteLineAsync($"Test Blob:\t{blob.Name}");
            return await Task.FromResult(blob);
        }
    }
}
