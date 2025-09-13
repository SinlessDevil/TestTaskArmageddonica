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
        private readonly ICardSelectionService _cardSelectionWindowService;
        private readonly IStateMachine<IBattleState> _stateMachine;
        private readonly ISelectionCardInputService _selectionCardInputService;
        private readonly IUIFactory _uiFactory;

        public CardSelectionBattleState(
            ICardSelectionService cardSelectionWindowService,
            IStateMachine<IBattleState> stateMachine,
            ISelectionCardInputService selectionCardInputService,
            IUIFactory uiFactory)
        {
            _cardSelectionWindowService = cardSelectionWindowService;
            _stateMachine = stateMachine;
            _selectionCardInputService = selectionCardInputService;
            _uiFactory = uiFactory;
        }

        void IState.Enter()
        {
            _selectionCardInputService.Enable();
            
            _cardSelectionWindowService.Open();
            _cardSelectionWindowService.ClosedWindowEvent += OnClosedWindow;
            
            CardHolder.Hide();
        }

        void IExitable.Exit()
        {
            _selectionCardInputService.Disable();
            
            _cardSelectionWindowService.ClosedWindowEvent -= OnClosedWindow;
            _cardSelectionWindowService.Close();
        }

        private void OnClosedWindow()
        {
            _stateMachine.Enter<CardPlacementBattleState>();
        }
        
        private CardHolder CardHolder => _uiFactory.GameHud.CardHolder;
    }
}