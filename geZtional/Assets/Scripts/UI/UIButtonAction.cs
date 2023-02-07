using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonAction : MonoBehaviour
{
    public Selectable CurrentSelected;
    public void Action()
    {
        switch (CurrentSelected.FactionType)
        {
            case EFactionType.Zombie:
                BuildingZombies buildingZombie = CurrentSelected as BuildingZombies;

                if (buildingZombie != null)
                {
                    buildingZombie.AddToQueue();
                }
                break;
            case EFactionType.Human:
                BuildingBase buildingHumans = CurrentSelected as BuildingHumans;

                if (buildingHumans != null)
                {
                    buildingHumans.AddToQueue();
                }
                break;
        }
    }
}
