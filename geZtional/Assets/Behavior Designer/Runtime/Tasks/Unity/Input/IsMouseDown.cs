using UnityEngine;
using UnityEngine.InputSystem;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
    [TaskCategory("Unity/Input")]
    [TaskDescription("Returns success when the specified mouse button is pressed.")]
    
    public class IsMouseDown : Conditional
    {
        [Tooltip("The button index")]
        public SharedInt buttonIndex;
        public override TaskStatus OnUpdate()
        {
            bool clicked = GameManager.Instance.inputActions.Player.MouseRight.phase == InputActionPhase.Performed;


            return clicked == true ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            buttonIndex = 0;
        }
    }
}