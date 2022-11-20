using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskDescription("Controlla se c'è una moneta a portata")]
[TaskCategory("Movement")]
[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WithinDistanceIcon.png")]
public class CanSeeComponent : Conditional
{

    [Tooltip("If using the object layer mask, specifies the maximum number of colliders that the physics cast can collide with")]
    public int maxCollisionCount = 10;
    [Tooltip("The distance that the object needs to be within")]
    public SharedFloat magnitude = 5;

    private List<GameObject> objectList;
    private Collider[] overlapColliders;
    public SharedGameObject objectFound;

    public string componentToSearch;

    public override void OnStart()
    {
        overlapColliders = new Collider[maxCollisionCount];
        objectList = new List<GameObject>();
    }

    // returns success if any object is within distance of the current object. Otherwise it will return failure
    public override TaskStatus OnUpdate()
    {
        if (objectFound.Value != null)
        {
            return TaskStatus.Success;
        }
        objectList.Clear();
        int count = Physics.OverlapSphereNonAlloc(transform.position, magnitude.Value, overlapColliders);
        for (int i = 0; i < count; ++i)
        {

            if (overlapColliders[i].gameObject.GetComponent(Type.GetType(componentToSearch)) != null)
            {
                objectList.Add(overlapColliders[i].gameObject);
            }

            if (objectList.Count > 0)
            {
                GameObject oggettoPiuVicino = objectList[0];
                foreach (GameObject oggettoAttuale in objectList)
                {
                    if ((oggettoPiuVicino.transform.position - transform.position).magnitude > (oggettoAttuale.transform.position - transform.position).magnitude)
                    {
                        oggettoPiuVicino = oggettoAttuale;
                    }
                }
                objectFound.Value = oggettoPiuVicino;
                return TaskStatus.Success;
            }
        }

        return TaskStatus.Failure;
    }

    public override void OnReset()
    {

    }

    // Draw the seeing radius
    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Owner == null || magnitude == null)
        {
            return;
        }
        var oldColor = UnityEditor.Handles.color;
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(Owner.transform.position, Owner.transform.up, magnitude.Value);
        UnityEditor.Handles.color = oldColor;
#endif
    }
}
