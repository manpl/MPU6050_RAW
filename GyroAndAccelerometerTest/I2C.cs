using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace GyroAndAccelerometerTest
{
    public class I2C : I2CDevice
    {
        public I2C(I2CDevice.Configuration config) : base(config) { }
        
        public byte[] Read(byte[] RegisterNum, int length)
        {
            if (length == -1) length = RegisterNum.Length;
            I2CDevice.I2CTransaction[] xActions = new I2CDevice.I2CTransaction[2];

            xActions[0] = I2CDevice.CreateWriteTransaction(RegisterNum);
            byte[] RegisterValue = new byte[length];

            xActions[1] = I2CDevice.CreateReadTransaction(RegisterValue);
            var transfered = this.Execute(xActions, 1000);

            return RegisterValue;
        }

        public byte[] Read(params byte[] bytes)
        {
            return Read(bytes, bytes.Length);
        }
        
        public void Write(byte[] values)
        {
            I2CDevice.I2CTransaction[] xActions = new I2CDevice.I2CTransaction[1];
            xActions[0] = I2CDevice.CreateWriteTransaction(values);
            var transfered = this.Execute(xActions, 100);
        }
    }
}
