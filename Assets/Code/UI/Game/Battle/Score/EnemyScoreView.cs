using Code.Services.LevelConductor;
using Code.Services.PowerCalculation;
using Code.StaticData.Invocation.DTO;
using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Code.UI.Game.Battle.Score
{
    public class EnemyScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _enemyScoreText;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _scaleMultiplier = 1.2f;
        [SerializeField] private Color _highlightColor = Color.red;
        
        private ILevelConductor _levelConductor;
        private IInvocationPowerCalculationService _powerCalculationService;
        private Color _originalColor;
        private Vector3 _originalScale;
        private int _currentScore = 0;
        
        [Inject]
        public void Constructor(ILevelConductor levelConductor, IInvocationPowerCalculationService powerCalculationService)
        {
            _levelConductor = levelConductor;
            _powerCalculationService = powerCalculationService;
        }
        
        public void Initialize()
        {
            _originalColor = _enemyScoreText.color;
            _originalScale = _enemyScoreText.transform.localScale;
            _enemyScoreText.text = "0";
            _currentScore = 0;
            
            _levelConductor.ChangedPowerEnemyEvent += OnChangedPowerEnemy;
            _levelConductor.UpdateStatsEvent += OnChangedPowerEnemy;
        }

        public void Dispose()
        {
            _levelConductor.ChangedPowerEnemyEvent -= OnChangedPowerEnemy;
            _levelConductor.UpdateStatsEvent += OnChangedPowerEnemy;
        }

        private void OnChangedPowerEnemy()
        {
            int newScore = Mathf.RoundToInt(_powerCalculationService.CalculateEnemyPower());
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
            await _enemyScoreText.transform.DOScale(_originalScale * _scaleMultiplier, _animationDuration * 0.3f)
                .SetEase(Ease.OutBack)
                .ToUniTask();
            
            await _enemyScoreText.DOColor(_highlightColor, _animationDuration * 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            float duration = _animationDuration * 0.5f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(fromScore, toScore, progress));
                _enemyScoreText.text = currentValue.ToString();
                
                await UniTask.Yield();
            }
            
            _enemyScoreText.text = toScore.ToString();
            
            await _enemyScoreText.DOColor(_originalColor, _animationDuration * 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            await _enemyScoreText.transform.DOScale(_originalScale, _animationDuration * 0.3f)
                .SetEase(Ease.OutBounce)
                .ToUniTask();
        }
    }   
}