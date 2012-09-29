using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using netduino.helpers.Math;
using GyroAndAccelerometerTest.Extensions;

namespace GyroAndAccelerometerTest
{
    public class MPU6050
    {
        private Measurement InitialRawGyro = new Measurement();
        private Measurement LatestGyro = new Measurement();
        private Measurement LatestAcc = new Measurement();

        private const double gyroScale = 65.6;
        private const double time = 0.001;
        private const double complementFactor = 0.9;

        private double xAngle;
        private double yAngle;

        public MPU6050(ArrayList rawMeasurements)
        {
            if (rawMeasurements == null || rawMeasurements.Count == 0)
                throw new ArgumentException("Cannot initialize with specified values", "ramMeasurements");

            foreach (byte[] bits in rawMeasurements)
            {
                InitialRawGyro.X += bits.TwoBytesToInt( 8, 9);
                InitialRawGyro.Y += bits.TwoBytesToInt( 10, 11);
                InitialRawGyro.Z += bits.TwoBytesToInt( 12, 13);
            }

            InitialRawGyro.X /= rawMeasurements.Count;
            InitialRawGyro.Y /= rawMeasurements.Count;
            InitialRawGyro.Z /= rawMeasurements.Count;
        }

        public void Update(byte[] rawMeasurement)
        {
            UpdateAccelerometer(rawMeasurement);
            UpdateGyro(rawMeasurement);
            ComputeAngles();
        }

        private void UpdateAccelerometer(byte[] rawMeasurement)
        {
            LatestAcc.X = rawMeasurement.TwoBytesToInt(0, 1);
            LatestAcc.Y = rawMeasurement.TwoBytesToInt(2, 3);
            LatestAcc.Z = rawMeasurement.TwoBytesToInt(4, 5);
        }

        private void UpdateGyro(byte[] rawMeasurement)
        {
            double gyroX = rawMeasurement.TwoBytesToInt(8, 9);
            double gyroY = rawMeasurement.TwoBytesToInt(10, 11);
            double gyroZ = rawMeasurement.TwoBytesToInt(12, 13);

            LatestGyro.X = (gyroX - InitialRawGyro.X) / gyroScale * time;
            LatestGyro.Y = (gyroY - InitialRawGyro.Y) / gyroScale * time;
            LatestGyro.Z = (gyroZ - InitialRawGyro.Z) / gyroScale * time;
        }

        private void ComputeAngles()
        {
            //http://www.arduino.cc/cgi-bin/yabb2/YaBB.pl?num=1208368036
            double accXAngle = 57.295 * Trigo.Atan((float)this.LatestAcc.Y / Trigo.Sqrt(Trigo.Pow((float)this.LatestAcc.Z, 2) + Trigo.Pow((float)LatestAcc.X, 2)));
            double accYAngle = 57.295 * Trigo.Atan((float)-LatestAcc.X / Trigo.Sqrt(Trigo.Pow((float)LatestAcc.Z, 2) + Trigo.Pow((float)LatestAcc.Y, 2)));

            xAngle = xAngle * LatestGyro.X * (1 - complementFactor) + accXAngle * complementFactor;
            yAngle = yAngle * LatestGyro.Y * (1 - complementFactor) + accYAngle * complementFactor;
        }

        public void PrintAngles()
        {
            Debug.Print("X angle: " + xAngle);
            Debug.Print("Y angle: " + yAngle);
        }
    }
}
