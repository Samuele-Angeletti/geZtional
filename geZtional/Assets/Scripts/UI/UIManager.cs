using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("On Selected")]
    [SerializeField] GameObject onSelectedPanel;
    [SerializeField] TextMeshProUGUI selectedType;
    [SerializeField] TextMeshProUGUI selectedName;
    [SerializeField] Image selectedImage;

    [Header("Selected Building")]
    [SerializeField] UIBuildingActions buildingButtonsPanel;
    [SerializeField] GameObject buildingInfoPanel;
    [SerializeField] UIProductionInfo productionPanel;


    [Header("Selected Unit")]
    [SerializeField] GameObject unitButtonsPanel;
    [SerializeField] GameObject unitInfoPanel;

    [HideInInspector] public PlayerController PlayerController;
    [HideInInspector] public List<Selectable> CurrentSelectedlist;
    

    private void Start()
    {
        PlayerController = GameManager.Instance.PlayerController;
    }

    private void Update()
    {
        CurrentSelectedlist = PlayerController.ActiveSelectables;

        if (CurrentSelectedlist == null) return;

        if(CurrentSelectedlist.Count == 0)
        {
            CloseAll();
            return;
        }

        if (!onSelectedPanel.activeSelf)
            onSelectedPanel.SetActive(true);

        if(CurrentSelectedlist.Any(x => x.SelectableType == ESelectableType.Unit))
        {
            buildingButtonsPanel.gameObject.SetActive(false);
            buildingInfoPanel.SetActive(false);
            unitButtonsPanel.SetActive(true);
            unitInfoPanel.SetActive(true);
            return;
        }

        if(CurrentSelectedlist.Count == 1 && CurrentSelectedlist.Any(x => x.SelectableType == ESelectableType.Building))
        {
            unitButtonsPanel.SetActive(false);
            unitInfoPanel.SetActive(false);
            buildingButtonsPanel.gameObject.SetActive(true);
            buildingButtonsPanel.SetBuilding(CurrentSelectedlist[0]);
            buildingInfoPanel.SetActive(true);
            productionPanel.SetCurrentBuilding((BuildingZombies)CurrentSelectedlist[0]);
            selectedImage.color = Color.green;
            selectedName.text = CurrentSelectedlist[0].gameObject.name;
            selectedType.text = "Factory";
        }
    }

    private void CloseAll()
    {
        onSelectedPanel.SetActive(false);
    }
}
