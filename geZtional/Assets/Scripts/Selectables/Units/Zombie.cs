using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Selectable, IDestructible
{
    [SerializeField] SpriteRenderer selectionGraphics;
    BehaviorTree behaviorTree;
    public void Kill()
    {
        base.OnKilled();
    }
    private void Awake()
    {
        FactionType = EFactionType.Zombie;
        SelectableType = ESelectableType.Unit;
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Publisher.Publish(new AddRemoveSelectableOnSceneMessage(true, this));
        SelectableType = ESelectableType.Unit;
        behaviorTree = GetComponent<BehaviorTree>();
    }

    public override void OnSelect()
    {
        base.OnSelect();
        selectionGraphics.enabled = true;
        behaviorTree.SetVariableValue("Selected", true);
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        selectionGraphics.enabled = false;
        behaviorTree.SetVariableValue("Selected", false);
    }

    public override void SetDestination(Vector3 destination)
    {
        behaviorTree.SetVariableValue("NewDestination", true);
        behaviorTree.SetVariableValue("Destination", destination);
    }

    public override void SetTarget(GameObject target)
    {
        behaviorTree.SetVariableValue("TargetEnemy", target);
    }

}
