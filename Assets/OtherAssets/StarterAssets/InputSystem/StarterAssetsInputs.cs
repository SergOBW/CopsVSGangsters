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

		[Header("Movement Settings")]
		public bool analogMovement;
		
		public bool loot;
		public bool shooting;
		public bool isReloading;
		public bool aiming;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			LookInput(value.Get<Vector2>());
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void OnMove(InputAction.CallbackContext callbackContext)
		{
			MoveInput(callbackContext.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext callbackContext)
		{
			LookInput(callbackContext.ReadValue<Vector2>());
		}
		
		public void OnReload(InputAction.CallbackContext callbackContext)
		{
			ReloadInput(callbackContext.performed);
		}
		
		public void OnJump(InputAction.CallbackContext callbackContext)
		{
			JumpInput(callbackContext.performed);
		}

		public void OnSprint(InputAction.CallbackContext callbackContext)
		{
			SprintInput(callbackContext.performed);
		}
		
		public void OnLootInput(InputAction.CallbackContext callbackContext)
		{
			LootInput(callbackContext.performed);
		}

		public void OnShootInput(InputAction.CallbackContext callbackContext)
		{
			ShootInput(callbackContext.performed);
		}
		
		
		
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void ShootInput( bool isPreformed)
		{
			shooting = isPreformed;
		}
		
		public void LootInput( bool isPreformed)
		{
			loot = isPreformed;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}


		public void ReloadInput(bool isPreformed)
		{
			isReloading = isPreformed;
		}
	}
	
}