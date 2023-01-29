using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class Selectable : MonoBehaviour
{
    [HideInInspector] public bool IsSelected = false;

    [HideInInspector] public ESelectableType SelectableType;
    [HideInInspector] public EFactionType FactionType;
    public virtual void OnSelect() { IsSelected = true; }
    public virtual void OnDeselect() { IsSelected = false; }
    public virtual void SetDestination(Vector3 destination) { }
    public virtual void SetTarget(GameObject target) { }
    public virtual void OnKilled()
    {
        Publisher.Publish(new AddRemoveSelectableOnSceneMessage(false, this));
        Destroy(gameObject);
    }
}
