using Code.Services.LevelConductor;
using Code.Services.PowerCalculation;
using UnityEngine;
using Zenject;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Code.UI.Game.Battle.Score
{
    public class PlayerScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerScoreText;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _scaleMultiplier = 1.2f;
        [SerializeField] private Color _highlightColor = Color.green;
        
        private Color _originalColor;
        private Vector3 _originalScale;
        private int _currentScore = 0;
        
        private ILevelConductor _levelConductor;
        private IInvocationPowerCalculationService _powerCalculationService;
        
        [Inject]
        public void Constructor(
            ILevelConductor levelConductor, 
            IInvocationPowerCalculationService powerCalculationService)
        {
            _levelConductor = levelConductor;
            _powerCalculationService = powerCalculationService;
        }
        
        public void Initialize()
        {
            _originalColor = _playerScoreText.color;
            _originalScale = _playerScoreText.transform.localScale;
            _playerScoreText.text = "0";
            _currentScore = 0;
            
            _levelConductor.ChangedPowerPlayerEvent += OnChangedPowerPlayer;
            _levelConductor.UpdateStatsEvent += OnChangedPowerPlayer;
        }

        public void Dispose()
        {
            _levelConductor.ChangedPowerPlayerEvent -= OnChangedPowerPlayer;
            _levelConductor.UpdateStatsEvent -= OnChangedPowerPlayer;
        }
        
        private void OnChangedPowerPlayer()
        {
            int newScore = Mathf.RoundToInt(_powerCalculationService.CalculatePlayerPower());
            UpdateScoreAsync(newScore).Forget();
        }
        
        private async UniTask UpdateScoreAsync(int newScore)
        {
            if (newScore == _currentScore) 
                return;
            
            int oldScore = _currentScore;
            _currentScore = newScore;
            
            await AnimateScoreChangeAsync(oldScore, newScore);
        }
        
        private async UniTask AnimateScoreChangeAsync(int fromScore, int toScore)
        {
            await _playerScoreText.transform.DOScale(_originalScale * _scaleMultiplier, _animationDuration * 0.3f)
                .SetEase(Ease.OutBack)
                .ToUniTask();
            
            await _playerScoreText.DOColor(_highlightColor, _animationDuration * 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            float duration = _animationDuration * 0.5f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(fromScore, toScore, progress));
                _playerScoreText.text = currentValue.ToString();
                
                await UniTask.Yield();
            }
            
            _playerScoreText.text = toScore.ToString();
            
            await _playerScoreText.DOColor(_originalColor, _animationDuration * 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            await _playerScoreText.transform.DOScale(_originalScale, _animationDuration * 0.3f)
                .SetEase(Ease.OutBounce)
                .ToUniTask();
        }
    }
}