using System;
using KBCore.Refs;
using UnityEngine;

namespace RailShooter {
    public class WeaponSystem : ValidatedMonoBehaviour {
        [SerializeField, Self] InputReader input;

        [SerializeField] Transform targetPoint;
        [SerializeField] float targetDistance = 50f;
        [SerializeField] float smoothTime = 0.2f;
        [SerializeField] Vector2 aimLimit = new Vector2(50f, 20f);
        [SerializeField] float aimSpeed = 10f;
        [SerializeField] float aimReturnSpeed = 0.2f;
        
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform firePoint;
        
        Vector3 velocity;
        Vector2 aimOffset;

        void Awake() {
            input.Fire += OnFire;
        }

        void Start() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update() {
            // Set the targetPosition ahead of the player's local position by the target distance
            Vector3 targetPosition = transform.position + transform.forward * targetDistance;
            Vector3 localPos = transform.InverseTransformPoint(targetPosition);
            
            // If there is Aim input
            if (input.Aim != Vector2.zero) {
                // Add the aim input to the aim offset
                aimOffset += input.Aim * aimSpeed * Time.deltaTime;
                
                // Clamp the aim offset
                aimOffset.x = Mathf.Clamp(aimOffset.x, -aimLimit.x, aimLimit.x);
                aimOffset.y = Mathf.Clamp(aimOffset.y, -aimLimit.y, aimLimit.y);
            }
            else {
                // Otherwise, return the aim offset to zero
                aimOffset = Vector2.Lerp(aimOffset, Vector2.zero, Time.deltaTime * aimReturnSpeed);
            }
            
            // Apply the aim offset to the local position
            localPos.x += aimOffset.x;
            localPos.y += aimOffset.y;
            
            var desiredPosition = transform.TransformPoint(localPos);
            
            // Smooth damp to the desired position
            targetPoint.position = Vector3.SmoothDamp(targetPoint.position, desiredPosition, ref velocity, smoothTime);
        }

        void OnFire() {
            var projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(targetPoint.position - firePoint.position));
            Destroy(projectile, 5f);
        }

        void OnDestroy() {
            input.Fire -= OnFire;
        }
    }
}