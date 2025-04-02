using UnityEditor.Animations;
using UnityEngine;

public abstract class CharacterState : MonoBehaviour
{
    protected FiniteStateManager stateManager;
    
    
    [SerializeField] private AnimatorController animator;
    
    protected Camera _mainCamera;
    protected bool _hasAnimator;
    protected AnimController _animController;
    
    protected bool IsCurrentDeviceMouse
    {
        get
        {
            #if ENABLE_INPUT_SYSTEM
            return stateManager.GetPlayerInput().currentControlScheme == "KeyboardMouse";
            #else
				return false;
            #endif
        }
    }

    public virtual void Initialize(FiniteStateManager fsm)
    {
        this.stateManager = fsm;
        _mainCamera = Camera.main;
        _hasAnimator = stateManager.GetController().AnimController != null;
        if (_hasAnimator)
        {
            _animController = stateManager.GetController().AnimController;
        }
    }

    public virtual void EnterState()
    {
        stateManager.GetController().AnimController.UpdateAnimator(animator);
        gameObject.SetActive(true);
    }

    public virtual void ExitState()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void ChangeTarget(EnemySystem target) { }
    public virtual void GetClose() {}
    public virtual void PerformAttack() {}
    public virtual void ResetAttack() {}
}