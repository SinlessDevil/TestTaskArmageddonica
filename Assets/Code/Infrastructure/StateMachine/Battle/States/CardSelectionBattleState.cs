using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.CardSelection;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Card;
using Code.Services.Input.Card.Select;
using Code.UI.Game.Cards.Holder;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CardSelectionBattleState : IState, IBattleState
    {
        private readonly ICardSelectionWinndowService _cardSelectionWinndowWindowService;
        private readonly IStateMachine<IBattleState> _stateMachine;
        private readonly ISelectionCardInputService _selectionCardInputService;
        private readonly IUIFactory _uiFactory;

        public CardSelectionBattleState(
            ICardSelectionWinndowService cardSelectionWinndowWindowService,
            IStateMachine<IBattleState> stateMachine,
            ISelectionCardInputService selectionCardInputService,
            IUIFactory uiFactory)
        {
            _cardSelectionWinndowWindowService = cardSelectionWinndowWindowService;
            _stateMachine = stateMachine;
            _selectionCardInputService = selectionCardInputService;
            _uiFactory = uiFactory;
        }

        void IState.Enter()
        {
            _selectionCardInputService.Enable();
            
            _cardSelectionWinndowWindowService.Open();
            _cardSelectionWinndowWindowService.ClosedWindowEvent += OnClosedWinndowWindow;
            
            CardHolder.Hide();
        }

        void IExitable.Exit()
        {
            _selectionCardInputService.Disable();
            
            _cardSelectionWinndowWindowService.ClosedWindowEvent -= OnClosedWinndowWindow;
            _cardSelectionWinndowWindowService.Close();
        }

        private void OnClosedWinndowWindow()
        {
            _stateMachine.Enter<CardPlacementBattleState>();
        }
        
        private CardHolder CardHolder => _uiFactory.GameHud.CardHolder;
    }
}