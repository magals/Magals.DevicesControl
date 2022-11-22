using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
  [DataContract]
  [AttributeUsage(AttributeTargets.Class)]
  public class GpioSettingsAttribute : Attribute
  {
    public bool FlagDefault = true;
    public GpioSettingsAttribute()
    {

    }

    public GpioSettingsAttribute(int[] outputHigh,
                                 int[] outputLow,
                                 int[] input,
                                 int[] inputPullDown,
                                 int[] InputPullUp)
    {
      this.outputHigh = outputHigh;
      this.outputLow = outputLow;
      this.input = input;
      this.inputPullDown = inputPullDown;
      this.inputPullUp = InputPullUp;

      FlagDefault = false;
    }

    [DataMember]
    public int[] outputHigh { get; set; } = new int[0];
    [DataMember]
    public int[] outputLow { get; set; } = new int[0];

    [DataMember]
    public int[] input { get; set; } = new int[0];

    [DataMember]
    public int[] inputPullDown { get; set; } = new int[0];

    [DataMember]
    public int[] inputPullUp { get; set; } = new int[0];

  }
}
