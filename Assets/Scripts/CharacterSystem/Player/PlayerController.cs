using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : Humanoid
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private FiniteStateManager finiteStateManager;
    
    [Space]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    
        
#if ENABLE_INPUT_SYSTEM 
    private PlayerInput _playerInput;
#endif
    private StarterAssetsInputs _input;
    
    [Space]
    [Header("Custom")]
    private bool canMove;
    [Range(0f, 1f)]
    public float directMoveBlend = 0f;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        #if ENABLE_INPUT_SYSTEM 
        _playerInput = GetComponent<PlayerInput>();
        #else
			            Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
        #endif
        
        //NOTE: Init Health
        healthController.Init(100);
    }
    
    public FiniteStateManager GetStateManager()
    {
        return finiteStateManager;
    }
    
    public PlayerInput GetPlayerInput()
    {
        return _playerInput;
    }
    
    public StarterAssetsInputs GetInput()
    {
        return _input;
    }

    public override bool IsMoving => _input.move.magnitude > 0 && !HasJumped && !IsSprinting;
    
    public override bool IsLightAttack
    {
        get => _input.lightAttack;
        set => _input.lightAttack = value;
    }

    public override bool IsHeavyAttack
    {
        get => _input.heavyAttack;
        set => _input.heavyAttack = value;
    }

    public override bool CanMove
    {
        get => CanMove;
        set => canMove = value;
    }

    public override bool HasJumped => _input.jump;
    
    public override bool IsSprinting => _input.sprint;
   

    #region AnimEvents
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }
    
    #endregion

    #region AttackAnimEvents

    public void GetClose()
    {
        finiteStateManager.attackState.GetClose();
    }
    
    public void PerformAttack()
    {
        finiteStateManager.attackState.PerformAttack();
    }
    
    public void ResetAttack()
    {
        finiteStateManager.attackState.ResetAttack();
    }

    #endregion
}
