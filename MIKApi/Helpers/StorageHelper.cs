using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage;
using MIKApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Diagnostics;
using Azure.Storage;

namespace MIKApi.Helpers
{
    public class StorageHelper
    {
        private readonly IConfiguration _configuration;
        private string StorageConnStr { get; set; }
        private string ContainerName { get; set; }
        public string StorageAccountName { get; }
        public string StorageAccountKey { get; }

        public StorageHelper(IConfiguration Configuration)
        {
            _configuration = Configuration;
            StorageConnStr = Configuration.GetSection("AzureStorageSecret")["StorageConnString"];
            ContainerName = Configuration.GetSection("AzureStorageSecret")["ContainerName"];
            StorageAccountName = Configuration.GetSection("AzureStorageSecret")["StorageAccountName"]; 
            StorageAccountKey = Configuration.GetSection("AzureStorageSecret")["StorageAccountKey"];

        }
        public NauhaDTO Upload(NauhaDTO nauhaDTO)
        {
            try
            {

                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobServiceClient = new BlobServiceClient(StorageConnStr);

              
                BlobServiceProperties prop = blobServiceClient.GetProperties();
                prop.DefaultServiceVersion = "2013-08-15";

                blobServiceClient.SetProperties(prop);

                // Get a reference to a container named "sample-container" and then create it
                BlobContainerClient container = new BlobContainerClient(StorageConnStr, ContainerName);
                container.CreateIfNotExists();
                
                string fileName = $"{nauhaDTO.LocationDto.LocationName.Replace(",","_")}\\{nauhaDTO.Year.Year}\\{Guid.NewGuid()}.mp3";

                // Get a reference to a blob named "sample-file" in a container named "sample-container"
                BlobClient blobClient = container.GetBlobClient(fileName);
                Debug.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
                nauhaDTO.UrlPath = blobClient.Uri.AbsoluteUri;
                using (var stream = nauhaDTO.fileBinaries.OpenReadStream())
                {
                    blobClient.Upload(stream);
                    
                }
                
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                nauhaDTO = null;
            }

            return nauhaDTO;
            
        }

    }
}
