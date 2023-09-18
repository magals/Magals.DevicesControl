using System;
using System.Collections.Generic;
using System.Text;

namespace CameraDetectCarNumber_UNV
{
    public struct CameraStruct
    {
        public static byte[] StartFlag = new byte[] { 0x77, 0xAA, 0x77, 0xAA };
        public static byte[] EndFlag = new byte[] { 0x77, 0xAB, 0x77, 0xAB };
    }
}
