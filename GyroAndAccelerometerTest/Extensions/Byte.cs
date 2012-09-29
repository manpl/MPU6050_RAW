using System;
using Microsoft.SPOT;

namespace GyroAndAccelerometerTest.Extensions
{
    public static class Byte
    {
        public static int ConvertTwoBitsToInt(this byte b, int higherIndexFromLeft, int lowerIndexFromLeft)
        {
            b = (byte)(b << (8 - (higherIndexFromLeft + 1)));
            return (b >> ((8 - (higherIndexFromLeft + 1)) + lowerIndexFromLeft));
        }
    }
}
