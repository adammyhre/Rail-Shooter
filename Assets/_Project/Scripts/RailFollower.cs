using UnityEngine;

namespace RailShooter {
    public class RailFollower : MonoBehaviour {
        [SerializeField] Transform player;
        [SerializeField] Transform followTarget;
        [SerializeField] float followDistance = 22f;
        [SerializeField] float smoothTime = 0.2f;
        
        Vector3 velocity;

        void Update() {
            Vector3 targetPos = followTarget.position + followTarget.forward * -followDistance;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            
            // Set the camera's rotation to match the player's rotation
            transform.rotation = player.rotation;
        }
    }
}