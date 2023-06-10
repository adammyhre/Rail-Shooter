using KBCore.Refs;
using UnityEngine;

namespace RailShooter {
    public class Reticle : ValidatedMonoBehaviour {
        [SerializeField] Transform targetPoint;
        [SerializeField, Self] RectTransform rectTransform;

        void Update() => rectTransform.position = Camera.main.WorldToScreenPoint(targetPoint.position);
    }
}