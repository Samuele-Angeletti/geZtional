using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildingActions : MonoBehaviour
{

    [SerializeField] List<UIButtonAction> buttons;

    public void SetBuilding(Selectable selectable)
    {
        buttons.ForEach(x => x.CurrentSelected = selectable);
    }
}
