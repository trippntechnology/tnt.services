using System.Runtime.Serialization;

namespace TNT.Services.Objects.Faults
{
	/// <summary>
	/// Fault details base
	/// </summary>
	[DataContract]
	public abstract class FaultDetails
	{
		/// <summary>
		/// Description of the fault
		/// </summary>
		[DataMember]
		virtual public string Description { get; set; }

		/// <summary>
		/// Initialization constructor
		/// </summary>
		/// <param name="description">Description of the fault</param>
		public FaultDetails(string description)
		{
			Description = description;
		}
	}
}
