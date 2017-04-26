using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFRenderer.Primitives
{
    public struct Quaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;


        public static readonly Quaternion Zero = new Quaternion();
 
        public static void RotationYawPitchRoll(float yaw, float pitch, float roll, out Quaternion result)
        {
            float halfRoll = roll * 0.5f;
            float halfPitch = pitch * 0.5f;
            float halfYaw = yaw * 0.5f;

            float sinRoll = (float)Math.Sin(halfRoll);
            float cosRoll = (float)Math.Cos(halfRoll);
            float sinPitch = (float)Math.Sin(halfPitch);
            float cosPitch = (float)Math.Cos(halfPitch);
            float sinYaw = (float)Math.Sin(halfYaw);
            float cosYaw = (float)Math.Cos(halfYaw);

            result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
            result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
        }

 
        public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion result;
            RotationYawPitchRoll(yaw, pitch, roll, out result);
            return result;
        }

    }

}
