using System;
using Microsoft.SPOT;

namespace GyroAndAccelerometerTest.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int TwoBytesToInt(this byte[] bytes, int higherIndex, int lowerIndex)
        {
            return ((bytes[higherIndex] << 8) | bytes[lowerIndex]);
        }

        public static string ToSingleString(this byte[] bytes)
        {
            string result = "";

            foreach (byte b in bytes)
            {
                result += b.ToString() + "\t";
            }

            return result;
        }
    }
}
