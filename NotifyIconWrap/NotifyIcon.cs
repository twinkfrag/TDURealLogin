using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forms = System.Windows.Forms;

namespace NotifyIconWrap
{
	class NotifyIcon
	{
		Forms.NotifyIcon notifyIcon;

		public NotifyIcon(bool visiblity = true)
		{
			notifyIcon = new Forms.NotifyIcon();
			notifyIcon.Visible = visiblity;
		}

		public Forms.ToolTipIcon BalloonTipIcon
		{
			get { return this.notifyIcon.BalloonTipIcon; }
			set { this.notifyIcon.BalloonTipIcon = value; }
		}
	}
}
