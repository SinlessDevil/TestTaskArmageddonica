using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Code.Services.CameraController
{
    public interface ICameraDirector
    {
        public void Setup(Transform rootTransform, Camera camera);
        public UniTask FocusAsync(Transform target, CameraShot shot, CancellationToken ct = default);
        public UniTask FocusAsync(Vector3 worldPos, Quaternion worldRot, CameraShot shot, CancellationToken ct = default);
        public UniTask ReturnAsync(CameraShot shot, CancellationToken ct = default);
        public void Kill();
        void Dispose();
    }
}