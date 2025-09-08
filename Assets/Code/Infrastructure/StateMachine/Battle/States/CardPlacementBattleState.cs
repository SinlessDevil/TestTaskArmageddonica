using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CardPlacementBattleState : IState, IBattleState, IUpdatable
    {
        private readonly IGridInputService _gridInputService;
        private readonly ICardInputService _cardInputService;

        public CardPlacementBattleState(
            IGridInputService gridInputService,
            ICardInputService cardInputService)
        {
            _gridInputService = gridInputService;
            _cardInputService = cardInputService;
        }
        
        public void Enter()
        {
            _gridInputService.Enable();
            _cardInputService.Enable(TypeInput.Drag);
        }

        public void Exit()
        {
            _gridInputService.Disable();
            _cardInputService.Disable();
        }


        public void Update()
        {
            
        }
    }
}