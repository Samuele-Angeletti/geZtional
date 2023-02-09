using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Human : Selectable, IDestructible
{
    [SerializeField] GameObject ZombiePrefabToSpawn;
    [SerializeField] SpriteRenderer selectionGraphics;
    [SerializeField] GameObject graphics;
    [Header("Statistics")]
    [SerializeField] float life;
    [SerializeField] float damage;
    [SerializeField] float speedMovement;
    [SerializeField] float goldProductionPerSecond;
    [SerializeField] float restingTime;
    [SerializeField] float workingTime;

    BehaviorTree _behaviorTree;
    Damager _damager;
    Damageable _damageable;
    NavMeshAgent _agent;
    Collider _collider;
    float _currentWorkTime;
    float _currentRestTime;

    public bool IsResting;
    public bool IsWorking;
    private void Awake()
    {
        FactionType = EFactionType.Human;
        SelectableType = ESelectableType.Unit;
        _damageable = GetComponent<Damageable>();
        _damager = GetComponent<Damager>();
        _agent = GetComponent<NavMeshAgent>();
        _behaviorTree = GetComponent<BehaviorTree>();
        _collider = GetComponent<Collider>();

        _damager.Damage = damage;
        _damageable.MaxLife = life;
        _agent.speed = speedMovement;

        _behaviorTree.SetVariableValue("SpeedMovement", speedMovement);

        if (workingTime == -1)
            Debug.Log("work time = -1");
    }

    private void Update()
    {
        if(IsResting)
        {
            _currentRestTime += Time.deltaTime * 1;
            if(_currentRestTime >= restingTime)
            {
                IsResting = false;
            }
        }
        else if(IsWorking)
        {
            _currentWorkTime += Time.deltaTime * 1;
            if(_currentWorkTime >= workingTime)
            {
                IsWorking = false;
            }
        }
    }

    public void StartResting()
    {
        IsResting = true;
        _currentRestTime = 0;
    }

    public void StartWorking()
    {
        IsWorking = true;
        _currentWorkTime = 0;
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

    public float GoldProduced()
    {
        return goldProductionPerSecond;
    }

    public void ActiveVisibilityComponents(bool active)
    {
        _agent.enabled = active;
        _behaviorTree.enabled = active;
        _collider.enabled = active;
        graphics.SetActive(active);
    }
}
