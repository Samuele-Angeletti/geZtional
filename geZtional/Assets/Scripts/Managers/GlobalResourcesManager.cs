using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalResourcesManager : MonoBehaviour
{
    #region SINGLETON
    private static GlobalResourcesManager instance;
    public static GlobalResourcesManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GlobalResourcesManager>();
                if (instance != null)
                    return instance;

                GameObject go = new GameObject("GlobalResourcesManager");
                return go.AddComponent<GlobalResourcesManager>();
            }
            else
                return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    //[Header("Zombies")]
    private float _virus;
    public float TotalVirus
    {
        get
        {
            return _virus;
        }
        set
        {
            _virus = value;
        }
    }
    public int MaxZombies;
    public int StartZombies;

    //[Header("Humans")]
    private float _gold;
    public float TotalGold
    {
        get { return _gold; }
        set
        {
            _gold = value;
        }
    }
    public int MaxHumans;
    public int StartHumans;

    private EFactionType currentFaction;

    public void SetInitialResources(Settings settings)
    {
        TotalVirus = settings.StartVirus;
        MaxZombies = settings.MaxZombies;
        StartZombies = settings.StartZombies;
        TotalGold = settings.StartGold;
        MaxHumans = settings.MaxHumans;
        StartHumans = settings.MaxHumans;
        currentFaction = settings.User;
    }
}
