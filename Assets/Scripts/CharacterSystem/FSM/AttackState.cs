using UnityEngine;

public class AttackState : CharacterState
{
    private bool attackFinished = false;

    public override void EnterState()
    {
        base.EnterState();
        attackFinished = false;
        //NOTE: Send Animation Call
        Invoke(nameof(FinishAttack), 0.8f); // Adjust timing for your animation
        Debug.Log("Entering Attack State");
    }

    void Update()
    {
        if (stateManager.IsDead())
            stateManager.ChangeState(stateManager.deadState);

        if (attackFinished)
        {
            if (stateManager.IsMoving())
                stateManager.ChangeState(stateManager.moveState);
            else
                stateManager.ChangeState(stateManager.idleState);
        }
    }

    private void FinishAttack()
    {
        attackFinished = true;
    }


    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Attack State");
    }
}