using Code.Services.LevelConductor;
using UnityEngine;
using Zenject;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Code.UI.Game.Battle
{
    public class FightDisplayer : MonoBehaviour
    {
        private const string FightText = "Fight";
        
        [SerializeField] private TMP_Text _fightText;
        [SerializeField] private float _animationDuration = 2f;
        [SerializeField] private float _textScale = 1.5f;
        [SerializeField] private Color _pulseColor = Color.red;
        
        private ILevelConductor _levelConductor;
        private Color _originalColor;
        private Vector3 _originalScale;
        
        [Inject]
        public void Constructor(ILevelConductor levelConductor)
        {
            _levelConductor = levelConductor;
        }
        
        public void Initialize()
        {
            _originalColor = _fightText.color;
            _originalScale = _fightText.transform.localScale;
            _fightText.transform.localScale = Vector3.zero;
            _fightText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0f);
            
            _levelConductor.RunnedBattleEvent += OnRunnedBattleEvent;
        }

        public void Dispose()
        {
            _levelConductor.RunnedBattleEvent -= OnRunnedBattleEvent;
        }
        
        private void OnRunnedBattleEvent()
        {
            PlayFightAnimationAsync().Forget();
        }
        
        private async UniTask PlayFightAnimationAsync()
        {
            _fightText.text = FightText;
            
            await _fightText.transform.DOScale(_originalScale * _textScale, 0.3f)
                .SetEase(Ease.OutBack)
                .ToUniTask();
            
            await _fightText.DOFade(1f, 0.3f)
                .ToUniTask();
            
            await _fightText.DOColor(_pulseColor, 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            await _fightText.DOColor(_originalColor, 0.2f)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            await _fightText.transform.DOShakePosition(0.5f, strength: 10f, vibrato: 20)
                .SetEase(Ease.InOutQuad)
                .ToUniTask();
            
            await _fightText.transform.DOScale(_originalScale, 0.4f)
                .SetEase(Ease.OutBounce)
                .ToUniTask();
            
            await _fightText.DOFade(0f, 0.5f)
                .SetEase(Ease.InQuad)
                .ToUniTask();
            
            await _fightText.transform.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack)
                .ToUniTask();
            
            _fightText.transform.localScale = Vector3.zero;
            _fightText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0f);
        }
    }   
}