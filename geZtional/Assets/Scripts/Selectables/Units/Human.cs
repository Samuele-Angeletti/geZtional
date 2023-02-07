using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Selectable, IDestructible
{
    [SerializeField] GameObject ZombiePrefabToSpawn;
    [SerializeField] SpriteRenderer selectionGraphics;

    [Header("Statistics")]
    [SerializeField] float life;
    [SerializeField] float damage;
    [SerializeField] float speedMovement;
    [SerializeField] float goldProductionPerSecond;
    [SerializeField] float restingTime;
    [SerializeField] float workingTime;

    BehaviorTree behaviorTree;
    private void Awake()
    {
        FactionType = EFactionType.Human;
        SelectableType = ESelectableType.Unit;
    }
    public void Kill()
    {
        Instantiate(ZombiePrefabToSpawn, transform.position, Quaternion.identity);
        base.OnKilled();
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

    public float GoldProduced()
    {
        return goldProductionPerSecond;
    }
}
