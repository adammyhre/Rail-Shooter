using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

namespace RailShooter {
    public class PlayerController : ValidatedMonoBehaviour {
        [SerializeField, Self] InputReader input;
        
        [SerializeField] Transform followTarget;
        [SerializeField] Transform playerModel;
        [SerializeField] float followDistance = 2f;
        [SerializeField] Vector2 movementLimit = new Vector2(2f, 2f);
        [SerializeField] float movementRange = 5f;
        [SerializeField] float movementSpeed = 10f;
        [SerializeField] float smoothTime = 0.2f;

        [SerializeField] float maxRoll = 15f;
        [SerializeField] float rollSpeed = 2f;
        [SerializeField] float rollDuration = 1f;
        
        Vector3 velocity;
        float roll;

        void Awake() {
            input.LeftTap += OnLeftTap;
            input.RightTap += OnRightTap;
        }

        void Update() {
            // Calculate the target position based on the follow distance and the target's position
            Vector3 targetPos = followTarget.position + followTarget.forward * -followDistance;
            
            // Apply smooth damp to the player's position
            Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            
            // Calculate the new local position
            Vector3 localPos = transform.InverseTransformPoint(smoothedPos);
            localPos.x += input.Move.x * movementSpeed * Time.deltaTime * movementRange;
            localPos.y += input.Move.y * movementSpeed * Time.deltaTime * movementRange;
            
            // Clamp the local position
            localPos.x = Mathf.Clamp(localPos.x, -movementLimit.x, movementLimit.x);
            localPos.y = Mathf.Clamp(localPos.y, -movementLimit.y, movementLimit.y);
            
            // Update the player's position
            transform.position = transform.TransformPoint(localPos);
            
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
