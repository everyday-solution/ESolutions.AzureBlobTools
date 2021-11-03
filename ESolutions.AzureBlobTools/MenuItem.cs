using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.AzureBlobTools
{
	public class MenuItem
	{
		//Properties
		#region Key
		public String Key
		{
			get;
			set;
		}
		#endregion

		#region Title
		public String Title
		{
			get;
			set;
		}
		#endregion

		#region Action
		public MenuActionDelegate MenuAction
		{
			get;
			set;
		}
		#endregion

		//Delegates
		#region MenuActionDelegate
		public delegate Task MenuActionDelegate();
		#endregion

		//Constructors
		#region MenuItem
		public MenuItem(String key, String title, MenuActionDelegate menuAction)
		{
			this.Key = key;
			this.Title = title;
			this.MenuAction = menuAction;
		}
		#endregion
	}
}
