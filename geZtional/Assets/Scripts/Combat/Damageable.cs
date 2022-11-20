using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Damageable : MonoBehaviour
{
    [SerializeField] float maxLife;

    [HideInInspector] public EDamageableType DamageableType;

    float currentLife;
    
    IDestructible destructible;
    Rigidbody rigidBody;
    NavMeshAgent agent;
    BehaviorTree behaviorTree;
    private void Start()
    {
        currentLife = maxLife;
        destructible = gameObject.SearchComponent<IDestructible>();

        HumanBuilding humanBuilding = gameObject.SearchComponent<HumanBuilding>();
        if (humanBuilding != null)
            DamageableType = EDamageableType.HumanBuilding;
        else
        {
            Human human = gameObject.SearchComponent<Human>();
            if (human != null)
                DamageableType = EDamageableType.Human;
            else
            {
                Building building = gameObject.SearchComponent<Building>();
                DamageableType = building != null ? EDamageableType.ZombieBuilding : EDamageableType.Zombie;
            }
        }
        agent = gameObject.SearchComponent<NavMeshAgent>();
        behaviorTree = gameObject.SearchComponent<BehaviorTree>();
        rigidBody = gameObject.SearchComponent<Rigidbody>();
    }

    public void Damage(float amount, float forceAttack, Vector3 impactPoint)
    {
        currentLife -= amount;
        
        if(currentLife <= 0)
        {
            destructible.Kill();
        }
        else
        {
            StartCoroutine(GotDamageCoroutine());
            rigidBody.AddForce(new Vector3(impactPoint.x, 0, impactPoint.z) * forceAttack);

        }
    }

    public IEnumerator GotDamageCoroutine()
    {
        if (agent != null)
            agent.enabled = false;
        if (behaviorTree != null)
            behaviorTree.enabled = false;

        yield return new WaitForSeconds(0.5f);

        if (agent != null)
            agent.enabled = true;
        if (behaviorTree != null)
            behaviorTree.enabled = true;
    }
}
