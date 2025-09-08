using System.Threading;
using Code.Services.StaticData;
using Code.StaticData.СameraShots;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Services.Contex;

namespace Code.Services.CameraController
{
    public class CameraDirector : ICameraDirector
    {
        
        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;
        
        private float _defaultFov;
        private float _defaultOrthographic;
        
        private Tween _moveTw, _rotTw, _fovTw, _orthoTw;

        private readonly IStaticDataService _staticDataService;
        private readonly IGameContext _gameContext;

        public CameraDirector(
            IStaticDataService staticDataService,
            IGameContext gameContext)
        {
            _staticDataService = staticDataService;
            _gameContext = gameContext;
        }

        public async UniTask FocusSelectedShotAsync()
        {
            await FocusAsync(SelectionLookAt, CameraShotStaticData.SelectedShot);
        }

        public async UniTask FocusBattleShotAsync()
        {
            await FocusAsync(BattleLookAt, CameraShotStaticData.BattleShot);
        }

        private async UniTask FocusAsync(Transform target, CameraShot shot, CancellationToken ct = default)
        {
            if (target == null) 
                return;
            
            Vector3 worldPos = shot.PositionOffset;
            Quaternion worldRot = shot.LookAtTarget
                ? Quaternion.LookRotation((target.position - worldPos).normalized, Vector3.up)
                : RootTransform.rotation;

            await FocusAsync(worldPos, worldRot, shot, ct);
        }

        private async UniTask FocusAsync(Vector3 worldPos, Quaternion worldRot, CameraShot shot, CancellationToken ct = default)
        {
            Kill();
            
            _moveTw = RootTransform.DOMove(worldPos, shot.Duration)
                         .SetEase(shot.Ease)
                         .SetUpdate(true);
            
            _rotTw = RootTransform.DORotateQuaternion(worldRot, shot.Duration)
                        .SetEase(shot.Ease)
                        .SetUpdate(true);
            
            if (shot.FieldOfView.HasValue && !Camera.orthographic)
                _fovTw = Camera.DOFieldOfView(shot.FieldOfView.Value, shot.Duration)
                                .SetEase(shot.Ease)
                                .SetUpdate(true);

            if (shot.OrthoSize.HasValue && Camera.orthographic)
                _orthoTw = Camera.DOOrthoSize(shot.OrthoSize.Value, shot.Duration)
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

            _moveTw = RootTransform.DOMove(_defaultPosition, shot.Duration)
                         .SetEase(shot.Ease)
                         .SetUpdate(true);

            _rotTw = RootTransform.DORotateQuaternion(_defaultRotation, shot.Duration)
                        .SetEase(shot.Ease)
                        .SetUpdate(true);

            if (!Camera.orthographic)
            {
                _fovTw = Camera.DOFieldOfView(_defaultFov, shot.Duration)
                                .SetEase(shot.Ease)
                                .SetUpdate(true);
            }
            else
            {
                _orthoTw = Camera.DOOrthoSize(_defaultOrthographic, shot.Duration)
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
        
        private Transform RootTransform => _gameContext.Camera.transform;
        private Camera Camera => _gameContext.Camera;
        
        private Transform SelectionLookAt => _gameContext.SelectionLookAtPoint?.transform;
        private Transform BattleLookAt => _gameContext.BattleLookAtPoint?.transform;
    }
}
