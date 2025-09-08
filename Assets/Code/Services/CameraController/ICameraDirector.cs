using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services.CameraController
{
    public interface ICameraDirector
    {
        public UniTask FocusSelectedShotAsync();
        public UniTask FocusBattleShotAsync();
    }
}