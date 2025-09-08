using Code.Logic.Points;
using Code.Services.CameraController;
using Code.Services.Factories.UIFactory;
using Code.Services.Input;
using Code.Services.Input.Device;
using Code.Services.LevelConductor;
using Code.Services.Providers;
using Code.Services.Providers.Widgets;
using Code.Services.StaticData;
using Code.StaticData;
using Code.UI;
using UnityEngine;
using Grid = Code.Logic.Grid.Grid;
using Object = UnityEngine.Object;

namespace Code.Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IPoolProvider<Widget> _widgetProvider;
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;
        private readonly ICameraDirector _cameraDirector;
        private readonly ILevelConductor _levelConductor;

        public LoadLevelState(
            IStateMachine<IGameState> gameStateMachine, 
            ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain, 
            IUIFactory uiFactory,
            IPoolProvider<Widget> widgetProvider,
            IInputService inputService,
            IStaticDataService staticDataService,
            ICameraDirector cameraDirector,
            ILevelConductor levelConductor)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _widgetProvider = widgetProvider;
            _inputService = inputService;
            _staticDataService = staticDataService;
            _cameraDirector = cameraDirector;
            _levelConductor = levelConductor;
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
            SetupCameraDirector();
            
            SetupLevelConductor();
            
            _inputService.SetInputDevice(GetInputDevice());
            
            _uiFactory.CreateUiRoot();
            
            InitHud();
            
            InitProviders();
        }

        private void SetupLevelConductor()
        {
            Grid grid = Object.FindAnyObjectByType<Grid>();
            _levelConductor.Setup(grid);
        }
        
        private void SetupCameraDirector()
        {
            SelectionLookAtPoint selectionLookAt = Object.FindAnyObjectByType<SelectionLookAtPoint>();
            BattleLookAtPoint battleLookAt = Object.FindAnyObjectByType<BattleLookAtPoint>();
            _cameraDirector.Setup(Camera.main.transform, Camera.main, 
                selectionLookAt.transform, battleLookAt.transform);
        }
        
        private void InitProviders()
        {
            _widgetProvider.CreatePool();
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