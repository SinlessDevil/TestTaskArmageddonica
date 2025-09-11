using Code.Services.LevelConductor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Code.UI.Game.Battle.Progress
{
    public class BattleProgressView : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private float _animationDuration = 0.8f;
        [SerializeField] private float _scaleMultiplier = 1.15f;
        [SerializeField] private Color _highlightColor = Color.yellow;
        
        private ILevelConductor _levelConductor;
        private Color _originalTextColor;
        private Vector3 _originalTextScale;
        private float _currentProgress = 0f;
        
        [Inject]
        public void Constructor(ILevelConductor levelConductor)
        {
            _levelConductor = levelConductor;
        }
        
        public void Initialize()
        {
            _originalTextColor = _progressText.color;
            _originalTextScale = _progressText.transform.localScale;
            
            UpdateProgressDisplay();
            
            _levelConductor.ChangedWaveEvent += OnChangedWave;
        }

        public void Dispose()
        {
            _levelConductor.ChangedWaveEvent -= OnChangedWave;
        }
        
        private void OnChangedWave()
        {
            UpdateProgressAsync().Forget();
        }
        
        private async UniTask UpdateProgressAsync()
        {
            await AnimateProgressChangeAsync();
        }
        
        private async UniTask AnimateProgressChangeAsync()
        {
            await _progressText.transform.DOScale(_originalTextScale * _scaleMultiplier, _animationDuration * 0.3f)
                .SetEase(Ease.OutBack)
                .ToUniTask();
            
            await _progressText.DOColor(_highlightColor, _animationDuration * 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            UpdateProgressDisplay();
            
            float targetProgress = CalculateProgress();
            await _fillImage.DOFillAmount(targetProgress, _animationDuration * 0.5f)
                .SetEase(Ease.OutQuart)
                .ToUniTask();
            
            await _progressText.DOColor(_originalTextColor, _animationDuration * 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            await _progressText.transform.DOScale(_originalTextScale, _animationDuration * 0.3f)
                .SetEase(Ease.OutBounce)
                .ToUniTask();
        }
        
        private void UpdateProgressDisplay()
        {
            int currentWave = _levelConductor.GetCurrentWave;
            int maxWaves = _levelConductor.GetMaxWaves;
            
            int displayWave = currentWave - 1;
            _progressText.text = $"{displayWave}/{maxWaves}";
        }
        
        private float CalculateProgress()
        {
            int currentWave = _levelConductor.GetCurrentWave;
            int maxWaves = _levelConductor.GetMaxWaves;
            
            return (float)(currentWave - 1) / (maxWaves);
        }
    }
}