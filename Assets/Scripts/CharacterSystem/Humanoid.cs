using UnityEditor.Animations;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AnimController _animController;
    
    [Space]
    [SerializeField] protected HealthController healthController;

    protected bool _isMoving;
    protected bool _canMove;
    protected bool _hasJumped;
    protected bool _isSprinting;
    protected bool _isAttacking;
    protected bool _isCrouching;
    protected bool _isEvading;
    protected bool _isLadderClimbing;
    protected bool isLightAttack;
    protected bool isHeavyAttack;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animController = new AnimController(animator);
    }
    
    public void UpdateAnimator(Animator animator)
    {
        _animController.UpdateAnimator(animator);
    }
    
    public HealthController GetHealthController()
    {
        return healthController;
    }
    
    public AnimController AnimController
    {
        get => _animController;
        set => _animController = value;
    }

    public virtual bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }
    
    public virtual bool CanMove
    {
        get => true;
        set => _canMove = value;
    }

    public virtual bool HasJumped
    {
        get => _hasJumped;
        set => _hasJumped = value;
    }

    public bool IsDead() => healthController.IsDead;

    public virtual bool IsSprinting
    {
        get => _isSprinting;
        set => _isSprinting = value;
    }

    public virtual bool IsAttacking
    {
        get => _isAttacking;
        set => _isAttacking = value;
    }

    public virtual bool IsCrouching
    {
        get => _isCrouching;
        set => _isCrouching = value;
    }
    
    public virtual bool IsEvading
    {
        get => _isEvading;
        set => _isEvading = value;
    }
    public virtual float GetCrouchValue()
    {
        return _isCrouching ? 1 : 0;
    }

    public virtual bool IsLadderClimbing
    {
        get => _isLadderClimbing;
        set => _isLadderClimbing = value;
    }
    
    public virtual bool IsLightAttack
    {
        get => isLightAttack;
        set => isLightAttack = value;
    }
    
    public virtual bool IsHeavyAttack
    {
        get => isHeavyAttack;
        set => isHeavyAttack = value;
    }
}
