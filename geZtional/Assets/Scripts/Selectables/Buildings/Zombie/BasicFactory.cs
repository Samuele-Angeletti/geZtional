using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BasicFactory : Building
{
    public const EBuildingType buildingType = EBuildingType.BasicFactory;

    [Space(20)]
    [Header("Basic Fatory Settings")]
    [SerializeField] Zombie UnitProductionPrefab;

    private void Awake()
    {
        FactionType = EFactionType.Zombie;
    }

    private void Update()
    {
        if(ProductionQueue > 0)
            Produce();
    }

    private void Produce()
    {
        TimePassed += Time.deltaTime;
        if (TimePassed >= timeProductionUnit)
        {
            TimePassed = 0;

            if(DestinationFlag == transform.position)
            {
                DestinationFlag = GetNextEmptySpace();
            }

            Zombie newUnit = Instantiate(UnitProductionPrefab, GetNextEmptySpace(), Quaternion.identity);
            newUnit.Initialize();
            newUnit.SetDestination(DestinationFlag);

            RemoveToQueue();
        }
    }

}
