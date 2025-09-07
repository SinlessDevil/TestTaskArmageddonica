using Code.Services.CameraController;
using UnityEngine;
using Zenject;
using Sirenix.OdinInspector;

namespace Code.UI.Game
{
    public class TestCamera : MonoBehaviour
    {
        private ICameraDirector _cameraDirector;
        
        [Inject]
        public void Construct(ICameraDirector cameraDirector)
        {
            _cameraDirector = cameraDirector;
        }
        
        [Button]
        public void ForceLootAtCameraSelected(Transform target)
        {
            _cameraDirector.FocusSelectedShotAsync();
        }
        
        [Button]
        public void ForceLootAtCameraBattle(Transform target)
        {
            _cameraDirector.FocusBattleShotAsync();
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