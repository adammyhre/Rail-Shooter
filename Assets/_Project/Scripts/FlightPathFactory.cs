using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter {
    public static class FlightPathFactory {
        public static SplineContainer GenerateFlightPath(Annulus[] annuli) {
            Vector3[] pathPoints = new Vector3[annuli.Length];

            for (int i = 0; i < annuli.Length; i++) {
                pathPoints[i] = annuli[i].GetRandomPoint();
            }
            
            return CreateFlightPath(pathPoints);
        }

        static SplineContainer CreateFlightPath(Vector3[] pathPoints) {
            GameObject flightPath = new GameObject("Flight Path");
            
            var container = flightPath.AddComponent<SplineContainer>();
            var spline = container.AddSpline();
            var knots = new BezierKnot[pathPoints.Length];
            
            for (int i = 0; i < pathPoints.Length; i++) {
                knots[i] = new BezierKnot(
                    pathPoints[i], 
                    -30 * Vector3.forward, 
                    30 * Vector3.forward);
            }
            
            spline.Knots = knots;
            
            return container;
        }
    }

}