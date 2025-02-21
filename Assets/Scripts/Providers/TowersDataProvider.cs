using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersDataProvider : MonoBehaviour
{
    [SerializeField]
    private List<TowerData> towerDatas;
    
    public List<TowerData> GetTowerDatas() { return towerDatas; }
}
