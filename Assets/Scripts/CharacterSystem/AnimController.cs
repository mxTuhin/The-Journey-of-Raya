using System;
using UnityEditor.Animations;
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
    
    // animation IDs
    public static readonly int AnimIDSpeed = Animator.StringToHash("Speed");
    public static readonly int AnimIDGrounded = Animator.StringToHash("Grounded");
    public static readonly int AnimIDJump = Animator.StringToHash("Jump");
    public static readonly int AnimIDFreeFall = Animator.StringToHash("FreeFall");
    public static readonly int AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    
    
    private Animator animator;
    
    public AnimController(Animator animator)
    {
        this.animator = animator;
    }
    
    //NOTE: Update the Animator Properties
    public void UpdateAnimator(RuntimeAnimatorController animator, Avatar avatar)
    {
        this.animator.runtimeAnimatorController = animator;
        this.animator.avatar = avatar;
    }
    
    //NOTE: Update the Animator Reference
    public void UpdateAnimator(Animator animator)
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
    
    public void SetFloat(int id, float value)
    {
        animator.SetFloat(id, value);
    }
    
    public void SetBool(int id, bool value)
    {
        animator.SetBool(id, value);
    }
}