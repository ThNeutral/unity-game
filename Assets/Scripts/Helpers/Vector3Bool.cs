using System;
using UnityEngine;

[Serializable]
public struct Vector3Bool
{
    public static Vector3Bool False => new Vector3Bool(false, false, false);
    public static Vector3Bool True => new Vector3Bool(true, true, true);

    public bool x;
    public bool y;
    public bool z;
    public Vector3Bool(bool x, bool y, bool z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
