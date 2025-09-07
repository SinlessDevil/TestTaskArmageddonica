using DG.Tweening;
using UnityEngine;

namespace Code.Services.CameraController
{
    [System.Serializable]
    public struct CameraShot
    {
        public float Duration;
        public Ease Ease;
        public Vector3 PositionOffset;
        public bool LookAtTarget;
        public float? FieldOfView;
        public float? OrthoSize;
    }   
}