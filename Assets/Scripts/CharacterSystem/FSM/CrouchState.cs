using UnityEngine;

public class CrouchState : CharacterState
{
    void Update()
    {
        if (!stateManager.IsCrouching())
            stateManager.ChangeState(stateManager.idleState);

        // if (stateManager.IsMoving())
        //     stateManager.ChangeState(stateManager.walkState);

        if (stateManager.IsDead())
            stateManager.ChangeState(stateManager.deadState);
    }

    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Crouch State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Crouch State");
    }
}