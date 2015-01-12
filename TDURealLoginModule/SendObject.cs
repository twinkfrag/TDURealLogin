using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TDURealLoginModule
{
	[DataContract]
	public class SendObject
	{
		[DataMember(Name = nameof(SSIDs))]
		public IEnumerable<string> SSIDs { get; set; }

		//[DataMember(Name = "IPaddr")]
		//public string IPaddr = string.Empty;

		[DataMember(Name = nameof(name))]
		public string name { get; set; }
	}
}
