using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, IDestructible
{
    Selectable selectable;
    private void Awake()
    {
        selectable = gameObject.SearchComponent<Selectable>();
    }

    public void Kill()
    {
        selectable.OnKilled();
    }
}
