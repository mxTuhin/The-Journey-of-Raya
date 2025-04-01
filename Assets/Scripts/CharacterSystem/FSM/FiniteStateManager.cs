using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FiniteStateManager : MonoBehaviour
{
    [SerializeField] private Humanoid characterObject;
    public Humanoid GetCharacterObject() => characterObject;
    
    [Header("State References")]
    public CharacterState idleState;
    public CharacterState moveState;
    public CharacterState crouchState;
    public CharacterState attackState;
    public CharacterState climbState;
    public CharacterState deadState;

    private CharacterState currentState;

    
#if ENABLE_INPUT_SYSTEM 
    private PlayerInput _playerInput;
#endif
    private StarterAssetsInputs _input;

    void Start()
    {
        
        _input = GetComponent<StarterAssetsInputs>();
        #if ENABLE_INPUT_SYSTEM 
        _playerInput = GetComponent<PlayerInput>();
        #else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
        #endif
        
        idleState.Initialize(this);
        moveState?.Initialize(this);
        crouchState?.Initialize(this);
        attackState?.Initialize(this);
        climbState?.Initialize(this);
        deadState?.Initialize(this);
        
        //NOTE: Disable all states initially
        idleState.enabled = false;
        if (moveState != null) moveState.enabled = false;
        if (crouchState != null) crouchState.enabled = false;
        if (attackState != null) attackState.enabled = false;
        if (climbState != null) climbState.enabled = false;
        if (deadState != null) deadState.enabled = false;

        ChangeState(idleState);
        
        
    }

    public void ChangeState(CharacterState newState)
    {
        if (currentState == newState) return;

        if (currentState != null)
        {
            currentState.ExitState();
            currentState.enabled = false; //NOTE: Ensure the previous state is disabled
        }

        currentState = newState;
        currentState.enabled = true; //NOTE: Enable the new state
        currentState.EnterState();
    }


    public bool IsMoving() => _input.move.magnitude > 0.1f && !IsJumping() && !IsSprinting();
    
    public bool IsJumping() => _input.jump;
    
    public bool IsDead() => false;
    
    public bool IsSprinting() => _input.sprint;
    public bool IsAttacking() => false;
    public bool IsCrouching() => false;
    public bool IsLadderClimbing() => false;
    
    public StarterAssetsInputs GetInput()
    {
        return _input;
    }
    
    public PlayerInput GetPlayerInput()
    {
        return _playerInput;
    }
}

public enum CharacterType
{
    Player,
    Enemy,
}