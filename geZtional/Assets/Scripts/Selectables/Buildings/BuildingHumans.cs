using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHumans : BuildingBase
{
    [Header("Building Settings")]
    public float BuildingLife;
    public float BuildCost;
    public List<EUnitType> HumanUnitAccepted;
    public int WorkersCountLimit;

    [Header("Unit Settings")]
    public Human HumanProductionprefab;
    public float GoldCostPerUnit;


    public void ProduceGold()
    {

    }
}
