using Azure.Storage.Blobs;
using System;
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

			var menuItems = new List<MenuItem>()
			{
				new MenuItem("a", "Get statistics of source", Program.GetStatistics),
				new MenuItem("b", "Test sync status between source and target", Program.AreInSync),
				new MenuItem("c", "Sync source to target", Program.Sync)
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
	}
}
