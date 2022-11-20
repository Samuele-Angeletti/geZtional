using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour, IDestructible
{
    [SerializeField] GameObject ZombiePrefabToSpawn;

    public void Kill()
    {
        Instantiate(ZombiePrefabToSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
