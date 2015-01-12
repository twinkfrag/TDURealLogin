using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace TDURealLogin
{
	/// <summary>
	/// NotifyIcon等のWindows.Forms Componentsを生成するクラスです。
	/// Exit, Open のToolStripMenuを提供し、クリック時にそのイベントを実行します。
	/// ダブルクリック時にはOpenイベントを起動します。
	/// </summary>
	class Components : IDisposable
	{
		#region Exit イベントハンドラー
		private EventHandler _exit = (s, e) => { };
		/// <summary>
		/// Exit ToolStripItemをクリックした時に発生するようバインドされます。
		/// </summary>
		public event EventHandler Exit
		{
			add
			{
				toolStripMenuItem_Exit.Click -= _exit;
				_exit += value;
				toolStripMenuItem_Exit.Click += _exit;
			}
			remove
			{
				toolStripMenuItem_Exit.Click -= _exit;
				_exit -= value;
				toolStripMenuItem_Exit.Click += _exit;
			}
		}
		#endregion

		#region Open イベントハンドラー
		private EventHandler _open = (s, e) => { };
		/// <summary>
		/// Open ToolStripItemをクリックした時に発生するようバインドされます。
		/// </summary>
		public event EventHandler Open
		{
			add
			{
				toolStripMenuItem_Open.Click -= _open;
				_open += value;
				toolStripMenuItem_Open.Click += _open;
			}
			remove
			{
				toolStripMenuItem_Open.Click -= _open;
				_open -= value;
				toolStripMenuItem_Open.Click += _open;
			}
		}
		#endregion

		#region Components

		private NotifyIcon notifyIcon;
		private ContextMenuStrip contextMenuStrip;
		private ToolStripMenuItem toolStripMenuItem_Exit;
		private ToolStripMenuItem toolStripMenuItem_Open;

		public Components()
		{
			//
			// Constructors
			//
			notifyIcon = new NotifyIcon();
			contextMenuStrip = new ContextMenuStrip();
			toolStripMenuItem_Exit = new ToolStripMenuItem(Properties.Resources.NotifyIcon_ToolStrip_Exit);
			toolStripMenuItem_Open = new ToolStripMenuItem(Properties.Resources.NotifyIcon_ToolStrip_Open);

			//
			// notifyIcon
			//
			notifyIcon.ContextMenuStrip = contextMenuStrip;
			//notifyIcon.Icon = Icon.ExtractAssociatedIcon("icon.ico");
			notifyIcon.Icon = SystemIcons.Application;
			notifyIcon.Text = Properties.Resources.NotifyIcon_Text;
			notifyIcon.Visible = true;
			notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);
			notifyIcon.MouseDoubleClick += new MouseEventHandler(notifyIcon_MouseDobleClick);

			//
			// contextMenuStrip
			//
			contextMenuStrip.Items.AddRange(new ToolStripItem[] {
				toolStripMenuItem_Exit,
				toolStripMenuItem_Open
			});
		}

		private void notifyIcon_MouseDobleClick(object sender, MouseEventArgs e)
		{
			var mi = typeof(ToolStripMenuItem).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic);
			mi.Invoke(toolStripMenuItem_Open, new object[] { e });
		}

		private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
		{
			var mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
			mi.Invoke(notifyIcon, null);
		}
		#endregion

		public void notifyIcon_ShowBalloonTipWithNoIcon(int timeout, string tipTitle, string tipText)
		{
			notifyIcon.ShowBalloonTip(timeout, tipTitle, tipText, ToolTipIcon.None);
		}

		#region Disposable
		public void Dispose()
		{
			notifyIcon.Visible = false;
			notifyIcon.Dispose();
			contextMenuStrip.Dispose();
			toolStripMenuItem_Exit.Dispose();
			toolStripMenuItem_Open.Dispose();
		}
		#endregion
	}
}
