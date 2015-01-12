using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NativeWifi;
using System.Net.Sockets;
using System.Console;
using Codeplex.Data;

namespace TDURealLoginModule
{
	/// <summary>
	/// TDU Real Login のデータ取得、および送信機能を提供します。
	/// 初期化時点では送信データがnullなので、必ずGetData()してからSendData()します。
	/// </summary>
	public class RealLoginModule : IDisposable
	{
		private SendObject send = null;

		public void Dispose()
		{
			send = null;
		}

		/// <summary>
		/// 現在の環境から送信データを取得します。SendData()の前に必ず実行してください。
		/// </summary>
		public void GetData()
		{
			var ssids = new List<string>();

			try
			{
				new WlanClient().Interfaces.ToList().ForEach(wi =>
				{
					wi
					.GetAvailableNetworkList(Wlan.WlanGetAvailableNetworkFlags.IncludeAllAdhocProfiles)
					.Select(n => n.dot11Ssid)
					.ToList()
					.ForEach(ssid =>
					{
						if (ssid.SSID.FirstOrDefault() != 0)
							ssids.Add(new string(Encoding.UTF8.GetChars(ssid.SSID, 0, (int)ssid.SSIDLength)));
					});

					wi.ToString();
				});
				send = new SendObject()
				{
					SSIDs = new HashSet<string>(ssids),
				};
				WriteLine("Get SSIDs");
			}
			catch (System.ComponentModel.Win32Exception)
			{
				WriteLine("Cannot Access to Wi-Fi Device");
			}
			catch (Exception exc)	//Win32Exception: Not Exist Device
			{
				WriteLine(exc);
			}
			ssids.ForEach(WriteLine);
			WriteLine();
		}

		public void GetData(string twitter_id)
		{
			GetData();
			send.name = twitter_id;
		}


		/// <summary>
		/// GetData()で取得されたデータをJSONにシリアライズしてSocket通信で送信します。
		/// </summary>
		/// <param name="host">宛先のホスト名、またはIPアドレス</param>
		/// <param name="port">宛先のポート番号</param>
		public dynamic SendData(string host, int port)
		{
			string jsonData = DynamicJson.Serialize(send);
			WriteLine("Serialize: {0}", jsonData);

			WriteLine("try to connect: {0}:{1}", host, port);
			try
			{
				using (var tcp = new TcpClient(host, port))
				{
					WriteLine("Connect");

					using (var ns = tcp.GetStream())
					{
						var sendBytes = Encoding.UTF8.GetBytes(jsonData);
						ns.Write(sendBytes, 0, sendBytes.Length);
						WriteLine("sent");

						try
						{
							var res = new byte[2048];
							ns.Read(res, 0, res.Length);
							var resStr = Encoding.UTF8.GetString(res.TakeWhile(b => b != 0).ToArray());
							WriteLine("Recieve: {0}", resStr);
							return DynamicJson.Parse(resStr);
						}
						catch (Exception e) { WriteLine(e); }
					}
				}
			}
			catch (SocketException)
			{
				WriteLine("Cannot Connect");
			}
			return new RecieveObject();
		}
	}
}
