using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.AzureBlobTools
{
	public class ContainerSyncer
	{
		#region Sync
		public static async Task Sync(IContainerClient sourceClient, IContainerClient targetClient, Action<String> logging)
		{
			logging("Sync is starting....");
			var itemCount = 0;
			var contentSize = 0.0;

			void BlobItemCallback(BlobItem blobItem)
			{
				logging($"Processing: {blobItem.Name}");

				var sourceblob = sourceClient.GetBlobClient(blobItem.Name);
				var targetblob = targetClient.GetBlobClient(blobItem.Name);

				if (!targetblob.Exists())
				{
					var data = sourceblob.DownloadContent();
					targetblob.Upload(data.Value.Content);
					logging($"Created on target.");
				}
				else
				{
					logging($"Already exists in target.");
				}

				itemCount++;
				contentSize += (blobItem.Properties?.ContentLength ?? 0.0) / 1024 / 1024 / 1024; //Bytes => Gbytes
				Console.WriteLine("CNT: " + (itemCount) + " sum up to: " + contentSize);
			}

			await sourceClient.EnumerateBlobsInPages(
				logging,
				(p) => { },
				BlobItemCallback);

			logging("Sync is finished!");
		}
		#endregion

		#region AreInSync
		public static async Task AreInSync(IContainerClient sourceClient, IContainerClient targetClient, Action<String> logging)
		{
			logging("Sync check starting...");

			var filesChecked = 0;
			var filesOutOfSync = 0;
			var filesInSourceNotInTarget = 0;

			void BlobItemCallback(BlobItem blobItem)
			{
				var sourceblob = sourceClient.GetBlobClient(blobItem.Name);
				var targetblob = targetClient.GetBlobClient(blobItem.Name);

				if (targetblob.Exists())
				{
					var sourceLength = sourceblob.GetProperties()?.Value?.ContentLength;
					var targetLength = targetblob.GetProperties()?.Value?.ContentLength;

					if (sourceLength != targetLength)
					{
						filesOutOfSync++;
					}
				}
				else
				{
					filesInSourceNotInTarget++;
				}

				filesChecked++;
			}

			await sourceClient.EnumerateBlobsInPages(
				logging,
				(p) => { },
				BlobItemCallback);

			logging($"Files checked			: {filesChecked}");
			logging($"Files out of sync		: {filesOutOfSync}");
			logging($"Files not in target	: {filesInSourceNotInTarget}");
			logging("Sync check finished!");
		}
		#endregion
	}
}
