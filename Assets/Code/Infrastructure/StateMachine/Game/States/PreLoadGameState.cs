using Code.Services.PersistenceProgress;
using Code.Services.StaticData;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.StateMachine.Game.States
{
    public class PreLoadGameState : IPayloadedState<TypeLoad>, IGameState
    {
        private string _firstSceneName;
        
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IPersistenceProgressService _persistenceProgressService;
        private readonly IStaticDataService _staticData;
        private readonly ILoadingCurtain _loadingCurtain;

        public PreLoadGameState(
            IStateMachine<IGameState> stateMachine,
            IPersistenceProgressService persistenceProgressService,
            IStaticDataService staticData,
            ILoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _persistenceProgressService = persistenceProgressService;
            _staticData = staticData;
            _loadingCurtain = loadingCurtain;
        }
        
        public void Enter(TypeLoad payload)
        {
            if(TypeLoad.MenuLoading == payload)
            {
                _stateMachine.Enter<LoadLevelState, string>(_staticData.GameConfig.GameScene);
                return;
            }
            
            bool hasCompletedFirstLevel = _persistenceProgressService.PlayerData.PlayerTutorialData.HasFirstCompleteLevel;
            _firstSceneName = FirstSceneName(hasCompletedFirstLevel);

            if (hasCompletedFirstLevel)
            {
                _stateMachine.Enter<LoadMenuState, string>(_firstSceneName);
            }
            else
            {
                _stateMachine.Enter<LoadLevelState, string>(_firstSceneName);
            }
        }

        public void Exit()
        {
                
        }
        
        private string FirstSceneName(bool hasCompletedFirstLevel)
        {
            var nameScene = string.Empty;
            nameScene = hasCompletedFirstLevel ? _staticData.GameConfig.MenuScene : _staticData.GameConfig.GameScene;
            
#if UNITY_EDITOR
            if (_staticData.GameConfig.CanLoadCurrentOpenedScene)
                nameScene = SceneManager.GetActiveScene().name;        
#endif
            return nameScene;
        }
    }
}