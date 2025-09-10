using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.CardSelection;
using Code.Services.Input.Card;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CardSelectionBattleState : IState, IBattleState
    {
        private readonly ICardSelectionService _cardSelectionWindowService;
        private readonly IStateMachine<IBattleState> _stateMachine;
        private readonly ICardInputService _cardInputService;

        public CardSelectionBattleState(
            ICardSelectionService cardSelectionWindowService,
            IStateMachine<IBattleState> stateMachine,
            ICardInputService cardInputService)
        {
            _cardSelectionWindowService = cardSelectionWindowService;
            _stateMachine = stateMachine;
            _cardInputService = cardInputService;
        }

        void IState.Enter()
        {
            _cardInputService.Enable(TypeInput.Click);
            
            _cardSelectionWindowService.Open();
            _cardSelectionWindowService.ClosedWindowEvent += OnClosedWindow;
        }

        void IExitable.Exit()
        {
            _cardSelectionWindowService.ClosedWindowEvent -= OnClosedWindow;
            _cardSelectionWindowService.Close();
        }

        private void OnClosedWindow()
        {
            _stateMachine.Enter<CardPlacementBattleState>();
        }
    }
}