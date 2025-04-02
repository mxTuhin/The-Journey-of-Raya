using UnityEditor.Animations;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private AnimController _animController;
    
    [Space]
    [SerializeField] protected HealthController healthController;

    protected bool _isMoving;
    protected bool _isJumping;
    protected bool _isSprinting;
    protected bool _isAttacking;
    protected bool _isCrouching;
    protected bool _isLadderClimbing;
    
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

    public virtual bool IsJumping
    {
        get => _isJumping;
        set => _isJumping = value;
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

    public virtual bool IsLadderClimbing
    {
        get => _isLadderClimbing;
        set => _isLadderClimbing = value;
    }
}
