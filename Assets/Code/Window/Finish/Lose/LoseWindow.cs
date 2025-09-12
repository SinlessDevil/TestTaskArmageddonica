using System.Collections.Generic;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.AudioVibrationFX.Sound;
using Code.Services.StaticData;
using Code.UI.Game.Finish.InvocationIcon;
using Code.UI.Game.Finish.Lose;
using UnityEngine;
using Zenject;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Code.Window.Finish.Lose
{
    public class LoseWindow : FinishWindow
    {
        [Header("Score Display")]
        [SerializeField] private TMP_Text _scoreText;
        [Header("Units Container")]
        [SerializeField] private Transform _invocationContainer;
        [SerializeField] private InvocationLayoutEngine _layoutEngine;
        
        private IStateMachine<IGameState> _gameStateMachine;
        private IStaticDataService _staticDataService;
        private ISoundService _soundService;
        private ILoseWindowPM _loseWindowPM;
        
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
        
        public void Initialize(ILoseWindowPM loseWindowPM)
        {
            _loseWindowPM = loseWindowPM;
            
            SubscribeEvents();
            SetupUI();
            
            if (_layoutEngine != null)
                _layoutEngine.Initialize();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
            _loseWindowPM = null;
        }
        
        private void SetupUI()
        {
            SetupScore();
            SetupUsedUnits();
        }
        
        private void SetupScore()
        {
            _scoreText.text = $"Score: {_loseWindowPM.PlayerScore}";
        }
        
        private void SetupUsedUnits()
        {
            if (_layoutEngine != null)
                _layoutEngine.ClearAllIcons();
            
            CreateUnitIconsWithAnimation().Forget();
        }
        
        private async UniTask CreateUnitIconsWithAnimation()
        {
            float delayBetweenIcons = 0.1f;
            List<InvocationIconComposite> invocationCompositeCollection = _loseWindowPM.GetInvocationCompositeCollection();

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