using Code.StaticData.AudioVibration;

namespace Code.Services.AudioVibrationFX.StaticData
{
    public interface IAudioStaticDataService
    {
        SoundsData SoundsData { get; }
        void LoadData();
    }
}