using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.AzureBlobTools
{
	public interface IStatistics
	{
		/// <summary>
		/// Gets the item count.
		/// </summary>
		/// <value>
		/// The item count.
		/// </value>
		Int32 ItemCount { get; }

		/// <summary>
		/// Gets the page count.
		/// </summary>
		/// <value>
		/// The page count.
		/// </value>
		Int32 PageCount { get; }

		/// <summary>
		/// Gets the size of the container in bytes.
		/// </summary>
		/// <value>
		/// The size of the container.
		/// </value>
		Decimal ContainerSize { get; }
	}
}
