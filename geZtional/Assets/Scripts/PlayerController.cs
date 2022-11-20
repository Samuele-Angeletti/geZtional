using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISubscriber
{
    [Header("Selection Settings")]
    [SerializeField] int maxInLineFormation = 10;
    [SerializeField] float unitFormationOffset = 4;
    [SerializeField] GameObject unitDestinationEffect;


    private List<Selectable> allSelectables;
    private List<Selectable> activeSelectables;
    public List<Selectable> AllSelectables => allSelectables;
    public List<Selectable> ActiveSelectables => activeSelectables;
    void Awake()
    {
        activeSelectables = new List<Selectable>();
        allSelectables = new List<Selectable>();
        Publisher.Subscribe(this, typeof(AddRemoveSelectableOnSceneMessage));
    }

    public void Selection(List<Selectable> newSelectables)
    {
        activeSelectables.ForEach(x => x.OnDeselect());
        
        if (newSelectables == null)
        {
            activeSelectables.Clear();
            return;
        }

        activeSelectables.Clear();
        newSelectables.ForEach(x => x.OnSelect());

        activeSelectables.AddRange(newSelectables);
    }

    public void OnPublish(IPublisherMessage message)
    {
        if(message is AddRemoveSelectableOnSceneMessage addRemoveMessage)
        {
            if (addRemoveMessage.AddingNewSelectable)
            {
                allSelectables.Add(addRemoveMessage.Selectable);
            }
            else
            {
                allSelectables.Remove(addRemoveMessage.Selectable);
                Deselect(addRemoveMessage.Selectable);
            }
        }
    }

    private void Deselect(Selectable selectable)
    {
        if (activeSelectables.Contains(selectable))
            activeSelectables.Remove(selectable);
        selectable.OnDeselect();
    }

    public void OnDisableSubscribe()
    {
        Publisher.Unsubscribe(this, typeof(AddRemoveSelectableOnSceneMessage));
    }

    private void OnDestroy()
    {
        OnDisableSubscribe();
    }

    public void SetUnitsDestination(Vector3 destination)
    {
        Vector3 offSet = new(unitFormationOffset, 0, 0);
        
        int inLine = Mathf.Clamp(activeSelectables.Count / 2, 1, maxInLineFormation);

        for (int i = 0, j = 0; i < activeSelectables.Count; i++, j++)
        {
            if (i >= inLine)
            {
                destination.z += unitFormationOffset;
                inLine += maxInLineFormation;
                j = 0;
            }

            Vector3 nextDestination = destination + j * offSet;

            Instantiate(unitDestinationEffect, new Vector3(nextDestination.x, nextDestination.y + 0.2f, nextDestination.z), Quaternion.Euler(90,0,0));
            activeSelectables[i].SetDestination(nextDestination);
        }

    }
}
