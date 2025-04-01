using UnityEngine;

public class LadderClimbState : CharacterState
{
    void Update()
    {
        if (!stateManager.IsLadderClimbing())
            stateManager.ChangeState(stateManager.idleState);

        if (stateManager.IsDead())
            stateManager.ChangeState(stateManager.deadState);
    }

    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Climb State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Climb State");
    }
}