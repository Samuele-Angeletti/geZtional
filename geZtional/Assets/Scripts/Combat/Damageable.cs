using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Damageable : MonoBehaviour
{
    [HideInInspector] public float MaxLife;

    float _currentLife;
    
    IDestructible _destructible;
    NavMeshAgent _agent;
    BehaviorTree _behaviorTree;
    Collider _collider;
    private void Start()
    {
        _currentLife = MaxLife;
        _destructible = gameObject.SearchComponent<IDestructible>();

        _agent = gameObject.SearchComponent<NavMeshAgent>();
        _behaviorTree = gameObject.SearchComponent<BehaviorTree>();
        _collider = gameObject.SearchComponent<Collider>();
    }

    public void Damage(float amount)
    {
        _currentLife -= amount;
        
        if(_currentLife <= 0)
        {
            _destructible.Kill();
        }
        else
        {
            StartCoroutine(GotDamageCoroutine());
        }
    }

    public IEnumerator GotDamageCoroutine()
    {
        if (_agent != null)
            _agent.enabled = false;
        if (_behaviorTree != null)
            _behaviorTree.enabled = false;
        if (_collider != null)
            _collider.enabled = false;

        yield return new WaitForSeconds(0.5f);

        if (_agent != null)
            _agent.enabled = true;
        if (_behaviorTree != null)
            _behaviorTree.enabled = true;
        if (_collider != null)
            _collider.enabled = true;
    }
}
