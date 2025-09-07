using System.Threading;
using Code.Services.StaticData;
using Code.StaticData.СameraShots;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Code.Services.CameraController
{
    public class CameraDirector : ICameraDirector
    {
        private Transform _rootTransform;
        private Camera _camera;
        
        private Transform _selectionLookAt;
        private Transform _battleLookAt;
        
        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;
        
        private float _defaultFov;
        private float _defaultOrthographic;
        
        private Tween _moveTw, _rotTw, _fovTw, _orthoTw;

        private readonly IStaticDataService _staticDataService;
        
        public CameraDirector(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Setup(Transform rootTransform, Camera camera, Transform selectionLookAt, Transform battleLookAt)
        {
            _rootTransform = rootTransform;
            _camera = camera;
            
            _selectionLookAt = selectionLookAt;
            _battleLookAt = battleLookAt;
            
            _defaultPosition   = _rootTransform.position;
            _defaultRotation   = _rootTransform.rotation;
            
            _defaultFov   = _camera.fieldOfView;
            _defaultOrthographic = _camera.orthographicSize;
        }

        public void Dispose()
        {
            _rootTransform = null;
            _camera = null;
            
            _selectionLookAt = null;
            _battleLookAt = null;
        }

        public async UniTask FocusSelectedShotAsync()
        {
            await FocusAsync(_selectionLookAt, CameraShotStaticData.SelectedShot);
        }

        public async UniTask FocusBattleShotAsync()
        {
            await FocusAsync(_battleLookAt, CameraShotStaticData.BattleShot);
        }

        private async UniTask FocusAsync(Transform target, CameraShot shot, CancellationToken ct = default)
        {
            if (target == null) 
                return;
            
            Vector3 worldPos = shot.PositionOffset;
            Quaternion worldRot = shot.LookAtTarget
                ? Quaternion.LookRotation((target.position - worldPos).normalized, Vector3.up)
                : _rootTransform.rotation;

            await FocusAsync(worldPos, worldRot, shot, ct);
        }

        private async UniTask FocusAsync(Vector3 worldPos, Quaternion worldRot, CameraShot shot, CancellationToken ct = default)
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

        private async UniTask ReturnAsync(CameraShot shot, CancellationToken ct = default)
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

        private void Kill()
        {
            _moveTw?.Kill(); 
            _moveTw = null;
            
            _rotTw?.Kill();  
            _rotTw  = null;
            
            _fovTw?.Kill();  
            _fovTw  = null;
            
            _orthoTw?.Kill();
            _orthoTw= null;
        }

        private static bool Completed(Tween t) => t == null || !t.IsActive() || t.IsComplete();
        
        private CameraShotStaticData CameraShotStaticData => _staticDataService.Balance.CameraShotStaticData;
    }
}
