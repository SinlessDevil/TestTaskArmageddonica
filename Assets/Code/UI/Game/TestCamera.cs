using Code.Services.CameraController;
using Code.Services.StaticData;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Code.UI.Game
{
    public class TestCamera : MonoBehaviour
    {
        private ICameraDirector _cameraDirector;
        private IStaticDataService _staticDataService;
        
        [Inject]
        public void Construct(
            ICameraDirector cameraDirector,
            IStaticDataService staticDataService)
        {
            _cameraDirector = cameraDirector;
            _staticDataService = staticDataService;
        }
        
        [Button]
        public void ForceLootAtCameraSelected(Transform target)
        {
            var cameraShotStaticData = _staticDataService.Balance.CameraShotStaticData;
            _cameraDirector.FocusAsync(target, cameraShotStaticData.SelectedShot);
        }
        
        [Button]
        public void ForceLootAtCameraBattle(Transform target)
        {
            var cameraShotStaticData = _staticDataService.Balance.CameraShotStaticData;
            _cameraDirector.FocusAsync(target, cameraShotStaticData.BattleShot);
        }
        
        private void OnDrawGizmos()
        {
            if (Camera.main == null)
                return;

            Gizmos.color = Color.red;
            
            Vector3 camPos = Camera.main.transform.position;
            Vector3 camDir = Camera.main.transform.forward;
            
            Gizmos.DrawRay(camPos, camDir * 10f);
            Gizmos.DrawSphere(camPos + camDir * 10f, 0.2f);
        }
    }
}