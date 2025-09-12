using Code.Services.Factories.UIFactory;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.PersistenceProgress;
using Code.Services.PersistenceProgress.Player;
using Code.Services.PowerCalculation;
using Code.Services.SaveLoad;
using Code.Services.StaticData;
using Code.Services.Timer;
using Code.Services.Window;
using Code.UI.Game.Finish.Win;
using Code.Window;
using Code.Window.Finish.Win;
using UnityEngine;

namespace Code.Services.Finish.Win
{
    public class WinService : IWinService
    {
        private IWinWindowPM _winWindowPM;
        private WinWindow _winWindow;
        
        private readonly IWindowService _windowService;
        private readonly ILevelService _levelService;
        private readonly ISaveLoadFacade _saveLoadFacade;
        private readonly IPersistenceProgressService _persistenceProgressService;
        private readonly ITimeService _timeService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IInvocationPowerCalculationService _invocationPowerCalculationService;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;

        public WinService(
            IWindowService windowService, 
            ILevelService levelService,
            ISaveLoadFacade saveLoadFacade,
            IPersistenceProgressService persistenceProgressService,
            ITimeService timeService,
            ILevelLocalProgressService levelLocalProgressService,
            IInvocationPowerCalculationService invocationPowerCalculationService,
            IUIFactory uiFactory,
            IStaticDataService staticDataService)
        {
            _windowService = windowService;
            _levelService = levelService;
            _saveLoadFacade = saveLoadFacade;
            _persistenceProgressService = persistenceProgressService;
            _timeService = timeService;
            _levelLocalProgressService = levelLocalProgressService;
            _invocationPowerCalculationService = invocationPowerCalculationService;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
        }
        
        public void Win()
        {
            CompleteLevel();
            CompleteTutor();
            SetRecordText();
            SaveProgress();
            
            _winWindowPM = new WinWindowPM(_levelLocalProgressService, _invocationPowerCalculationService,
                _uiFactory, _staticDataService);
            
            RectTransform window = _windowService.Open(WindowTypeId.Win);
            _winWindow = window.GetComponent<WinWindow>();
            _winWindow.Initialize(_winWindowPM);

            _winWindowPM.ClosedWindowEvent += OnCleanup;
        }
        
        private void CompleteLevel() => _levelService.LevelsComplete();

        private void CompleteTutor() => _persistenceProgressService.PlayerData.PlayerTutorialData.HasFirstCompleteLevel = true;

        private void SetRecordText()
        {
            float currentRecordTime = GetCurrentRecordTime();
            float currentTime = _timeService.GetElapsedTime();
            LevelContainer currentLevelContainer = _levelService.GetCurrentLevelContainer();
            
            if(currentRecordTime == 0)
                return;

            if (!(currentTime > currentRecordTime)) 
                return;
            
            LevelContainer existingLevel = _persistenceProgressService.PlayerData.PlayerLevelData.LevelsComleted
                .Find(level => level == currentLevelContainer);
            existingLevel.Time = currentTime;
        }

        private float GetCurrentRecordTime()
        {
            LevelContainer currentLevelContainer = _levelService.GetCurrentLevelContainer();
            if(currentLevelContainer == null)
                return 0;
            
            return currentLevelContainer.Time;
        }
        
        private void SaveProgress() => _saveLoadFacade.SaveProgress(SaveMethod.PlayerPrefs);

        private void OnCleanup()
        {
            _winWindow.Dispose();
            _winWindow = null;
            
            _winWindowPM.ClosedWindowEvent -= OnCleanup;
            _winWindowPM.Cleanup();
            _winWindowPM = null;
        }
    }
}
