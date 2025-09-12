using System;
using System.Linq;
using System.Collections.Generic;
using Code.Services.Factories.UIFactory;
using Code.Services.LocalProgress;
using Code.Services.PowerCalculation;
using Code.Services.StaticData;
using Code.StaticData.Invocation.DTO;
using Code.UI.Game.Finish.InvocationIcon;

namespace Code.UI.Game.Finish.Win
{
    public class WinWindowPM : IWinWindowPM
    {
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IInvocationPowerCalculationService _invocationPowerCalculationService;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;

        private List<InvocationIconComposite> _invocationIconComposites = new();
        
        public WinWindowPM(
            ILevelLocalProgressService levelLocalProgressService, 
            IInvocationPowerCalculationService invocationPowerCalculationService,
            IUIFactory uiFactory,
            IStaticDataService staticDataService)
        {
            _levelLocalProgressService = levelLocalProgressService;
            _invocationPowerCalculationService = invocationPowerCalculationService;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
        }
        
        public event Action ClosedWindowEvent; 
        
        public float PlayerScore => _invocationPowerCalculationService.CalculatePlayerPower();
        
        public List<InvocationDTO> UsedUnits => _levelLocalProgressService.GetPlayerInvocations().Values.ToList();

        public List<InvocationIconComposite> GetInvocationCompositeCollection()
        {
            Dictionary<string, InvocationDTO> invocationDtoCollection = _levelLocalProgressService.GetPlayerInvocations();

            foreach (KeyValuePair<string, InvocationDTO> invocationDto in invocationDtoCollection)
            {
                InvocationIconView invocationIconView = _uiFactory.CreateInvocationIconView();
                invocationIconView.transform.SetParent(_uiFactory.UIRoot);
                IInvocationIconPM invocationIconPm = new InvocationIconPM(invocationDto.Value, _staticDataService);
                InvocationIconComposite invocationIconComposite = new InvocationIconComposite(invocationIconView, invocationIconPm);
                _invocationIconComposites.Add(invocationIconComposite);
            }

            return _invocationIconComposites;
        }

        public void OnCloseWindow()
        {
            ClosedWindowEvent?.Invoke();
        }

        public void Cleanup()
        {
            _invocationIconComposites.Clear();
        }
    }
}
