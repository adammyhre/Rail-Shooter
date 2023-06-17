using Shapes;
using UnityEngine;
using Utilities;

namespace RailShooter {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] Annulus[] annuli;
        [SerializeField] Enemy enemyPrefab;
        [SerializeField] float spawnInterval = 5f;
        
        [SerializeField] Transform enemyParent;
        [SerializeField] Transform flightPathParent;
        
        float spawnTimer;

        void Update() {
            if (spawnTimer > spawnInterval) {
                spawnTimer = 0f;
                SpawnEnemy();
            }
            spawnTimer += Time.deltaTime;
        }

        void SpawnEnemy() {
            var flightPath = FlightPathFactory.GenerateFlightPath(annuli);
            EnemyFactory.GenerateEnemy(enemyPrefab, flightPath, enemyParent, flightPathParent);
        }
    }
}