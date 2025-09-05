using System;
using Code.Services.Factories.UIFactory;
using Code.Services.Input;
using Code.Services.Input.Device;
using Code.Services.Providers.Widgets;
using Code.Services.StaticData;
using Code.StaticData;
using UnityEngine.XR;

namespace Code.Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IWidgetProvider _widgetProvider;
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;

        public LoadLevelState(
            IStateMachine<IGameState> gameStateMachine, 
            ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain, 
            IUIFactory uiFactory,
            IWidgetProvider widgetProvider,
            IInputService inputService,
            IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _widgetProvider = widgetProvider;
            _inputService = inputService;
            _staticDataService = staticDataService;
        }

        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(payload, OnLevelLoad);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        protected virtual void OnLevelLoad()
        {
            InitGameWorld();

            _gameStateMachine.Enter<GamePlayState>();
        }

        private void InitGameWorld()
        {
            _inputService.SetInputDevice(GetInputDevice());
            
            _uiFactory.CreateUiRoot();
            
            InitHud();
            
            InitProviders();
        }
        
        private void InitProviders()
        {
            _widgetProvider.CreatePoolWidgets();
        }
        
        private void InitHud()
        {
            var gameHud = _uiFactory.CreateGameHud();
            gameHud.Initialize();
        }

        private IInputDevice GetInputDevice()
        {
            return _staticDataService.GameConfig.TypeInput switch
            {
                TypeInput.PC => new MouseInputDevice(),
                TypeInput.Mobile => new TouchInputDevice(),
                _ => new NullableInputDevice()
            };
        }
    }
}