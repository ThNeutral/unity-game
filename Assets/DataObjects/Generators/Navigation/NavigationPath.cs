using System;
using System.Collections.Generic;

[Serializable]
public class NavigationPath
{
    public EnemyType type;
    public List<NavigationPoint> path;
}