using Code.Services.Factories.UIFactory;
using Code.Services.LocalProgress;
using Code.Services.PowerCalculation;
using Code.Services.StaticData;
using Code.Services.Window;
using Code.UI.Game.Finish.Lose;
using Code.Window;
using Code.Window.Finish.Lose;
using UnityEngine;

namespace Code.Services.Finish.Lose
{
    public class LoseService : ILoseService
    {
        private ILoseWindowPM _loseWindowPm;
        private LoseWindow _loseWindow;
        
        private readonly IWindowService _windowService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IInvocationPowerCalculationService _invocationPowerCalculationService;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;

        public LoseService(
            IWindowService windowService,
            ILevelLocalProgressService levelLocalProgressService, 
            IInvocationPowerCalculationService invocationPowerCalculationService,
            IUIFactory uiFactory,
            IStaticDataService staticDataService)
        {
            _windowService = windowService;
            _levelLocalProgressService = levelLocalProgressService;
            _invocationPowerCalculationService = invocationPowerCalculationService;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
        }
        
        public void Lose()
        {
            _loseWindowPm = new LoseWindowPM(_levelLocalProgressService, _invocationPowerCalculationService,
                _uiFactory, _staticDataService);
            
            RectTransform window = _windowService.Open(WindowTypeId.Lose);
            _loseWindow = window.GetComponent<LoseWindow>();
            _loseWindow.Initialize(_loseWindowPm);

            _loseWindowPm.ClosedWindowEvent += OnCleanup;
        }

        private void OnCleanup()
        {
            _loseWindow.Dispose();
            _loseWindow = null;
            
            _loseWindowPm.ClosedWindowEvent -= OnCleanup;
            _loseWindowPm.Cleanup();
            _loseWindowPm = null;
        }
    }
}