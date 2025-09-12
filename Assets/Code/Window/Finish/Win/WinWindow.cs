using System.Collections.Generic;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.AudioVibrationFX.Sound;
using Code.Services.StaticData;
using Code.UI.Game.Finish.InvocationIcon;
using Code.UI.Game.Finish.Win;
using UnityEngine;
using Zenject;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Code.Window.Finish.Win
{
    public class WinWindow : FinishWindow
    {
        [Header("Score Display")]
        [SerializeField] private TMP_Text _scoreText;
        [Header("Units Container")]
        [SerializeField] private Transform _invocationContainer;
        
        private IStateMachine<IGameState> _gameStateMachine;
        private IStaticDataService _staticDataService;
        private ISoundService _soundService;
        private IWinWindowPM _winWindowPM;
        
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
        }

        public void Dispose()
        {
            UnsubscribeEvents();
            _winWindowPM = null;
        }
        
        private void SetupUI()
        {
            SetupScore();
            SetupUsedUnits();
        }
        
        private void SetupScore()
        {
            _scoreText.text = $"Score: {_winWindowPM.PlayerScore}";
        }
        
        private void SetupUsedUnits()
        {
            foreach (Transform child in _invocationContainer) 
                Destroy(child.gameObject);
            
            CreateUnitIconsWithAnimation().Forget();
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
            invocationIconComposite.View.transform.SetParent(_invocationContainer);
            invocationIconComposite.View.Initialize(invocationIconComposite.PM);
            invocationIconComposite.View.ShowWithAnimation(delay);
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