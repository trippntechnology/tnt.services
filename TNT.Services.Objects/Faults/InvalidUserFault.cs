using System.Runtime.Serialization;

namespace TNT.Services.Objects.Faults
{
	/// <summary>
	/// Invalid user fault
	/// </summary>
	[DataContract]
	public class InvalidUserFault : FaultDetails
	{
		/// <summary>
		/// Initialization constructor
		/// </summary>
		/// <param name="description">Descrtion</param>
		public InvalidUserFault(string description)
			: base(description)
		{
		}
	}
}
