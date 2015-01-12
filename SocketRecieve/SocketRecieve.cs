using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Console;
using System.Net.Sockets;
using System.Net;
using TDURealLoginModule;
using Codeplex.Data;

namespace SocketRecieve
{
	class SocketRecieve
	{
		static void Main(string[] args)
		{
			var serv = new TcpListener(IPAddress.Any, 4000);
			serv.Start();

			Write("Wait");
			while (true)
			{
				try
				{
					if (serv.Pending())
					{
						WriteLine("Pending");
						using (var st = serv.AcceptTcpClient().GetStream())
						{
							WriteLine();
							var data = new byte[1024];
							st.Read(data, 0, data.Length);
							//var json = DynamicJson.Parse(Encoding.UTF8.GetString(data));

							WriteLine("Recieve: {0}", Encoding.UTF8.GetString(data));

							var res = DynamicJson.Serialize(new RecieveObject()
							{
								message = "Recieved!",
								active_users = new RecieveObject.User[]
								{
									new RecieveObject.User() { name = "foo", last_access = DateTime.Now.ToString() },
									new RecieveObject.User() { name = "hoge", last_access = DateTime.Now.ToString() },
									new RecieveObject.User() { name = "foo", last_access = DateTime.Now.ToString() },
									new RecieveObject.User() { name = "bar", last_access = DateTime.Now.ToString() },
									new RecieveObject.User() { name = "qux", last_access = DateTime.Now.ToString() },
									new RecieveObject.User() { name = "fuga", last_access = DateTime.Now.ToString() },
									new RecieveObject.User() { name = "piyo", last_access = DateTime.Now.ToString() },
									new RecieveObject.User() { name = "hoge", last_access = DateTime.Now.ToString() },
								}
							});
							//res = @"{""message"":""Recieved!"",""active_users"":[{""name"":""foo""},{""name"":""bar""}]}";
							WriteLine("Response: {0}", res);
							var res_byte = Encoding.UTF8.GetBytes(res);
							st.Write(res_byte, 0, res_byte.Length);
						}
					}
					else Write(".");
				}
				finally
				{
					System.Threading.Thread.Sleep(1000);
				}
			}
			try
			{
				serv?.Stop();
			}
			catch { }
		}
	}
}
