using UnityEngine;

public class IdleState : CardioState
{
    void Update()
    {
        StateControl();
        
        TakeInput();
        
        JumpAndGravity();
        GroundedCheck();
        VerticalMove();
        
    }
    
    private void StateControl()
    {
        if ((stateManager.IsMoving() || stateManager.IsSprinting()) && !stateManager.IsCrouching && !stateManager.IsLadderClimbing() && !stateManager.HasJumped())
            stateManager.ChangeState(stateManager.moveState);
        
        if (stateManager.IsDead())
            stateManager.ChangeState(stateManager.deadState);
        
        if ((stateManager.IsLightAttack || stateManager.IsHeavyAttack))
            stateManager.ChangeState(stateManager.attackState);

        if (stateManager.IsCrouching && stateManager.GetInput().move.magnitude <= 0)
        {
            // _animController.CrossFadeInFixedTime(AnimController.CrouchIdle, 0.05f);
            stateManager.ChangeState(stateManager.crouchState);
        }
           

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
    
    public override void ResetAttack()
    {
        Debug.Log("Crouch Reset");
        stateManager.IsCrouching = false;
        stateManager.IsEvading = false;
    }
}