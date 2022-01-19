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
	public interface IContainerClient
	{
		/// <summary>
		/// Gets the statistics.
		/// </summary>
		/// <returns></returns>
		Task<IStatistics> GetStatistics(
			Action<String> logging);

		/// <summary>
		/// Enumerates the blobs in pages.
		/// </summary>
		/// <returns></returns>
		Task EnumerateBlobsInPages(
			Action<String> logging,
			Action<Page<BlobItem>> pageCallback,
			Action<BlobItem> blobCallback);

		/// <summary>
		/// Gets the BLOB client.
		/// </summary>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <returns></returns>
		BlobClient GetBlobClient(
			String blobName);

		/// <summary>
		/// Downloads all.
		/// </summary>
		/// <param name="logging">The logging.</param>
		/// <returns></returns>
		Task DownloadAll(Action<String> logging);

		/// <summary>
		/// Downloads the one.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="logging">The logging.</param>
		/// <returns></returns>
		Task DownloadOne(DirectoryInfo localDirectory, String filename, Action<String> logging);
	}
}
