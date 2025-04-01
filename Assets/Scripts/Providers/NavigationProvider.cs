using UnityEngine;
using System.Collections.Generic;

public class NavigationProvider : MonoBehaviour
{
    public NavigationData navData;

    public Vector3 GetClosestNavPoint(Vector3 target)
    {
        if (navData.navPoints.Count == 0)
        {
            Debug.LogError("Nav Data is empty");
            return Vector3.zero;
        }

        var closestPoint = navData.navPoints[0];
        var closestDist = Vector3.Distance(closestPoint, target);
        for (int i = 1; i < navData.navPoints.Count - 1; i++)
        {
            var point = navData.navPoints[i];
            var dist = Vector3.Distance(target, point);
            if (dist < closestDist)
            {
                closestPoint = point;
                closestDist = dist;
            }
        }

        return closestPoint;
    }
}
