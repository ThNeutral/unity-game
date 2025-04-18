using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NavigationProvider
{
    public NavigationData navData;

    public List<NavigationPoint> GetRemainingRoute(EnemyType type, Vector3 from)
    {
        AssertNavDataIsNotNull();
        var path = navData.paths.FirstOrDefault(p => p.type == type);
        if (path == null)
        {
            Debug.LogError("No path found for enemy type " + type.ToString());
            return new();
        }
        var (_, index) = GetClosestNavPoint(path.path, from);
        if (index == -1) return new();
        return path.path.Skip(index).ToList();
    }

    private void AssertNavDataIsNotNull()
    {
        Assertion.NotNull(navData, "Navigation Data was not set. Check Navigation Provider.");
    }

    private (NavigationPoint pos, int index) GetClosestNavPoint(List<NavigationPoint> path, Vector3 target)
    {
        if (path.Count == 0)
        {
            Debug.LogError("Nav Data is empty");
            return (new NavigationPoint(), -1);
        }

        var closestPointIndex = 0;
        var closestPoint = path[0];
        var closestDist = Vector3.Distance(closestPoint.position, target);
        for (int i = 1; i < path.Count - 1; i++)
        {
            var point = path[i];
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
