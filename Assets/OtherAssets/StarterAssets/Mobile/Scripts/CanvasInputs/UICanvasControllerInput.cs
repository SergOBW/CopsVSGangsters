using System;
using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        private void OnEnable()
        {
            starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        }

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLootInput(bool isPreformed)
        {
            starterAssetsInputs.LootInput(isPreformed);
        }
        
        public void VirtualReloadInput(bool isPreformed)
        {
            starterAssetsInputs.ReloadInput(isPreformed);
        }

        public void VirtualShootInput(bool isPreformed)
        {
            starterAssetsInputs.ShootInput(isPreformed);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }
        
    }

}
