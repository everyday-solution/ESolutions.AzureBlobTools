using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.AzureBlobTools
{
	public class Statistics : IStatistics
	{
		#region ItemCount
		public Int32 ItemCount
		{
			get;
			set;
		}
		#endregion

		#region PageCount
		public Int32 PageCount
		{
			get;
			set;
		}
		#endregion

		#region ContainerSize
		public Decimal ContainerSize
		{
			get;
			set;
		}
		#endregion
	}
}
