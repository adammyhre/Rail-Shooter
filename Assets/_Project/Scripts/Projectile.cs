using UnityEngine;

namespace RailShooter {
    public class Projectile : MonoBehaviour {
        [SerializeField] float speed = 10f;

        void Update() {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}