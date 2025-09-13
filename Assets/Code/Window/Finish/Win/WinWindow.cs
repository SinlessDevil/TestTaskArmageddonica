using System.Collections.Generic;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.AudioVibrationFX.Sound;
using Code.Services.StaticData;
using Code.UI.Game.Finish.InvocationIcon;
using Code.UI.Game.Finish.Win;
using Coffee.UIExtensions;
using UnityEngine;
using Zenject;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Code.Window.Finish.Win
{
    public class WinWindow : FinishWindow
    {
        [Header("Score Display")]
        [SerializeField] private TMP_Text _scoreText;
        [Header("Units Container")]
        [SerializeField] private Transform _invocationContainer;
        [SerializeField] private InvocationLayoutEngine _layoutEngine;
        [Header("Animation Components")]
        [SerializeField] private List<UIParticle> _uiParticles;
        [SerializeField] private CanvasGroup _backgroundFade;
        [SerializeField] private RectTransform _mainBackground;
        [SerializeField] private RectTransform _header;
        [SerializeField] private RectTransform _leftButton;
        [SerializeField] private RectTransform _rightButton;
        [Header("Animation Settings")]
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _scaleDuration = 0.4f;
        [SerializeField] private float _scoreCountDuration = 1.0f;
        [SerializeField] private float _buttonSlideDuration = 0.6f;
        [SerializeField] private float _buttonSlideDistance = 200f;
        
        private IStateMachine<IGameState> _gameStateMachine;
        private IStaticDataService _staticDataService;
        private ISoundService _soundService;
        private IWinWindowPM _winWindowPM;
        
        private Vector2 _leftButtonOriginalPosition;
        private Vector2 _rightButtonOriginalPosition;
        private bool _isAnimating = false;
        
        [Inject]
        public void Constructor(
            IStateMachine<IGameState> gameStateMachine,
            IStaticDataService staticDataService,
            ISoundService soundService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _soundService = soundService;
        }
        
        public void Initialize(IWinWindowPM winWindowPM)
        {
            _winWindowPM = winWindowPM;
            
            SubscribeEvents();
            SetupUI();
            
            InitializeAnimation();
            
            _layoutEngine.Initialize();
            
            PlayShowAnimation().Forget();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
            _winWindowPM = null;
        }
        
        private void SetupUI()
        {
            SetupScore();
        }
        
        private void SetupScore()
        {
            _scoreText.text = $"Score: {_winWindowPM.PlayerScore}";
        }
        
        private void InitializeAnimation()
        {
            _leftButtonOriginalPosition = _leftButton.anchoredPosition;
            _rightButtonOriginalPosition = _rightButton.anchoredPosition;
            _backgroundFade.alpha = 0f;
            _mainBackground.localScale = Vector3.zero;
            _header.localScale = new Vector3(0f, 1f, 1f);
            _leftButton.anchoredPosition = _leftButtonOriginalPosition + Vector2.left * _buttonSlideDistance;
            _rightButton.anchoredPosition = _rightButtonOriginalPosition + Vector2.right * _buttonSlideDistance;
            _leftButton.gameObject.SetActive(false);
            _rightButton.gameObject.SetActive(false);

            
            SetInteractable(false);
        }
        
        
        private async UniTask CreateUnitIconsWithAnimation()
        {
            float delayBetweenIcons = 0.1f;
            List<InvocationIconComposite> invocationCompositeCollection = _winWindowPM.GetInvocationCompositeCollection();

            for (int i = 0; i < invocationCompositeCollection.Count; i++)
            {
                float delay = i * delayBetweenIcons;
                InitializeInvocationIcon(delay, invocationCompositeCollection[i]);
                await UniTask.Delay((int)(delayBetweenIcons * 1000));
            }
        }
        
        private void InitializeInvocationIcon(float delay, InvocationIconComposite invocationIconComposite)
        {
            invocationIconComposite.View.Initialize(invocationIconComposite.PM);
            invocationIconComposite.View.ShowWithAnimation(delay);
            
            if (_layoutEngine != null)
                _layoutEngine.AddIcon(invocationIconComposite.View);
        }
        
        private async UniTask PlayShowAnimation()
        {
            if (_isAnimating) 
                return;
            
            _isAnimating = true;
            
            try
            {
                await AnimateBackgroundFade();
                await AnimateMainBackground();
                await AnimateHeader();
                await PlayUIParticle();
                await AnimateScoreText();
                await CreateUnitIconsWithAnimation();
                await AnimateButtons();
                SetInteractable(true);
            }
            finally
            {
                _isAnimating = false;
            }
        }
        
        private async UniTask AnimateBackgroundFade() =>
            await _backgroundFade.DOFade(1f, _fadeDuration)
                .SetEase(Ease.OutQuad)
                .ToUniTask();

        private async UniTask AnimateMainBackground() =>
            await _mainBackground.DOScale(Vector3.one, _scaleDuration)
                .SetEase(Ease.OutBack)
                .ToUniTask();
        
        private async UniTask AnimateHeader() =>
            await _header.DOScaleX(1f, _scaleDuration)
                .SetEase(Ease.OutBack)
                .ToUniTask();

        private async UniTask PlayUIParticle()
        {
            foreach (UIParticle uiParticle in _uiParticles) 
                uiParticle.Play();
        }
        
        private async UniTask AnimateScoreText()
        {
            float targetScore = _winWindowPM.PlayerScore;
            _scoreText.text = "Score: 0";
            
            float currentScore = 0f;
            float elapsedTime = 0f;
            
            while (elapsedTime < _scoreCountDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / _scoreCountDuration;
                progress = DOVirtual.EasedValue(0f, 1f, progress, Ease.OutQuad);
                
                currentScore = Mathf.Lerp(0f, targetScore, progress);
                _scoreText.text = $"Score: {Mathf.RoundToInt(currentScore)}";
                
                await UniTask.Yield();
            }
            
            _scoreText.text = $"Score: {targetScore}";
        }
        
        private async UniTask AnimateButtons()
        {
            List<UniTask> tasks = new List<UniTask>();
            
            _leftButton.gameObject.SetActive(true);
            _rightButton.gameObject.SetActive(true);
            
            tasks.Add(_leftButton.DOAnchorPos(_leftButtonOriginalPosition, _buttonSlideDuration)
                .SetEase(Ease.OutBack)
                .ToUniTask());
            
            tasks.Add(_rightButton.DOAnchorPos(_rightButtonOriginalPosition, _buttonSlideDuration)
                .SetEase(Ease.OutBack)
                .ToUniTask());
            
            await UniTask.WhenAll(tasks);
        }
        
        private void SetInteractable(bool interactable)
        {
            _buttonLoadLevel.interactable = interactable;
            _buttonExitToMenu.interactable = interactable;
        }
        
        protected override void OnLoadLevelButtonClick()
        {
            _soundService.PlaySound(Sound2DType.Click);
            _gameStateMachine.Enter<LoadLevelState, string>(_staticDataService.GameConfig.GameScene);
        }

        protected override void OnExitToMenuButtonClick()
        {
            _soundService.PlaySound(Sound2DType.Click);
            _gameStateMachine.Enter<LoadMenuState, string>(_staticDataService.GameConfig.MenuScene);
        }
    }   
}