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

    public override void AddToQueue()
    {
        
        base.AddToQueue();
    }
    private void Update()
    {
        if (ProductionQueue > 0)
            ProduceUnit();
    }
    public override void ProduceUnit()
    {
        TimePassed += Time.deltaTime;
        if (TimePassed >= TimeProductionUnit)
        {
            TimePassed = 0;

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

    }

}
