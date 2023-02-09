using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [HideInInspector] public float Damage;
    [SerializeField] LayerMask targetMask;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Damageable>(out var damageable))
        {
            if (targetMask.Contains(damageable.gameObject.layer))
            {
                damageable.Damage(Damage);
            }
        }
    }
}
