using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingZombies : BuildingBase
{

    [Header("Building Settings")]
    public float BuildingLife;
    public float BuildCost;
    public float VirusProductionTimePerSecond;

    [Header("Unit Settings")]
    public Zombie ZombieProductionPrefab;
    public float VirusCostPerUnit;


    private void Awake()
    {
        FactionType = EFactionType.Zombie;
    }

    private void Update()
    {
        if (ProductionQueue > 0)
            ProduceUnit();

        ProduceVirus();
    }
    public override void ProduceUnit()
    {
        ProductionUnitTimePassed += Time.deltaTime * 1;
        if (ProductionUnitTimePassed >= TimeProductionUnit)
        {
            ProductionUnitTimePassed = 0;

            if (DestinationFlag == transform.position)
            {
                DestinationFlag = GetNextEmptySpace();
            }

            Zombie newUnit = Instantiate(ZombieProductionPrefab, GetNextEmptySpace(), Quaternion.identity);
            newUnit.Initialize();
            newUnit.SetDestination(DestinationFlag);

            RemoveToQueue();
        }
    }

    public void ProduceVirus()
    {
        ProductionResourceTimePassed += Time.deltaTime * 1;
        if(ProductionResourceTimePassed >= 1)
        {
            ProductionResourceTimePassed = 0;

            GlobalResourcesManager.Instance.TotalVirus += VirusProductionTimePerSecond;
        }
    }
    public override void AddToQueue()
    {
        if (GlobalResourcesManager.Instance.TotalVirus - VirusCostPerUnit >= 0 && ProductionQueue < ProductionQueueLimit)
        {
            GlobalResourcesManager.Instance.TotalVirus -= VirusCostPerUnit;
            base.AddToQueue();
        }
    }

    public void UseVirus(float virusAmount)
    {
        if (GlobalResourcesManager.Instance.TotalVirus - virusAmount >= 0)
        {
            GlobalResourcesManager.Instance.TotalVirus -= virusAmount;
        }
    }
}
