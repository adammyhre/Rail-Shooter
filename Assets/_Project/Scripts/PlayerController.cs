using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

namespace RailShooter {
    public class PlayerController : ValidatedMonoBehaviour {
        [SerializeField, Self] InputReader input;
        
        [SerializeField] Transform followTarget;
        [SerializeField] Transform aimTarget;
        
        [SerializeField] Transform playerModel;
        [SerializeField] float followDistance = 2f;
        [SerializeField] Vector2 movementLimit = new Vector2(2f, 2f);
        [SerializeField] float movementSpeed = 10f;
        [SerializeField] float smoothTime = 0.2f;

        [SerializeField] float maxRoll = 15f;
        [SerializeField] float rollSpeed = 2f;
        [SerializeField] float rollDuration = 1f;

        [SerializeField] Transform modelParent;
        [SerializeField] float rotationSpeed = 5f;
        
        Vector3 velocity;
        float roll;

        void Awake() {
            input.LeftTap += OnLeftTap;
            input.RightTap += OnRightTap;
        }

        void Update() {
            HandlePosition();
            HandleRoll();
            HandleRotation();
        }

        void HandleRotation() {
            // Determine direction to the target
            Vector3 direction = aimTarget.position - transform.position;
            
            // Calculate the rotation required to look at the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            modelParent.rotation = Quaternion.Lerp(modelParent.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        void HandlePosition() {
            // Calculate the target position based on the follow distance and the target's position
            Vector3 targetPos = followTarget.position + followTarget.forward * -followDistance;

            // Apply smooth damp to the player's position
            Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

            // Calculate the new local position
            Vector3 localPos = transform.InverseTransformPoint(smoothedPos);
            localPos.x += input.Move.x * movementSpeed * Time.deltaTime;
            localPos.y += input.Move.y * movementSpeed * Time.deltaTime;

            // Clamp the local position
            localPos.x = Mathf.Clamp(localPos.x, -movementLimit.x, movementLimit.x);
            localPos.y = Mathf.Clamp(localPos.y, -movementLimit.y, movementLimit.y);

            // Update the player's position
            transform.position = transform.TransformPoint(localPos);
        }

        void HandleRoll() {
            // Match the player's rotation to the follow target's rotation
            transform.rotation = followTarget.rotation;

            // Match the roll based on player input
            roll = Mathf.Lerp(roll, input.Move.x * maxRoll, Time.deltaTime * rollSpeed);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, roll);
        }

        void OnLeftTap() => BarrelRoll();
        void OnRightTap() => BarrelRoll(1);

        void BarrelRoll(int direction = -1) {
            if (!DOTween.IsTweening(playerModel)) {
                playerModel.DOLocalRotate(new Vector3(playerModel.localEulerAngles.x, playerModel.localEulerAngles.y, 360 * direction), 
                    rollDuration, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic);
            }
        }

        void OnDestroy() {
            input.LeftTap -= OnLeftTap;
            input.RightTap -= OnRightTap;
        }
    }
}
