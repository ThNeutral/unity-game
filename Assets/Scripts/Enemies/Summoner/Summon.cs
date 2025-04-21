using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : BaseEnemy
{
    public override EnemyType Type => EnemyType.Summon;

    private NavigationPoint point;

    protected override bool HandleUniqueMove() 
    {
        if (point != null) 
        {
            moveDirection = point.DirectionWithLock(transform.position);
        }
        else if (target != null) 
        { 
            moveDirection = (target.transform.position - transform.position).normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        return true;
    }

    protected override void HandleInitialRoute() {}
    protected override void HandleRoute() {}
    
    public void SetTarget(MonoBehaviour target)
    {
        point = null;
        this.target = target; 
    }
    
    public void SetPoint(NavigationPoint point)
    {
        target = null;
        this.point = point;
    }

    public void Freeze()
    {
        target = null;
        point = null;
    }
}
