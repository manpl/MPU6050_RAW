using System;
using Microsoft.SPOT;

namespace GyroAndAccelerometerTest
{
    public class MPU6050Regs
    {
        // http://www.invensense.com/mems/gyro/documents/RM-MPU-6000A.pdf
        public const byte GYRO_CONFIG = 0x1B;
        public const byte ACCE_CONFIG = 0x1C;
        public const byte FIFO_EN = 0x23;
        public const byte CONFIG = 0x1A;
        public const byte ACCEL_XOUT_H = 0x3B;  // register 25 determines sampling rate
        public const byte ACCEL_XOUT_L = 0x3C;
        public const byte ACCEL_YOUT_H = 0x3D;
        public const byte ACCEL_YOUT_L = 0x3E;
        public const byte ACCEL_ZOUT_H = 0x3F;
        public const byte ACCEL_ZOUT_L = 0x40;
        public const byte TEMP_OUT_H = 0x41;
        public const byte TEMP_OUT_L = 0x42;
        public const byte GYRO_XOUT_H = 0x43; // regiter 27 determines full scale  
        public const byte GYRO_XOUT_L = 0x44;
        public const byte GYRO_YOUT_H = 0x45;
        public const byte GYRO_YOUT_L = 0x46;
        public const byte GYRO_ZOUT_H = 0x47;
        public const byte GYRO_ZOUT_L = 0x48;
        public const byte PWR_MGMT_1 = 0x6B;

        public const byte SIGNAL_PATH_RESET = 0x68;

        public const byte WHO_AM_I = 0x75;
    }
}
