using System;
using Code.Services.Factories.UIFactory;
using Code.Services.Finish.Lose;
using Code.Services.Finish.Win;
using Code.Services.Levels;
using Code.StaticData.Levels;

namespace Code.Services.Finish
{
    public class FinishService : IFinishService
    {
        private readonly IWinService _winService;
        private readonly ILoseService _loseService;
        private readonly ILevelService _levelService;
        private readonly IUIFactory _uiFactory;

        public FinishService(
            IWinService winService, 
            ILoseService loseService,
            ILevelService levelService,
            IUIFactory uiFactory)
        {
            _winService = winService;
            _loseService = loseService;
            _levelService = levelService;
            _uiFactory = uiFactory;
        }

        public void Win()
        {
            _uiFactory.GameHud?.Dispose();
            
            switch (_levelService.GetCurrentLevelStaticData().LevelTypeId)
            {
                case LevelTypeId.Regular:
                    _winService.Win();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Lose()
        {
            _uiFactory.GameHud?.Dispose();
            
            _loseService.Lose();
        }
    }
}
