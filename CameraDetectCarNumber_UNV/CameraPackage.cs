using CameraDetectCarNumber_UNV.Enums;
using System;
using System.Linq;
using System.Text;

namespace CameraDetectCarNumber_UNV
{
    public class CameraPackage
    {
        public byte[] StartFlag;
        public byte[] EndFlag;

        public int LenPackage;
        public byte[] UnknowType1 = new byte[4];
        public byte[] UnknowType2 = new byte[4];

        public byte[] Data;
        public string ViewData;

        public CodeResult CodeResult;

        public static CameraPackage ParseByteArray(byte[] array)
        {
            var result = new CameraPackage();
            if (array.Length < 60)
            {
                result.CodeResult = CodeResult.None;
                return result;
            }

            var start = FindBytes(array, CameraStruct.StartFlag);
            var end = FindBytes(array, CameraStruct.EndFlag);

            if (start == -1 || end == -1)
            {
                result.CodeResult = CodeResult.ErrorPackage;
                return result;
            }

            while (true)
            {
                start = FindBytes(array, CameraStruct.StartFlag);
                end = FindBytes(array, CameraStruct.EndFlag);
                if (start == -1 || end == -1)
                {
                    result.CodeResult = CodeResult.ErrorPackage;
                    return result;
                }
                else if (end - start < 56)
                {
                    Buffer.BlockCopy(array, end + CameraStruct.EndFlag.Length, array, 0, array.Length - end - CameraStruct.EndFlag.Length);
                    continue;
                }
                else if (end - start == 56)
                {
                    var temp = array;
                    array = new byte[60];
                    Buffer.BlockCopy(temp, 0, array, 0, 60);
                    break;
                }
                else
                {
                    result.CodeResult = CodeResult.ErrorPackage;
                    return result;
                }
            }

            result.StartFlag = array.Take(4).ToArray();
            result.EndFlag = array.Skip(56).Take(4).ToArray();
            result.LenPackage = BitConverter.ToInt32(array.Skip(4).Take(4).Reverse().ToArray(), 0);
            result.UnknowType1 = array.Skip(8).Take(4).ToArray();
            result.UnknowType2 = array.Skip(12).Take(4).ToArray();
            result.Data = array.Skip(16).Take(result.LenPackage - 8).ToArray();

            result.ViewData = Encoding.ASCII.GetString(result.Data.Where(x => x != 0x00).ToArray());

            result.CodeResult = CodeResult.DetectCar;
            return result;
        }


        private static int FindBytes(byte[] src, byte[] find, int startindex = 0)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            if (startindex != 0 && startindex > 0)
            {
                startindex += 1;
            }

            for (int i = startindex; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }
    }
}
