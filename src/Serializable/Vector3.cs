using System;
using System.Collections.Generic;
using System.Globalization;

namespace WhitePlugin.Serializable;
[Serializable]
public class Vector3
{
    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public Vector3()
    {
    }

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3 ToUnityVector3() => new(X, Y, Z);

    public static Vector3? Deserialize(object? data)
    {
        if (data is Vector3 vector3)
        {
            return vector3;
        }

        if (data is Dictionary<object, object> dictionary)
        {
            float x = float.Parse(dictionary["x"].ToString(), CultureInfo.InvariantCulture);
            float y = float.Parse(dictionary["y"].ToString(), CultureInfo.InvariantCulture);
            float z = float.Parse(dictionary["z"].ToString(), CultureInfo.InvariantCulture);

            return new(x, y, z);
        }

        return null;
    }
}