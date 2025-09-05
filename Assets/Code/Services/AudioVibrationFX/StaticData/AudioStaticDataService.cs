using Code.StaticData.AudioVibration;
using UnityEngine;

namespace Code.Services.AudioVibrationFX.StaticData
{
    public class AudioStaticDataService : IAudioStaticDataService
    {
        private const string SoundsDataPath = "StaticData/Sounds/Sounds";
        
        private SoundsData _soundsData;
        
        public SoundsData SoundsData => _soundsData;

        public void LoadData()
        {
            _soundsData = Resources.Load<SoundsData>(SoundsDataPath);
        }
    }
}