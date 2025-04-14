using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NavigationProvider
{
    public NavigationData navData;

    public List<NavigationPoint> GetRemainingRoute(Vector3 from)
    {
        AssertNavDataIsNotNull();
        var (_, index) = GetClosestNavPoint(from);
        return navData.navPoints.Skip(index).ToList();
    }

    private void AssertNavDataIsNotNull()
    {
        Assertion.NotNull(navData, "Navigation Data was not set. Check Navigation Provider.");
    }

    private (NavigationPoint pos, int index) GetClosestNavPoint(Vector3 target)
    {
        if (navData.navPoints.Count == 0)
        {
            Debug.LogError("Nav Data is empty");
            return (new NavigationPoint(), -1);
        }

        var closestPointIndex = 0;
        var closestPoint = navData.navPoints[0];
        var closestDist = Vector3.Distance(closestPoint.position, target);
        for (int i = 1; i < navData.navPoints.Count - 1; i++)
        {
            var point = navData.navPoints[i];
            var dist = Vector3.Distance(target, point.position);
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
