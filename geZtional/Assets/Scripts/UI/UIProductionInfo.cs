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

    private BasicFactory currentBuilding;

    public void SetCurrentBuilding(Building building)
    {
        currentBuilding = (BasicFactory)building; // TODO: fare meglio questo switch
        productionSprite.color = Color.red;
        productionInfo.text = "Nome unità";
        productionProgress.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentBuilding.ProductionQueue == 0)
            ResetValues();

        productionQuantity.text = $"{currentBuilding.ProductionQueue}/{currentBuilding.productionQueueLimit}";

        productionProgress.value = currentBuilding.TimePassed / currentBuilding.timeProductionUnit;
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
