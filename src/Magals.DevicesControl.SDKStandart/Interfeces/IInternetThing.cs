using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
	public interface IInternetThing : IDevice
	{
		void SendCommand(string command);
		object GetData(string command);
	}
}
