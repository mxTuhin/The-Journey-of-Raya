using UnityEngine;

public class IdleState : CardioState
{
    void Update()
    {
        StateControl();
        
        JumpAndGravity();
        GroundedCheck();
        VerticalMove();
        
    }
    
    private void StateControl()
    {
        if (stateManager.IsMoving() && !stateManager.IsCrouching() && !stateManager.IsLadderClimbing() && !stateManager.IsAttacking() && !stateManager.IsJumping())
            stateManager.ChangeState(stateManager.moveState);
        
        if (stateManager.IsDead())
            stateManager.ChangeState(stateManager.deadState);
        
        if (stateManager.IsAttacking())
            stateManager.ChangeState(stateManager.attackState);

        if (stateManager.IsCrouching())
            stateManager.ChangeState(stateManager.crouchState);

        if (stateManager.IsLadderClimbing())
            stateManager.ChangeState(stateManager.climbState);
    }
    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Idle State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Idle State");
    }
}