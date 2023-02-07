using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProductionInfo : MonoBehaviour
{
    [SerializeField] Image productionSprite;
    [SerializeField] TextMeshProUGUI productionInfo;
    [SerializeField] TextMeshProUGUI productionQuantity;
    [SerializeField] Slider productionProgress;

    private BuildingBase currentBuilding;

    public void SetCurrentBuilding(BuildingBase building)
    {
        currentBuilding = (BuildingZombies)building; // TODO: fare meglio questo switch
        productionSprite.color = Color.red;
        productionInfo.text = "Nome unità";
        productionProgress.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentBuilding.ProductionQueue == 0)
            ResetValues();

        productionQuantity.text = $"{currentBuilding.ProductionQueue}/{currentBuilding.ProductionQueueLimit}";

        productionProgress.value = currentBuilding.ProductionUnitTimePassed / currentBuilding.TimeProductionUnit;
    }

    private void ResetValues()
    {
        productionSprite.color = Color.white;
        productionInfo.text = "";
        productionQuantity.text = "";
        productionProgress.value = 0;
        productionProgress.gameObject.SetActive(false);
    }
}
