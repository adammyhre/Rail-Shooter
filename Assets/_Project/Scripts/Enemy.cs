using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter {
    public class Enemy : ValidatedMonoBehaviour {
        [SerializeField, Self] SplineAnimate splineAnimate;
        [SerializeField] GameObject explosionPrefab;
        
        SplineContainer flightPath;
        
        public SplineContainer FlightPath {
            get => flightPath;
            set => flightPath = value;
        }

        void Update() {
            if (splineAnimate != null && splineAnimate.ElapsedTime >= splineAnimate.Duration) {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter(Collider other) {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosion, 5f);
        }

        void OnDestroy() {
            if (flightPath != null) {
                Destroy(flightPath.gameObject);
            }
        }
    }
}