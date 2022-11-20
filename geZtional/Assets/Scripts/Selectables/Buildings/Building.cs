using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Selectable
{

    [Space(20)]
    [Header("")]
    [SerializeField] SpriteRenderer selectionGraphics;
    [SerializeField] GameObject flagRenderer;

    [Space(20)]
    [Header("Building Settings")]
    [SerializeField] public int productionQueueLimit;
    [SerializeField] public float timeProductionUnit;

    [HideInInspector] public float TimePassed = 0;
    [HideInInspector] public Vector3 DestinationFlag;
    [HideInInspector] public int ProductionQueue;
    

    GameObject _flag;
    BoxCollider _boxCollider;
    private void Start()
    {
        Publisher.Publish(new AddRemoveSelectableOnSceneMessage(true, this));
        SelectableType = ESelectableType.Building;
        DestinationFlag = transform.position;
        _boxCollider = GetComponent<BoxCollider>();
    }

    public override void OnSelect()
    {
        base.OnSelect();
        selectionGraphics.enabled = true;

        if(DestinationFlag != transform.position)
        {
            BuildFlag();
        }
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        selectionGraphics.enabled = false;
        Destroy(_flag);
    }

    public override void SetDestination(Vector3 destination)
    {
        DestinationFlag = destination;
        if (_flag != null)
            Destroy(_flag);

        BuildFlag();
    }

    private void BuildFlag()
    {
        _flag = Instantiate(flagRenderer, new Vector3(DestinationFlag.x, DestinationFlag.y + 0.2f, DestinationFlag.z), Quaternion.Euler(90, 0, 0));
        _flag.transform.parent = transform;
    }

    public Vector3 GetNextEmptySpace()
    {
        return _boxCollider.bounds.min;
    }

    public virtual void AddToQueue()
    {
        ProductionQueue = Mathf.Clamp(ProductionQueue + 1, 0, productionQueueLimit);
    }

    public virtual void RemoveToQueue()
    {
        ProductionQueue = Mathf.Clamp(ProductionQueue - 1, 0, productionQueueLimit);
    }
}
