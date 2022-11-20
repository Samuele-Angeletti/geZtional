using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDestinationAction : Conditional
{
    public SharedBool newDestination;
    public SharedVector3 newDestinationPosition;
    public SharedVector3 targetReached;
    private Vector3 destinationPos;
    public override void OnAwake()
    {
        base.OnAwake();
    }
    public override void OnStart()
    {
        base.OnStart();
    }
    public override TaskStatus OnUpdate()
    {
        if (targetReached.Value == Vector3.one)
            return TaskStatus.Success;

        if(newDestination.Value)
        {
            targetReached.Value = Vector3.one;
            destinationPos = newDestinationPosition.Value;
            newDestination.Value = false;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
