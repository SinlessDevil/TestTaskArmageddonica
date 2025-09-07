using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Code.Services.CameraController
{
    public class CameraDirector : ICameraDirector
    {
        private Transform _rootTransform;
        private Camera _camera;
        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;
        private float _defaultFov;
        private float _defaultOrthographic;
        
        private Tween _moveTw, _rotTw, _fovTw, _orthoTw;

        public void Setup(Transform rootTransform, Camera camera)
        {
            _rootTransform = rootTransform;
            _camera = camera;
            _defaultPosition   = _rootTransform.position;
            _defaultRotation   = _rootTransform.rotation;
            _defaultFov   = _camera.fieldOfView;
            _defaultOrthographic = _camera.orthographicSize;
        }

        public void Dispose()
        {
            _rootTransform = null;
            _camera = null;
        }

        public async UniTask FocusAsync(Transform target, CameraShot shot, CancellationToken ct = default)
        {
            if (target == null) 
                return;
            
            Vector3 worldPos = shot.PositionOffset;
            Quaternion worldRot = shot.LookAtTarget
                ? Quaternion.LookRotation((target.position - worldPos).normalized, Vector3.up)
                : _rootTransform.rotation;

            await FocusAsync(worldPos, worldRot, shot, ct);
        }

        public async UniTask FocusAsync(Vector3 worldPos, Quaternion worldRot, CameraShot shot, CancellationToken ct = default)
        {
            Kill();
            
            _moveTw = _rootTransform.DOMove(worldPos, shot.Duration)
                         .SetEase(shot.Ease)
                         .SetUpdate(true);
            
            _rotTw = _rootTransform.DORotateQuaternion(worldRot, shot.Duration)
                        .SetEase(shot.Ease)
                        .SetUpdate(true);
            
            if (shot.FieldOfView.HasValue && !_camera.orthographic)
                _fovTw = _camera.DOFieldOfView(shot.FieldOfView.Value, shot.Duration)
                                .SetEase(shot.Ease)
                                .SetUpdate(true);

            if (shot.OrthoSize.HasValue && _camera.orthographic)
                _orthoTw = _camera.DOOrthoSize(shot.OrthoSize.Value, shot.Duration)
                                   .SetEase(shot.Ease)
                                   .SetUpdate(true);

            await UniTask.WaitUntil(
                () => Completed(_moveTw) && Completed(_rotTw) && Completed(_fovTw) && Completed(_orthoTw),
                cancellationToken: ct
            );
        }

        public async UniTask ReturnAsync(CameraShot shot, CancellationToken ct = default)
        {
            Kill();

            _moveTw = _rootTransform.DOMove(_defaultPosition, shot.Duration)
                         .SetEase(shot.Ease)
                         .SetUpdate(true);

            _rotTw = _rootTransform.DORotateQuaternion(_defaultRotation, shot.Duration)
                        .SetEase(shot.Ease)
                        .SetUpdate(true);

            if (!_camera.orthographic)
            {
                _fovTw = _camera.DOFieldOfView(_defaultFov, shot.Duration)
                                .SetEase(shot.Ease)
                                .SetUpdate(true);
            }
            else
            {
                _orthoTw = _camera.DOOrthoSize(_defaultOrthographic, shot.Duration)
                                   .SetEase(shot.Ease)
                                   .SetUpdate(true);
            }

            await UniTask.WaitUntil(
                () => Completed(_moveTw) && Completed(_rotTw) && Completed(_fovTw) && Completed(_orthoTw),
                cancellationToken: ct
            );
        }

        public void Kill()
        {
            _moveTw?.Kill(); _moveTw = null;
            _rotTw?.Kill();  _rotTw  = null;
            _fovTw?.Kill();  _fovTw  = null;
            _orthoTw?.Kill();_orthoTw= null;
        }

        private static bool Completed(Tween t) => t == null || !t.IsActive() || t.IsComplete();
    }
}
