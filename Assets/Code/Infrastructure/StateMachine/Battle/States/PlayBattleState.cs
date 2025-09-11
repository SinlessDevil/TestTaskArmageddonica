using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.CameraController;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class PlayBattleState : IState, IBattleState, IUpdatable
    {
        private readonly IGridInputService _gridInputService;
        private readonly ICardInputService _cardInputService;
        private readonly ICameraDirector _cameraDirector;

        public PlayBattleState(
            IGridInputService gridInputService,
            ICardInputService cardInputService,
            ICameraDirector cameraDirector)
        {
            _gridInputService = gridInputService;
            _cardInputService = cardInputService;
            _cameraDirector = cameraDirector;
        }

        void IState.Enter()
        {
            _gridInputService.Disable();
            _cardInputService.Disable();
            _cameraDirector.FocusBattleShotAsync();
        }

        void IExitable.Exit()
        {
            
        }
        
        public void Update()
        {
            
        }
    }
}