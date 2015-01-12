using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TDURealLoginModule
{
	[DataContract]
	public class RecieveObject
	{
		[DataMember(Name = nameof(active_users))]
		public IEnumerable<User> active_users { get; set; }

		[DataMember(Name = nameof(message))]
		public string message { get; set; }


		[DataContract]
		public class User
		{
			[DataMember(Name = nameof(name))]
			public string name { get; set; }

			[DataMember(Name = nameof(last_access))]
			public string last_access { get; set; }
		}
	}
}
