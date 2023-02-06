using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonAction : MonoBehaviour
{
    public Selectable CurrentSelected;
    public void Action()
    {
        BuildingBase building = CurrentSelected as BuildingBase;
        if (building != null)
        {
            building.AddToQueue();
        }
        return;
    }
}
