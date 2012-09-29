using System;
using Microsoft.SPOT;

namespace GyroAndAccelerometerTest
{
    public class Measurement
    {
        public double X;
        public double Y;
        public double Z;

        public override string ToString()
        {
            var format = "F3";
            return "X: " + X.ToString(format) + " Y: " + Y.ToString(format) + " Z: " + Z.ToString(format);
        }
    }
}
