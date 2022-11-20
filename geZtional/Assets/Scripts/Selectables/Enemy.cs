using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
public class Enemy : MonoBehaviour
{
    BehaviorTree _behaviorTree;

    private void Awake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }

    public void Initialize()
    {

    }
}
