using UnityEngine;

public class DeadState : CharacterState
{
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Dead State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Dead State");
    }
}