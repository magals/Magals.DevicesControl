using System;
using System.Collections.Generic;
using System.Text;

namespace Magals.DevicesControl.SDKStandart.Interfeces.GPIO
{
	public interface IButton : IDevice
	{
		void Push();
	}
}
