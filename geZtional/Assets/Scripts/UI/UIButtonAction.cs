using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonAction : MonoBehaviour
{
    public Selectable CurrentSelected;
    public void Action()
    {
        Building building = CurrentSelected as Building;
        if (building != null)
        {
            building.AddToQueue();
        }
        return;
    }
}
