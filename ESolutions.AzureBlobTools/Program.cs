using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESolutions.AzureBlobTools
{
	public class Program
	{
		//Fields
		#region sourceClient
		private static IContainerClient sourceClient = null;
		#endregion

		#region targetClient
		private static IContainerClient targetClient = null;
		#endregion

		#region LocalDirectory
		private static DirectoryInfo localDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
		#endregion

		//Methods
		#region Main
		/// <summary>
		/// Defines the entry point of the application.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public static async Task Main(string[] args)
		{
			Program.sourceClient = Program.GetClient("Source");
			Program.targetClient = Program.GetClient("Target");
			Program.localDirectory = Program.GetLocalDirectory();

			var menuItems = new List<MenuItem>()
			{
				new MenuItem("a", "Get statistics of source", Program.GetStatistics),
				new MenuItem("b", "Test sync status between source and target", Program.AreInSync),
				new MenuItem("c", "Sync source to target", Program.Sync),
				new MenuItem("d", "Download all blobs", Program.DownloadAll),
				new MenuItem("e", "Download blobs by name", Program.DownloadByName),
				new MenuItem("f", "Upload blobs by name", Program.UploadByName),
			};

			await Program.DisplayMenu(menuItems);
		}
		#endregion

		#region DisplayMenu
		private async static Task DisplayMenu(List<MenuItem> menuItems)
		{
			var input = String.Empty;

			do
			{
				Console.WriteLine("=================");
				Console.WriteLine("Menu");
				foreach (var runner in menuItems)
				{
					Console.WriteLine($"{runner.Key}) {runner.Title}");
				}
				Console.WriteLine("x) Exit");

				input = Console.ReadLine();
				var menuAction = menuItems.FirstOrDefault(runner => runner.Key == input);
				if (menuAction != null)
				{
					await menuAction.MenuAction();
				}
			}
			while (input != "x");
		}
		#endregion

		#region GetClient
		/// <summary>
		/// Gets the client.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <returns></returns>
		public static IContainerClient GetClient(String title)
		{
			Console.WriteLine($"=> {title}");

			Console.Write("Key: ");
			var key = Console.ReadLine();

			Console.Write("Container: ");
			var container = Console.ReadLine();

			return new AzureBlobContainerClient(key, container);
		}
		#endregion

		#region GetLocalDirectory
		/// <summary>
		/// Gets the local directory.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <returns></returns>
		public static DirectoryInfo GetLocalDirectory()
		{
			Console.WriteLine($"=> local direcory");

			Console.Write("Path: ");
			var path = Console.ReadLine();

			return new DirectoryInfo(path);
		}
		#endregion

		#region GetStatistics
		public async static Task GetStatistics()
		{
			var statistics = await sourceClient.GetStatistics((log) => { Console.WriteLine(log); });
			Console.WriteLine("TOTAL pages		= " + statistics.PageCount);
			Console.WriteLine("TOTAL blobs		= " + statistics.ItemCount);
			Console.WriteLine("TOTAL GB (bytes)	= " + statistics.ContainerSize);
		}
		#endregion

		#region AreInSync
		public async static Task AreInSync()
		{
			await ContainerSyncer.AreInSync(sourceClient, targetClient, (log) => { Console.WriteLine(log); });
		}
		#endregion

		#region Sync
		public async static Task Sync()
		{
			await ContainerSyncer.Sync(sourceClient, targetClient, (log) => { Console.WriteLine(log); });
		}
		#endregion

		#region DownloadAll
		public async static Task DownloadAll()
		{
			await sourceClient.DownloadAll((log) => { Console.WriteLine(log); });
		}
		#endregion

		#region DownloadByName
		public async static Task DownloadByName()
		{
			Console.WriteLine("Type filename to download or x to exit: ");

			var filename = String.Empty;
			do
			{
				Console.Write("filename: ");
				filename = Console.ReadLine();
				if (filename != "x")
				{
					await Program.sourceClient.DownloadOne(Program.localDirectory, filename, (log) => { Console.WriteLine(log); });
				}
			}
			while (filename != "x");

			Console.WriteLine($"exit {nameof(DownloadByName)}");
		}
		#endregion

		#region UploadByName
		public async static Task UploadByName()
		{
			await Task.Run(() => { });
		}
		#endregion
	}
}
