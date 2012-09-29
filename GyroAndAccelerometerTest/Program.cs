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
    public class Program
    {
        private static I2C mpu = new I2C(new I2CDevice.Configuration(0x68, 80));

        private static void DisableSleepMode()
        {
            mpu.Write(new byte[]{MPU6050Regs.PWR_MGMT_1, 0});
        }

        private static void WhoAmI()
        {
            var value = mpu.Read(MPU6050Regs.WHO_AM_I);
            Debug.Print("Who am i ? " + value[0].ToString());
        }

        // default at power-up:
        //    Gyro at 250 degrees second
        //    Acceleration at 2g
        //    Clock source at internal 8MHz
        //    The device is in sleep mode.
        //

        //Accelerator with +/-2g range = 16384 LSB/g. So a value of 16384 is 1 g.
        //Get the accelerator value during setup(), and save it in rawOffset.
        //In the loop() get the value and calculate the 'g' value:
        public static void Main()
        {
            Debug.EnableGCMessages(true);

            WhoAmI();

            DisableSleepMode();

            Thread.Sleep(1000);

            ArrayList calibrationMeasurements = new ArrayList();
            GetMeasurementsForCalibration(calibrationMeasurements);

            MPU6050 measurement = new MPU6050(calibrationMeasurements);

            while (true)
            {
                var bytes = GetMpuRawData();
                measurement.Update(bytes);
                Thread.Sleep(1);
                measurement.PrintAngles();
            }
        }

        private static void GetMeasurementsForCalibration(ArrayList calibrationMeasurements)
        {
            for (int i = 0; i < 30; i++)
            {
                calibrationMeasurements.Add(GetMpuRawData());
                Thread.Sleep(281);
            }
        }

        private static byte[] GetMpuRawData()
        {
            return mpu.Read(new byte[] { MPU6050Regs.ACCEL_XOUT_H }, 14);
        }
    }
}
