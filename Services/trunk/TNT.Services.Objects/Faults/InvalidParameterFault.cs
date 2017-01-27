using System.Runtime.Serialization;

namespace TNT.Services.Objects.Faults
{
	/// <summary>
	/// Invalid parameter fault
	/// </summary>
	[DataContract]
	public class InvalidParameterFault : FaultDetails
	{
		/// <summary>
		/// Initialization constructor
		/// </summary>
		/// <param name="description"></param>
		public InvalidParameterFault(string description)
			: base(description)
		{
		}
	}
}
