using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStartSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown factionType;
    [SerializeField] TMP_Dropdown difficultMode;
    [SerializeField] TextMeshProUGUI startResourcesLabel;
    [SerializeField] TextMeshProUGUI startUnitsLabel;
    [SerializeField] TextMeshProUGUI maxUnitsLabel;
    [SerializeField] TextMeshProUGUI startResourcesValue;
    [SerializeField] TextMeshProUGUI startUnitsValue;
    [SerializeField] TextMeshProUGUI maxUnitsValue;

    public void UpdateValues()
    {
        var setting = GameManager.Instance.SetInitialSetting(GetMode(), GetFaction());

        if(setting.User == EFactionType.Zombie)
        {
            startResourcesLabel.text = "Initial Virus";
            startUnitsLabel.text = "Initial Zombies";
            maxUnitsLabel.text = "Max Production Zombies";
            startResourcesValue.text = $"{setting.StartVirus}";
            startUnitsValue.text = $"{setting.StartZombies}";
            maxUnitsValue.text = $"{setting.MaxZombies}";
        }
        else
        {
            startResourcesLabel.text = "Initial Gold";
            startUnitsLabel.text = "Initial Humans";
            maxUnitsLabel.text = "Max Production Humans";
            startResourcesValue.text = $"{setting.StartGold}";
            startUnitsValue.text = $"{setting.StartHumans}";
            maxUnitsValue.text = $"{setting.MaxHumans}";

        }
    }

    private void OnEnable()
    {
        UpdateValues();
    }

    private EDifficultMode GetMode()
    {
        return difficultMode.value switch
        {
            0 => EDifficultMode.Easy,
            1 => EDifficultMode.Medium,
            2 => EDifficultMode.Hard,
            3 => EDifficultMode.Thriller,
            _ => EDifficultMode.Medium
        };
    }

    private EFactionType GetFaction()
    {
        return factionType.value switch
        {
            0 => EFactionType.Zombie,
            1 => EFactionType.Human,
            _ => EFactionType.Zombie
        };
    }
}
