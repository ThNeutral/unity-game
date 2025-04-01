using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NavigationProvider : MonoBehaviour
{
    public NavigationData navData;

    public List<Vector3> GetRemainingRoute(Vector3 from)
    {
        var (_, index) = GetClosestNavPoint(from);
        return navData.navPoints.Skip(index).ToList();
    }

    private (Vector3 pos, int index) GetClosestNavPoint(Vector3 target)
    {
        if (navData.navPoints.Count == 0)
        {
            Debug.LogError("Nav Data is empty");
            return (Vector3.zero, -1);
        }

        var closestPointIndex = 0;
        var closestPoint = navData.navPoints[0];
        var closestDist = Vector3.Distance(closestPoint, target);
        for (int i = 1; i < navData.navPoints.Count - 1; i++)
        {
            var point = navData.navPoints[i];
            var dist = Vector3.Distance(target, point);
            if (dist < closestDist)
            {
                closestPointIndex = i;
                closestPoint = point;
                closestDist = dist;
            }
        }

        return (closestPoint, closestPointIndex);
    }
}
