using System.Collections;
using UnityEngine;

public class CardioState : CharacterState
{
    [Space] [SerializeField] protected CharacterController _controller;
    
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    
    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;
    
    // timeout deltatime
    protected float _jumpTimeoutDelta;
    protected float _fallTimeoutDelta;
    
    protected float _verticalVelocity;
    protected float _terminalVelocity = 53.0f;

    protected bool _evadeCallInit;

    protected bool _crouchEvade;

    protected void VerticalMove()
    {
        // move the player
        _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }
    
    protected void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animController.SetBool(AnimController.AnimIDJump, false);
                    _animController.SetBool(AnimController.AnimIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (stateManager.GetInput().jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animController.SetBool(AnimController.AnimIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animController.SetBool(AnimController.AnimIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                stateManager.GetInput().jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    
    protected void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (_hasAnimator)
        {
            _animController.SetBool(AnimController.AnimIDGrounded, Grounded);
        }
    }
    
    protected virtual void TakeInput()
    {
        if(stateManager.IsCrouching && stateManager.GetInput().move.magnitude >0 && !_evadeCallInit)
            Evade(timer: 0.45f, setCrouchFlag: false);
    }
    
    
    protected virtual void Evade(float timer, bool setCrouchFlag)
    {
        StartCoroutine(HandleRootMotion(timer, setCrouchFlag));
        _animController.SetTrigger(AnimController.Evade);
    }
    
    private IEnumerator HandleRootMotion(float timer, bool setCrouchFlag)
    {
        if(setCrouchFlag)
            stateManager.IsCrouching = true;
        stateManager.IsEvading = true;
        _evadeCallInit = true;
        _animController.GetAnimator().applyRootMotion = true;
        yield return new WaitForSeconds(timer);
        _animController.GetAnimator().applyRootMotion = false;
        stateManager.IsCrouching = setCrouchFlag;
        // yield return new WaitForSeconds(0.25f);
        // stateManager.IsCrouching = setCrouchFlag;
        // yield return new WaitForSeconds(0.15f);
        _evadeCallInit = false;
        stateManager.IsEvading = false;
    }
}
