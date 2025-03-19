using System;
using UnityEngine;

public class AnimController
{
    //NOTE: Anim Hash
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Jump = Animator.StringToHash("Jump");
    
    
    private Animator animator;
    
    public AnimController(Animator animator)
    {
        this.animator = animator;
    }
    
    public void SetMove()
    {
        animator.CrossFadeInFixedTime(Move, 0);
    }
    
    
    public void SetIdle()
    {
        animator.CrossFadeInFixedTime(Idle, 0);
    }
    
    public void SetAttack()
    {
        animator.CrossFadeInFixedTime(Attack, 0);
    }
    
    public void SetDie()
    {
        animator.CrossFadeInFixedTime(Die, 0);
    }
    
    public void SetHit()
    {
        animator.CrossFadeInFixedTime(Hit, 0);
    }
    
    public void SetJump()
    {
        animator.CrossFadeInFixedTime(Jump, 0);
    }
}