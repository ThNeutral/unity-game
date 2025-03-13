using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NavigationProvider : MonoBehaviour
{
    [SerializeField]
    private Bounds size;
    
    [SerializeField]
    private float step;

    private List<Vector3> gridPoints = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        if (step <= 0) return;

        for (float x = size.min.x; x <= size.max.x; x += step)
        {
            for (float z = size.min.z; z <= size.max.z; z += step)
            {
                Vector3 point = new Vector3(x, 0, z); 
                gridPoints.Add(point);
            }
        }
    }
    public Vector3 GetGridPoint(int row, int column)
    {
        int index = row * Mathf.CeilToInt((size.max.x - size.min.x) / step) + 1 + column;

        if (index >= 0 && index < gridPoints.Count)
        {
            return gridPoints[index];
        }
        else
        {
            throw new System.ArgumentOutOfRangeException("Row or column is out of bounds");
        }
    }
    public EnemyTarget GetNextPathNode(
        BaseEnemy enemy,
        Dictionary<BaseTower, bool> towers,
        EnemyTarget target
        )
    {
        var point = GetClosestGridPoint(enemy.transform.position);
        var adjastent = GetAdjastentGridPoint(point);

        return FindNextPoint(adjastent, towers, target);
    }
    private Vector3 GetClosestGridPoint(Vector3 target)
    {
        var closest = Vector3.zero;
        var smallestDistance = float.MaxValue;
        
        foreach (var point in gridPoints)
        {
            var distance = Vector3.Distance(target, point);
            if (distance < smallestDistance)
            {
                closest = point;
                smallestDistance = distance;
            }
        }

        return closest;
    }
    private List<Vector3> GetAdjastentGridPoint(Vector3 gridPoint)
    {
        return new List<Vector3> {
            gridPoint + new Vector3(step, 0, 0),
            gridPoint + new Vector3(-step, 0, 0),
            gridPoint + new Vector3(0, 0, step),
            gridPoint + new Vector3(0, 0, -step),
            gridPoint + new Vector3(step, 0, step),
            gridPoint + new Vector3(-step, 0, step),
            gridPoint + new Vector3(step, 0, -step),
            gridPoint + new Vector3(-step, 0, -step),
        };
    }
    private EnemyTarget FindNextPoint(
        List<Vector3> adjastent,
        Dictionary<BaseTower, bool> towers,
        EnemyTarget target
        )
    {
        EnemyTarget closest = null;
        var smallestDistance = float.MaxValue;

        foreach (var adj in adjastent)
        {
            if (IsPointThreatened(towers, adj)) continue;

            var distance = Vector3.Distance(adj, target.Position);
            if (distance < smallestDistance)
            {
                closest = new EnemyTarget { Position = adj };
                smallestDistance = distance;
            }
        }

        return closest;
    }
    private bool IsPointThreatened(Dictionary<BaseTower, bool> towers, Vector3 point)
    {
        if (towers == null) return true;

        foreach (var tower in towers.Keys)
        {
            if (Vector3.Distance(tower.transform.position, point) < tower.GetShootRadius())
            {
                return true;
            }
        }
        return false;
    }
}
