using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FiniteStateManager : MonoBehaviour
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private PlayerController playerObject;
    [SerializeField] private EnemySystem enemySystem;
    public Humanoid GetController()
    {
        return characterType == CharacterType.Player ? playerObject : enemySystem;
    }

    [Header("State References")]
    public CharacterState idleState;
    public CharacterState moveState;
    public CharacterState crouchState;
    public CharacterState attackState;
    public CharacterState climbState;
    public CharacterState deadState;

    private CharacterState currentState;

    void Start()
    {
        
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


    public bool IsMoving() => GetController().IsMoving ;
    
    public bool IsJumping() => GetController().IsJumping;
    
    public bool IsDead() => GetController().IsDead();
    
    public bool IsSprinting() => GetController().IsInvoking();
    public bool IsAttacking() => GetController().IsAttacking;
    public bool IsCrouching() => GetController().IsCrouching;
    public bool IsLadderClimbing() => GetController().IsLadderClimbing;
    
    public StarterAssetsInputs GetInput()
    {
        return playerObject.GetInput();
    }
    
    public PlayerInput GetPlayerInput()
    {
        return playerObject.GetPlayerInput();
    }
    
}

public enum CharacterType
{
    Player,
    Enemy,
}