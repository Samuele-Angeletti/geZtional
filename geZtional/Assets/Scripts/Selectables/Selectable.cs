using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class Selectable : MonoBehaviour
{
    [Header("WARNING: High performance required. Use it only for strictly reasons")]
    public UnityEvent OnSelectEvent;
    [Header("WARNING: High performance required. Use it only for strictly reasons")]
    public UnityEvent OnDeselectEvent;
    [HideInInspector] public bool IsSelected = false;

    [HideInInspector]public ESelectableType SelectableType;

    public virtual void OnSelect() { IsSelected = true; }
    public virtual void OnDeselect() { IsSelected = false; }
    public virtual void SetDestination(Vector3 destination)
    {
    }
}
