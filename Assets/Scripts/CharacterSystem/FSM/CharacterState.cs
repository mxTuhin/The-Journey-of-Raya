using System.Collections;
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
        _hasAnimator = stateManager.GetController().GetAnimController != null;
        if (_hasAnimator)
        {
            _animController = stateManager.GetController().GetAnimController;
        }
    }

    public virtual void EnterState()
    {
        _animController.GetAnimator().applyRootMotion = false;
        stateManager.GetController().GetAnimController.UpdateAnimator(animator);
        // StartCoroutine(ChangeAnimatorController(animator));
        // gameObject.SetActive(true);
    }
    
    protected IEnumerator ChangeAnimatorController(RuntimeAnimatorController animator)
    {
        yield return new WaitForSeconds(0.1f);
        stateManager.GetController().GetAnimController.UpdateAnimator(animator);
    }

    public virtual void ExitState()
    {
        // gameObject.SetActive(false);
    }
    
    public virtual void ChangeTarget(EnemySystem target) { }

    #region AnimationaEvents

    public virtual void GetClose() {}
    public virtual void PerformAttack() {}
    public virtual void ResetAttack() {}

    #endregion
    

    public virtual bool IsGrounded()
    {
        return true;
    }
}