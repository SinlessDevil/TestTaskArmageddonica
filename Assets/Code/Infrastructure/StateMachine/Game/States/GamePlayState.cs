using Code.Services.Input;
using Code.Services.LevelConductor;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.Providers.Widgets;
using Code.Services.Timer;

namespace Code.Infrastructure.StateMachine.Game.States
{
    public class GamePlayState : IState, IGameState, IUpdatable
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly IWidgetProvider _widgetProvider;
        private readonly ILevelService _levelService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly ITimeService _timeService;
        private readonly ILevelConductor _levelConductor;

        public GamePlayState(
            IStateMachine<IGameState> gameStateMachine, 
            IInputService inputService,
            IWidgetProvider widgetProvider,
            ILevelService levelService,
            ILevelLocalProgressService levelLocalProgressService,
            ITimeService timeService,
            ILevelConductor levelConductor)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _widgetProvider = widgetProvider;
            _levelService = levelService;
            _levelLocalProgressService = levelLocalProgressService;
            _timeService = timeService;
            _levelConductor = levelConductor;
        }
        
        public void Enter()
        {
            _levelConductor.Setup();
            _levelConductor.Run();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            _levelConductor.Dispose();
            
            _inputService.Cleanup();
            _widgetProvider.CleanupPool();
            _levelService.Cleanup();
            _levelLocalProgressService.Cleanup();
            
            _timeService.ResetTimer();
        }
    }
}