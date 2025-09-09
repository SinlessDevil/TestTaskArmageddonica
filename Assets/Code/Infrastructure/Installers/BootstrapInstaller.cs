using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.Battle;
using Code.Infrastructure.StateMachine.Battle.States;
using Code.Infrastructure.StateMachine.Game;
using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.AudioVibrationFX.Music;
using Code.Services.AudioVibrationFX.Sound;
using Code.Services.AudioVibrationFX.StaticData;
using Code.Services.CameraController;
using Code.Services.CardSelection;
using Code.Services.Context;
using Code.Services.Factories.Game;
using Code.Services.Factories.Grid;
using Code.Services.Factories.UIFactory;
using Code.Services.Finish;
using Code.Services.Finish.Lose;
using Code.Services.Finish.Win;
using Code.Services.Input;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;
using Code.Services.IInvocation.Factories;
using Code.Services.IInvocation.InvocationHandle;
using Code.Services.IInvocation.Randomizer;
using Code.Services.IInvocation.StaticData;
using Code.Services.LevelConductor;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.PersistenceProgress;
using Code.Services.Providers;
using Code.Services.Providers.CardComposites;
using Code.Services.Providers.Cards;
using Code.Services.Providers.Widgets;
using Code.Services.Random;
using Code.Services.SaveLoad;
using Code.Services.StaticData;
using Code.Services.Storage;
using Code.Services.Timer;
using Code.Services.Window;
using Code.UI;
using Code.UI.Game.Cards.View;
using UnityEngine;
using Zenject;
using Application = UnityEngine.Application;

namespace Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingCurtain _curtain;
        
        private RuntimePlatform Platform => Application.platform;

        public override void InstallBindings()
        {
            Debug.Log("Installer");

            BindMonoServices();
            BindServices();
            BindGameStateMachine();
            BindBattleStateMachine();
            MakeInitializable();
        }
        
        public void Initialize() => BootstrapGame();

        private void BindMonoServices()
        {
            Container.Bind<ICoroutineRunner>().FromMethod(() => Container.InstantiatePrefabForComponent<ICoroutineRunner>(_coroutineRunner)).AsSingle();
            Container.Bind<ILoadingCurtain>().FromMethod(() => Container.InstantiatePrefabForComponent<ILoadingCurtain>(_curtain)).AsSingle();
            
            BindSceneLoader();
        }

        private void BindServices()
        {
            BindStaticDataService();
            BindInvocationServices();
            BindProviders();
            BindFactories();
            BindInputServices();
            BindWindowServices();
            
            Container.BindInterfacesTo<RandomService>().AsSingle();
            Container.BindInterfacesTo<UnifiedSaveLoadFacade>().AsSingle();
            Container.BindInterfacesTo<StorageService>().AsSingle();
            Container.BindInterfacesTo<TimeService>().AsSingle();
            Container.BindInterfacesTo<GameContext>().AsSingle();
            
            BindLevelServices();

            Container.BindInterfacesTo<CameraDirector>().AsSingle();
            
            BindDataServices();
            BindAudioVibrationService();
            BindFinishService();
        }

        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.LoadData();
            Container.Bind<IStaticDataService>().FromInstance(staticDataService).AsSingle();
            
            IAudioStaticDataService audioStaticDataService = new AudioStaticDataService();
            audioStaticDataService.LoadData();
            Container.Bind<IAudioStaticDataService>().FromInstance(audioStaticDataService).AsSingle();
            
            IInvocationStaticDataService invocationStaticDataService = new InvocationStaticDataService();
            invocationStaticDataService.LoadData();
            Container.Bind<IInvocationStaticDataService>().FromInstance(invocationStaticDataService).AsSingle();
        }

        private void BindInvocationServices()
        {
            Container.BindInterfacesTo<InvocationHandlerService>().AsSingle();
            Container.BindInterfacesTo<InvocationDataRandomizerService>().AsSingle();
        }
        
        private void BindProviders()
        {
            Container.Bind<IPoolFactory<CardView>>().To<CardViewFactory>().AsSingle();
            Container.Bind<IPoolProvider<CardView>>().To<CardViewProvider>().AsSingle();

            Container.Bind<IPoolFactory<Widget>>().To<WidgetFactory>().AsSingle();
            Container.Bind<IPoolProvider<Widget>>().To<WidgetProvider>().AsSingle();
            
            Container.BindInterfacesTo<CardCompositeProvider>().AsSingle();
        }
        
        private void BindFactories()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<GameFactory>().AsSingle();
            Container.BindInterfacesTo<GridFactory>().AsSingle();
            Container.BindInterfacesTo<InvocationFactory>().AsSingle();
        }
        
        private void BindInputServices()
        {
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.BindInterfacesTo<GridInputService>().AsSingle();
            Container.BindInterfacesTo<CardInputService>().AsSingle();
        }
        
        private void BindWindowServices()
        {
            Container.BindInterfacesTo<WindowService>().AsSingle();
            Container.BindInterfacesTo<CardSelectionWindowService>().AsSingle();
        }
        
        private void BindLevelServices()
        {
            Container.BindInterfacesTo<LevelService>().AsSingle();
            Container.BindInterfacesTo<LevelConductor>().AsSingle();
        }

        private void BindDataServices()
        {
            Container.BindInterfacesTo<PersistenceProgressService>().AsSingle();
            Container.BindInterfacesTo<LevelLocalProgressService>().AsSingle();
        }
        
        private void BindAudioVibrationService()
        {
            Container.BindInterfacesTo<SoundService>().AsSingle();
            Container.BindInterfacesTo<MusicService>().AsSingle();
            
            Container.Resolve<ISoundService>().Cache2DSounds();
            Container.Resolve<ISoundService>().CreateSoundsPool();
            
            Container.Resolve<IMusicService>().CacheMusic();
            Container.Resolve<IMusicService>().CreateMusicRoot();
        }

        private void BindFinishService()
        {
            Container.BindInterfacesTo<FinishService>().AsSingle();
            Container.BindInterfacesTo<WinService>().AsSingle();
            Container.BindInterfacesTo<LoseService>().AsSingle();
        }
        
        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            
            BindGameStates();
        }

        private void BindBattleStateMachine()
        {
            Container.Bind<BattleStateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStateMachine>().AsSingle();
            
            BindBattleStates();
        }
        
        private void MakeInitializable() => Container.Bind<IInitializable>().FromInstance(this);

        private void BindSceneLoader()
        {
            ISceneLoader sceneLoader = new SceneLoader(Container.Resolve<ICoroutineRunner>());
            Container.Bind<ISceneLoader>().FromInstance(sceneLoader).AsSingle();
        }
        
        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<BootstrapAnalyticState>().AsSingle();
            Container.Bind<PreLoadGameState>().AsSingle();
            Container.Bind<LoadMenuState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GamePlayState>().AsSingle();
        }

        private void BindBattleStates()
        {
            Container.Bind<InitializeBattleState>().AsSingle();
            Container.Bind<CardSelectionBattleState>().AsSingle();
            Container.Bind<CardPlacementBattleState>().AsSingle();
            Container.Bind<PlayBattleState>().AsSingle();
            Container.Bind<PauseBattleState>().AsSingle();
            Container.Bind<CleanupBattleState>().AsSingle();
        }
        
        private void BootstrapGame() => Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();
    }
}