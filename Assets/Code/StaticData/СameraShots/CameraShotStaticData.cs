using Code.Services.CameraController;
using UnityEngine;

namespace Code.StaticData.СameraShots
{
    [CreateAssetMenu(fileName = "CameraShotStaticData", menuName = "StaticData/CameraShots", order = 0)]
    public class CameraShotStaticData : ScriptableObject
    {
        public CameraShot SelectedShot;
        public CameraShot BattleShot;
    }
}