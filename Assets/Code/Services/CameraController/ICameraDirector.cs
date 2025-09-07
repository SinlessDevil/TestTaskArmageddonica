using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.CameraController
{
    public interface ICameraDirector
    {
        public void Setup(Transform rootTransform, Camera camera, Transform selectionLookAt, Transform battleLookAt);
        public void Dispose();
        public UniTask FocusSelectedShotAsync();
        public UniTask FocusBattleShotAsync();
    }
}