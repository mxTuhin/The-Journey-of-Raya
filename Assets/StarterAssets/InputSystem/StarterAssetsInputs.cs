using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool lightAttack;
		public bool heavyAttack;
		public bool crouch;
		public float crouchValue;
		public bool InSetCrouch;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		
		public void OnLightAttack(InputValue value)
		{
			LightAttackInput(value.isPressed);
		}
		public void OnHeavyAttack(InputValue value)
		{
			HeavyAttackInput(value.isPressed);
		}
		
		public void OnCrouch(InputValue value)
		{
			CrouchValueInput(value.Get<float>());
			CrouchInput(value.isPressed);
		}
		
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		public void LightAttackInput(bool newLightAttackState)
		{
			lightAttack = newLightAttackState;
		}
		
		public void HeavyAttackInput(bool newHeavyAttackState)
		{
			heavyAttack = newHeavyAttackState;
		}
		
		public void CrouchInput(bool newCrouchState)
		{
			if(InSetCrouch)
				return;
			if (newCrouchState)
			{
				if (crouch)
				{
					Debug.Log("In Set");
					InSetCrouch = true;
					Invoke(nameof(ResetInSetCrouch), 0.1f);
					// return;
				}
				
				Debug.Log("Crouch Set");
				crouch = !crouch;
			}
				
		}
		
		public void CrouchValueInput(float newCrouchValue)
		{
			crouchValue = newCrouchValue;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void ResetInSetCrouch()
		{
			InSetCrouch = false;
		}
	}
	
}