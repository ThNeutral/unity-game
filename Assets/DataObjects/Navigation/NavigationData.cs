﻿using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NavigationData", menuName = "Scriptable Objects/Navigation Data")]
public class NavigationData : ScriptableObject
{
    public List<Vector3> navPoints;
}
