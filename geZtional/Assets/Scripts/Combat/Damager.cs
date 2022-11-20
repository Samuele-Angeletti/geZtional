using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float strength;
    [SerializeField] EDamageableType target;

    private void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        if(damageable != null)
        {
            if (damageable.DamageableType == target)
            {

                damageable.Damage(damage, strength, collision.contacts[Random.Range(0, collision.contactCount)].point);
                
            }
        }
    }
}
