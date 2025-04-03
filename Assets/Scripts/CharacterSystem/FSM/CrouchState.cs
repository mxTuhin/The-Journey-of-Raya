using UnityEngine;

public class CrouchState : MoveState
{
    void Update()
    {
        // TakeInput();
        
        JumpAndGravity();
        GroundedCheck();
        Move();

        if (!stateManager.IsCrouching)
        {
            // _animController.CrossFadeInFixedTime(AnimController.BaseIdle, 0.05f);
            stateManager.ChangeState(stateManager.idleState);
        }
        
        if (stateManager.IsLadderClimbing())
            stateManager.ChangeState(stateManager.climbState);

        if ((stateManager.IsLightAttack || stateManager.IsHeavyAttack))
        {
            stateManager.AttackType = stateManager.IsLightAttack ? AttackType.Light : AttackType.Heavy;
            stateManager.ChangeState(stateManager.attackState);
        }
            
        
        // if (stateManager.IsMoving())
        //     stateManager.ChangeState(stateManager.walkState);

        if (stateManager.IsDead())
            stateManager.ChangeState(stateManager.deadState);
    }
    
    private void Move() {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (stateManager.GetInput().move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = stateManager.GetInput().analogMovement ? stateManager.GetInput().move.magnitude : 1f;
            

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(stateManager.GetInput().move.x, 0.0f, stateManager.GetInput().move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (stateManager.GetInput().move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(stateManager.GetController().transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                stateManager.GetController().transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                
                // //PEAK: NOT REQUIRED IN CROUCH MODE AS SPEED IS SLOW
                // if ((Mathf.Abs(_targetRotation - rotation) <= 1 || Mathf.Abs(_targetRotation - rotation) >=359))
                // {
                //     // Mathf.Lerp(_speed / 1.1f, _speed, Time.deltaTime * SpeedChangeRate);
                // }
                // else
                // {
                //     if(stateManager.IsSprinting())
                //         _speed /= 1.1f;
                // }
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            //PEAK: NOT REQUIRED IN CROUCH MODE AS SPEED IS SLOW
            // if (!Grounded && stateManager.IsSprinting())
            // {
            //     _speed/=1.2f;
            // }
            
            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animController.SetFloat(AnimController.AnimIDSpeed, _animationBlend);
                _animController.SetFloat(AnimController.AnimIDMotionSpeed, inputMagnitude);
            }
    }

    
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Crouch State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Crouch State");
    }

    #region AnimationEvents

    public override void ResetAttack()
    {
        Debug.Log("Crouch Reset");
        stateManager.IsCrouching = false;
        stateManager.IsEvading = false;
    }

    #endregion
}