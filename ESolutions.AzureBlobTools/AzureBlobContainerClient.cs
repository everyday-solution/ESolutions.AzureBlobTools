using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.AzureBlobTools
{
	public class AzureBlobContainerClient : IContainerClient
	{
		//Fields
		#region blobContainerClient
		private BlobContainerClient blobContainerClient = null;
		#endregion

		#region guid
		private Guid guid = Guid.NewGuid();
		#endregion

		//Constructors
		#region AzureBlobContainerClient
		public AzureBlobContainerClient(String connectionString, String containerName)
		{
			this.blobContainerClient = new BlobContainerClient(connectionString, containerName);
		}
		#endregion

		//Methods
		#region GetStatistics
		public async Task<IStatistics> GetStatistics(Action<String> logging)
		{
			var result = new Statistics();

			void PageCallback(Page<BlobItem> page)
			{
				result.PageCount++;
			}

			void BlobCallback(BlobItem blobItem)
			{
				result.ContainerSize += (Decimal)(blobItem.Properties?.ContentLength ?? 0.0) / 1024 / 1024 / 1024; //Bytes => Gbytes
				result.ItemCount++;
			}

			await this.EnumerateBlobsInPages(logging, PageCallback, BlobCallback);

			return result;
		}
		#endregion

		#region EnumerateBlobsInPages
		public async Task EnumerateBlobsInPages(
			Action<String> logging,
			Action<Page<BlobItem>> pageCallback,
			Action<BlobItem> blobCallback)
		{
			var pages = this.blobContainerClient.GetBlobsAsync().AsPages(default, 100);
			var pageCount = 1;
			var itemCount = 1;

			await foreach (var page in pages)
			{
				logging($"Enumerate blob page: {pageCount}");
				pageCallback(page);

				foreach (var item in page.Values)
				{
					logging($"Enumerate blob: {itemCount}");
					blobCallback(item);
					itemCount++;
				}

				pageCount++;
			}
		}
		#endregion

		#region GetBlobClient
		public BlobClient GetBlobClient(String blobName)
		{
			return this.blobContainerClient.GetBlobClient(blobName);
		}
		#endregion

		#region DownloadAll
		public async Task DownloadAll(Action<string> logging)
		{
			void PageCallback(Page<BlobItem> page)
			{
			}

			void BlobCallback(BlobItem blobItem)
			{
				var sourceblob = this.blobContainerClient.GetBlobClient(blobItem.Name);
				var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				var downloadsPath = Path.Combine(userPath, "Downloads");
				var downloadsDI = new DirectoryInfo(downloadsPath);
				var singleDownloadsDI = downloadsDI.CreateSubdirectory(this.guid.ToString());

				var filePath = Path.Combine(singleDownloadsDI.FullName, blobItem.Name);
				var data = sourceblob.DownloadTo(filePath);
			}

			await this.EnumerateBlobsInPages(logging, PageCallback, BlobCallback);
		}
		#endregion

		#region DownloadOne
		public async Task DownloadOne(DirectoryInfo loacalDirectory, string filename, Action<string> logging)
		{
			var sourceBlob = this.blobContainerClient.GetBlobClient(filename);
			var filePath = Path.Combine(loacalDirectory.FullName, filename);
			var data = await sourceBlob.DownloadToAsync(filePath);
		}
		#endregion
	}
}
