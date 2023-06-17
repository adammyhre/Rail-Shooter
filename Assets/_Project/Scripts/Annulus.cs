using UnityEngine;

namespace RailShooter {
    [System.Serializable]
    public class Annulus {
        public float distance; // Distance of the annulus center from the spawner
        public float innerRadius; // Inner radius of the annulus
        public float outerRadius; // Outer radius of the annulus

        public Vector3 GetRandomPoint() {
            // Random angle between 0 and 180 degrees
            float angle = Random.Range(0f, Mathf.PI); // full circle would be 2PI
            
            // Random radius between inner and outer radius
            float radius = Random.Range(innerRadius, outerRadius);
            
            // Calculate the x and y coordinates of the point
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            
            // Return the point
            return new Vector3(x, y, distance);
        }
    }
}