using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Selectable, IDestructible
{
    [SerializeField] SpriteRenderer selectionGraphics;

    [Header("Statistics")]
    [SerializeField] float life;
    [SerializeField] float damage;
    [SerializeField] float speedMovement;
    [SerializeField] float speedMovementOnAttack;
    [SerializeField, Range(1, 100)] float MutationChancePercent;

    BehaviorTree _behaviorTree;
    Damager _damager;
    Damageable _damageable;
    NavMeshAgent _agent;
    public void Kill()
    {
        base.OnKilled();
    }
    private void Awake()
    {
        FactionType = EFactionType.Zombie;
        SelectableType = ESelectableType.Unit;
        _damageable = GetComponent<Damageable>();
        _damager = GetComponent<Damager>();
        _agent = GetComponent<NavMeshAgent>();
        _behaviorTree = GetComponent<BehaviorTree>();

        if (damage == -1)
        {
            _damager.Damage = UnityEngine.Random.Range(41, 61);
            _damageable.MaxLife = UnityEngine.Random.Range(301, 501);
            _agent.speed = UnityEngine.Random.Range(40, 50);
        }
        else
        {
            _damager.Damage = damage;
            _damageable.MaxLife = life;
            _agent.speed = speedMovement;
        }

        _behaviorTree.SetVariableValue("SpeedMovement", speedMovement);
        _behaviorTree.SetVariableValue("SpeedMovementOnAttack", speedMovementOnAttack);
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Publisher.Publish(new AddRemoveSelectableOnSceneMessage(true, this));
        SelectableType = ESelectableType.Unit;
    }

    public override void OnSelect()
    {
        base.OnSelect();
        selectionGraphics.enabled = true;
        _behaviorTree.SetVariableValue("Selected", true);
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        selectionGraphics.enabled = false;
        _behaviorTree.SetVariableValue("Selected", false);
    }

    public override void SetDestination(Vector3 destination)
    {
        _behaviorTree.SetVariableValue("NewDestination", true);
        _behaviorTree.SetVariableValue("Destination", destination);
    }

    public override void SetTarget(GameObject target)
    {
        _behaviorTree.SetVariableValue("TargetEnemy", target);
    }

}
