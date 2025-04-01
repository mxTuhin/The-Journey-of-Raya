using UnityEngine;

public class MovingState : CardioState
{
    [Header("Movement Parameters")]
    
    [SerializeField] protected float MoveSpeed = 2f;
    [SerializeField] protected float SprintSpeed = 5f;
    [SerializeField] protected float CrouchSpeed = 1.5f;
    
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    
    [Tooltip("Acceleration and deceleration")] public float SpeedChangeRate = 10.0f;
    
    protected float _speed;
    protected float _animationBlend;
    protected float _targetRotation = 0.0f;
    protected float _rotationVelocity;
    
    
}
