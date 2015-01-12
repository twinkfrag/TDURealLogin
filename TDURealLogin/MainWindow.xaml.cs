using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using TDURealLoginModule;
using TDURealLogin.Properties;

namespace TDURealLogin
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public MainWindow()
		{
			this.DataContext = this;
			InitializeComponent();

			DebugMode = App.DebugMode;
			if (DebugMode) this.Visibility = Visibility.Visible;

			var components = new Components();
			components.Exit += (s, e) => Application.Current.Shutdown();
			components.Open += (s, e) =>
			{
				this.Visibility = Visibility.Visible;
			};
			Application.Current.Exit += new ExitEventHandler((s, e) =>
			{
				try { components?.Dispose(); }
				catch { Console.WriteLine("cannot components Dispose"); }

				Settings.Default.Save();
			});

			tickEvent(this, null);
			timer = new DispatcherTimer(DispatcherPriority.Normal)
			{
				Interval = Settings.Default.timespan,
			};
			timer.Tick += tickEvent;
			timer.Start();
		}

		private EventHandler tickEvent => (s, e) =>
		{
			if (string.IsNullOrEmpty(Settings.Default.name))
			{
				this.Visibility = Visibility.Visible;
				MessageBox.Show(this, "User Name が空です。");
			}
			else
			{
				Task.Factory.StartNew(() =>
				{
					try
					{
						using (var module = new RealLoginModule())
						{
							module.GetData(Settings.Default.name);
							var data = module.SendData(Settings.Default.Dest_host, Settings.Default.Dest_port);
							Console.WriteLine("response message: {0}", data.message);
							ActiveUsers = (dynamic[])(data.active_users);
							foreach (var user in ActiveUsers)
							{
								Console.WriteLine("active: {0}; last access: {1}", user.name, user.last_access);
							}
						}
					}
					catch { Console.WriteLine("out of module error"); }
				});
			}
		};

		protected override void OnClosing(CancelEventArgs e)
		{
			this.Visibility = Visibility.Hidden;
			e.Cancel = true;
			base.OnClosing(e);
		}


		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("設定をリセットします。", "確認", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
				Settings.Default.Reset();
#if DEBUG
			ActiveUsers = new RecieveObject.User[] {
							new RecieveObject.User()
							{
								name = "name", last_access = "2015-01-10 20:33:19"
							},
							new RecieveObject.User()
							{
								name = "foo", last_access = "2015-01-10 21:32:00"
							},
							new RecieveObject.User()
							{
								name = "name", last_access = "2015-01-10 20:33:19"
							},
							new RecieveObject.User()
							{
								name = "foo", last_access = "2015-01-10 21:32:00"
							},
							new RecieveObject.User()
							{
								name = "name", last_access = "2015-01-10 20:33:19"
							},
							new RecieveObject.User()
							{
								name = "foo", last_access = "2015-01-10 21:32:00"
							},
							new RecieveObject.User()
							{
								name = "name", last_access = "2015-01-10 20:33:19"
							},
							new RecieveObject.User()
							{
								name = "foo", last_access = "2015-01-10 21:32:00"
							},
							new RecieveObject.User()
							{
								name = "name", last_access = "2015-01-10 20:33:19"
							},
							new RecieveObject.User()
							{
								name = "foo", last_access = "2015-01-10 21:32:00"
							},
						};
#endif
		}


		private bool _debugMode = false;
		public bool DebugMode
		{
			get { return _debugMode; }
			set
			{
				_debugMode = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DebugMode)));
			}
		}

		private IEnumerable<dynamic> _activeUsers;
		public IEnumerable<dynamic> ActiveUsers
		{
			get { return _activeUsers; }
			set
			{
				_activeUsers = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveUsers)));
			}
		}

		private DispatcherTimer timer;

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
