using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
	/// <summary>
	/// Fictitious connection type for testing
	/// </summary>
	[DataContract]
	[AttributeUsage(AttributeTargets.Class)]
	public class FictionalTypeConnectSettingsAttribute : Attribute
	{
		public FictionalTypeConnectSettingsAttribute()
		{
			abstractfield = "T9000";
		}

		public FictionalTypeConnectSettingsAttribute(string abstractfield)
		{
			this.abstractfield = abstractfield;
		}

		[DataMember]
		[Required]
		public string abstractfield { get; set; }
	}
}
