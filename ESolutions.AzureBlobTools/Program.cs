using Azure.Storage.Blobs;
using System;
using System.Threading.Tasks;

namespace ESolutions.AzureBlobTools
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var sourceKey = "";
			var sourceContainer = "";
			var targetKey = "";
			var targetContainer = "";

			var sourceClient = new AzureBlobContainerClient(sourceKey, sourceContainer) as IContainerClient;
			var targetClient = new AzureBlobContainerClient(targetKey, targetContainer) as IContainerClient;


			await ContainerSyncer.AreInSync(sourceClient, targetClient, (log) => { Console.WriteLine(log); });
			await ContainerSyncer.Sync(sourceClient, targetClient, (log) => { Console.WriteLine(log); });

			var statistics = await sourceClient.GetStatistics((log) => { Console.WriteLine(log); });
			Console.WriteLine("TOTAL pages		= " + statistics.PageCount);
			Console.WriteLine("TOTAL blobs		= " + statistics.ItemCount);
			Console.WriteLine("TOTAL GB (bytes)	= " + statistics.ContainerSize);
		}
	}
}
