using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TDURealLogin
{
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			foreach(var arg in e.Args)
			{
				if(arg.Equals("/debug", StringComparison.CurrentCultureIgnoreCase))
				{
					DebugMode = true;
				}
			}
#if DEBUG
			DebugMode = true;
#endif
		}

		public static bool DebugMode = false;
	}
}
