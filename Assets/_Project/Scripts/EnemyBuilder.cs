using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter {
    public class EnemyBuilder {
        Transform flightPathParent;
        Enemy enemyPrefab;
        SplineContainer flightPath;
        SplineAnimate.LoopMode loopMode = SplineAnimate.LoopMode.Once;
        
        public EnemyBuilder withFlightPathParent(Transform flightPathParent) {
            this.flightPathParent = flightPathParent;
            return this;
        }
        
        public EnemyBuilder withPrefab(Enemy enemyPrefab) {
            this.enemyPrefab = enemyPrefab;
            return this;
        }
        
        public EnemyBuilder withFlightPath(SplineContainer flightPath) {
            this.flightPath = flightPath;
            return this;
        }
        
        public EnemyBuilder withLoopMode(SplineAnimate.LoopMode loopMode) {
            this.loopMode = loopMode;
            return this;
        }
        
        public Enemy build(Transform enemyParent) {
            var enemy = Object.Instantiate(enemyPrefab, enemyParent);
            
            enemy.FlightPath = flightPath;

            if (flightPath != null) {
                var splineAnimate = enemy.GetComponent<SplineAnimate>();
                splineAnimate.Container = flightPath;
                splineAnimate.Loop = loopMode;
                splineAnimate.ElapsedTime = 0f;
            }
            
            if (flightPathParent != null && flightPath != null) {
                Transform transform;
                (transform = flightPath.transform).SetParent(flightPathParent);
                // reset local position and rotation
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            
            return enemy;
        }
        
        
        
        
        
    }
}