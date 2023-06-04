using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RailShooter {
    [RequireComponent(typeof(PlayerInput))]
    public class InputReader : ValidatedMonoBehaviour {
        [SerializeField, Self] PlayerInput playerInput;
        [SerializeField] float doubleTapTime = 0.5f;
        
        InputAction moveAction;

        float lastMoveTime;
        float lastMoveDirection;
        
        public event Action LeftTap;
        public event Action RightTap;
        
        public Vector2 Move => moveAction.ReadValue<Vector2>();

        void Awake() {
            moveAction = playerInput.actions["Move"];
        }

        void OnEnable() {
            moveAction.performed += OnMovePerformed;
        }
        
        void OnDisable() {
            moveAction.performed -= OnMovePerformed;
        }

        void OnMovePerformed(InputAction.CallbackContext ctx) {
            float currentDirection = Move.x;
            
            if (Time.time - lastMoveTime < doubleTapTime && currentDirection == lastMoveDirection) {
                if (currentDirection < 0) {
                    LeftTap?.Invoke();
                } else if (currentDirection > 0) {
                    RightTap?.Invoke();
                }
            }
            
            lastMoveTime = Time.time;
            lastMoveDirection = currentDirection;
        }
    }
}