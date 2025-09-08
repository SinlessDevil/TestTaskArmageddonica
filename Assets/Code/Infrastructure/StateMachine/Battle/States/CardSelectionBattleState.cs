using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.CardSelection;
using Code.UI.Game.CardSelection;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CardSelectionBattleState : IState, IBattleState
    {
        private readonly ICardSelectionService _cardSelectionWindowService;
        private readonly IStateMachine<IBattleState> _stateMachine;
        
        private CardSelectionWindow _window;

        public CardSelectionBattleState(
            ICardSelectionService cardSelectionWindowService,
            IStateMachine<IBattleState> stateMachine)
        {
            _cardSelectionWindowService = cardSelectionWindowService;
            _stateMachine = stateMachine;
        }

        void IState.Enter()
        {
            _window = _cardSelectionWindowService.Open();
            _cardSelectionWindowService.Selected += OnSelected;
        }

        void IExitable.Exit()
        {
            _cardSelectionWindowService.Selected -= OnSelected;
            if (_window != null)
                _cardSelectionWindowService.Close(_window);
        }

        private void OnSelected()
        {
            _stateMachine.Enter<CardPlacementBattleState>();
        }
    }
}