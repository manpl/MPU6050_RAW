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

//        private Measurement InitialRawAcc = new Measurement();
        private Measurement LatestAcc = new Measurement();

        private double xAngle;
        private double yAngle;

//        private double Temperature { get; set; }

        public MPU6050(ArrayList rawMeasurements)
        {
            if (rawMeasurements == null || rawMeasurements.Count == 0)
                throw new ArgumentException("Cannot initialize with specified values", "ramMeasurements");

            foreach (byte[] bits in rawMeasurements)
            {
                //InitialRawAcc.X +=  bits.TwoBytesToInt( 0, 1);
                //InitialRawAcc.Y +=  bits.TwoBytesToInt( 2, 3);
                //InitialRawAcc.Z +=  bits.TwoBytesToInt( 4, 5);
                
                InitialRawGyro.X += bits.TwoBytesToInt( 8, 9);
                InitialRawGyro.Y += bits.TwoBytesToInt( 10, 11);
                InitialRawGyro.Z += bits.TwoBytesToInt( 12, 13);
            }

            //InitialRawAcc.X /= rawMeasurements.Count;
            //InitialRawAcc.Y /= rawMeasurements.Count;
            //InitialRawAcc.Z /= rawMeasurements.Count;

            InitialRawGyro.X /= rawMeasurements.Count;
            InitialRawGyro.Y /= rawMeasurements.Count;
            InitialRawGyro.Z /= rawMeasurements.Count;
        }

        public void Update(byte[] rawMeasurement)
        {
            UpdateAccelerometer(rawMeasurement);

            //double rawTempReading = rawMeasurement.TwoBytesToInt(6, 7);
            //Temperature = ((double)rawTempReading + 12412.0) / 340.0;

            UpdateGyro(rawMeasurement);
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

            var scale = 65.5;
            var time = 0.001;

            LatestGyro.X = (gyroX - InitialRawGyro.X) / scale * time;
            LatestGyro.Y = (gyroY - InitialRawGyro.Y) / scale * time;
            LatestGyro.Z = (gyroZ - InitialRawGyro.Z) / scale * time;
        }

        public void ComputeAngles()
        {
            //http://www.arduino.cc/cgi-bin/yabb2/YaBB.pl?num=1208368036
            double accXAngle = 57.295 * Trigo.Atan((float)this.LatestAcc.Y / Trigo.Sqrt(Trigo.Pow((float)this.LatestAcc.Z, 2) + Trigo.Pow((float)LatestAcc.X, 2)));
            double accYAngle = 57.295 * Trigo.Atan((float)-LatestAcc.X / Trigo.Sqrt(Trigo.Pow((float)LatestAcc.Z, 2) + Trigo.Pow((float)LatestAcc.Y, 2)));

            var factor = 0.9;
            xAngle = xAngle * LatestGyro.X * (1-factor) + accXAngle * factor;
            yAngle = yAngle * LatestGyro.Y * (1-factor) + accYAngle * factor;
        }

        public void PrintAngles()
        {
            Debug.Print("X angle: " + xAngle);
            Debug.Print("Y angle: " + yAngle);
        }
    }
}
