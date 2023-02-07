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

    List<Human> insideHumanList;

    private void Awake()
    {
        insideHumanList = new List<Human>();
    }
    private void Update()
    {
        if (ProductionQueue > 0)
            ProduceUnit();

        ProduceGold();
    }

    public override void ProduceUnit()
    {
        ProductionUnitTimePassed += Time.deltaTime;
        if (ProductionUnitTimePassed >= TimeProductionUnit)
        {
            ProductionUnitTimePassed = 0;

            if (DestinationFlag == transform.position)
            {
                DestinationFlag = GetNextEmptySpace();
            }

            Human newUnit = Instantiate(HumanProductionprefab, GetNextEmptySpace(), Quaternion.identity);
            newUnit.Initialize();
            newUnit.SetDestination(DestinationFlag);

            RemoveToQueue();
        }
    }

    public void ProduceGold()
    {
        ProductionResourceTimePassed += Time.deltaTime * 1;
        if (ProductionResourceTimePassed >= 1)
        {
            ProductionResourceTimePassed = 0;
            float gold = 0;
            foreach (var human in insideHumanList)
            {
                gold += human.GoldProduced();
            }
            GlobalResourcesManager.Instance.TotalGold += gold;
        }
    }

    public void Enter(Human human)
    {
        insideHumanList.Add(human);
    }

    public void Exit(Human human)
    {
        insideHumanList?.Remove(human);
    }

    public override void AddToQueue()
    {
        if(GlobalResourcesManager.Instance.TotalGold - GoldCostPerUnit >= 0 && ProductionQueue < ProductionQueueLimit)
        {
            GlobalResourcesManager.Instance.TotalGold -= GoldCostPerUnit;
            base.AddToQueue();
        }
    }

    public void UseGold(float goldAmount)
    {
        if(GlobalResourcesManager.Instance.TotalGold - goldAmount >= 0)
        {
            GlobalResourcesManager.Instance.TotalGold -= goldAmount;
        }
    }
}
