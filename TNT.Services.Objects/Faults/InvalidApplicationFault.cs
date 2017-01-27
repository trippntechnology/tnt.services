using System.Runtime.Serialization;

namespace TNT.Services.Objects.Faults
{
	/// <summary>
	/// Fault to indicate the application specified is invalid
	/// </summary>
	[DataContract]
	public class InvalidApplicationFault : FaultDetails
	{
		/// <summary>
		/// Initialization constructor
		/// </summary>
		/// <param name="description"></param>
		public InvalidApplicationFault(string description)
			: base(description)
		{
		}
	}
}
