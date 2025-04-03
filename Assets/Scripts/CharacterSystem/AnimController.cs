using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class AnimController
{
    
    //NOTE Universal
    public static readonly int MovementBlend = Animator.StringToHash("MovementBlend");
    public static readonly int CrouchIdle = Animator.StringToHash("CrouchIdle");
    public static readonly int BaseIdle = Animator.StringToHash("BaseIdle");
    
    //NOTE: Anim Hash
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Jump = Animator.StringToHash("Jump");
    
    public static readonly int Evade = Animator.StringToHash("Evade");
    
    // animation IDs
    public static readonly int AnimIDSpeed = Animator.StringToHash("Speed");
    public static readonly int AnimIDGrounded = Animator.StringToHash("Grounded");
    public static readonly int AnimIDJump = Animator.StringToHash("Jump");
    public static readonly int AnimIDFreeFall = Animator.StringToHash("FreeFall");
    public static readonly int AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    
    //Combat System
    public static readonly int AnimPunch = Animator.StringToHash("punch");
    public static readonly int AnimKick = Animator.StringToHash("kick");
    public static readonly int AnimMmaKick = Animator.StringToHash("mmakick");
    public static readonly int AnimHeavyAttack1 = Animator.StringToHash("heavyAttack1");
    public static readonly int AnimHeavyAttack2 = Animator.StringToHash("heavyAttack2");
    
    
    private Animator animator;
    
    public AnimController(Animator animator)
    {
        this.animator = animator;
    }
    
    //NOTE: Update the Animator Properties
    public void UpdateAnimator(RuntimeAnimatorController animator)
    {
        float lastBlendValue = this.animator.GetFloat(AnimIDSpeed);
        this.animator.runtimeAnimatorController = animator;
        this.animator.SetFloat(AnimIDSpeed, lastBlendValue);
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
    
    public void SetTrigger(int id)
    {
        animator.SetTrigger(id);
    }
    
    public void CrossFadeInFixedTime(int id, float transitionDuration)
    {
        animator.CrossFadeInFixedTime(id, transitionDuration);
    }
    
    public Animator GetAnimator()
    {
        return animator;
    }
}