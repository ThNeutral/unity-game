using System;
using UnityEngine;

[Serializable]
public class NavigationPoint
{
    public Vector3Bool lockedAxes = Vector3Bool.False;
    public Vector3 size = Vector3.one;
    public Vector3 position = Vector3.zero;
    public Color color = Color.yellow;
    public Bounds Bounds => new(position, size);

    public bool Contains(Vector3 point)
    {
        return Bounds.Contains(point);
    }

    public float DistanceWithLock(Vector3 from)
    {
        var fromWithLock = WithLock(from);
        var toWithLock = WithLock(position);
        return Vector3.Distance(fromWithLock, toWithLock);
    }

    public Vector3 DirectionWithLock(Vector3 from)
    {
        var fromWithLock = WithLock(from);
        var toWithLock = WithLock(position);
        return (toWithLock - fromWithLock).normalized;
    }

    private Vector3 WithLock(Vector3 source)
    {
        if (lockedAxes.x) source.x = 0;
        if (lockedAxes.y) source.y = 0;
        if (lockedAxes.z) source.z = 0;
        return source;
    }
}
