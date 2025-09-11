using Code.Logic.Grid;
using Code.Logic.Points;
using Code.Services.CameraController;
using Code.Services.Context;
using Code.Services.Factories.UIFactory;
using Code.Services.Input;
using Code.Services.Input.Device;
using Code.Services.LevelConductor;
using Code.Services.Providers;
using Code.Services.Providers.Widgets;
using Code.Services.StaticData;
using Code.StaticData;
using Code.UI;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.View;
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
        private readonly IPoolProvider<CardView> _cardviewProvider;
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;
        private readonly ICameraDirector _cameraDirector;
        private readonly ILevelConductor _levelConductor;
        private readonly IGameContext _gameContext;

        public LoadLevelState(
            IStateMachine<IGameState> gameStateMachine, 
            ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain, 
            IUIFactory uiFactory,
            IPoolProvider<Widget> widgetProvider,
            IPoolProvider<CardView> cardviewProvider,
            IInputService inputService,
            IStaticDataService staticDataService,
            ICameraDirector cameraDirector,
            ILevelConductor levelConductor,
            IGameContext gameContext)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _widgetProvider = widgetProvider;
            _cardviewProvider = cardviewProvider;
            _inputService = inputService;
            _staticDataService = staticDataService;
            _cameraDirector = cameraDirector;
            _levelConductor = levelConductor;
            _gameContext = gameContext;
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
            SetupGameContext();
            
            _inputService.SetInputDevice(GetInputDevice());
            
            _uiFactory.CreateUiRoot();
            
            InitHud();
            
            InitProviders();
        }

        private void SetupGameContext()
        {
            PlayerGrid playerGrid = Object.FindAnyObjectByType<PlayerGrid>();
            _gameContext.SetPlayerGrid(playerGrid);
            
            EnemyGird enemyGird = Object.FindAnyObjectByType<EnemyGird>();
            _gameContext.SetEnemyGrid(enemyGird);
            
            SelectionLookAtPoint selectionLookAt = Object.FindAnyObjectByType<SelectionLookAtPoint>();
            _gameContext.SetLookAtPoint(selectionLookAt);
            
            BattleLookAtPoint battleLookAt = Object.FindAnyObjectByType<BattleLookAtPoint>();
            _gameContext.SetBattleLookAtPoint(battleLookAt);
            
            Camera camera = Camera.main;
            _gameContext.SetCamera(camera);
        }
        
        private void InitProviders()
        {
            _widgetProvider.CreatePool();
            _cardviewProvider.CreatePool();
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