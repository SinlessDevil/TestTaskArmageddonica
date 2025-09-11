using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.CameraController;
using Code.Services.Factories.UIFactory;
using Code.Services.IInvocation.InvocationHandler;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;
using Code.UI.Game.Cards.Holder;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CardPlacementBattleState : IState, IBattleState, IUpdatable
    {
        private readonly IGridInputService _gridInputService;
        private readonly ICardInputService _cardInputService;
        private readonly IUIFactory _uiFactory;
        private readonly ICameraDirector _cameraDirector;
        private readonly IInvocationHandlerService _invocationHandlerService;
        private readonly IStateMachine<IBattleState> _stateMachine;

        public CardPlacementBattleState(
            IGridInputService gridInputService,
            ICardInputService cardInputService,
            IUIFactory uiFactory,
            ICameraDirector cameraDirector,
            IInvocationHandlerService invocationHandlerService,
            IStateMachine<IBattleState> stateMachine)
        {
            _gridInputService = gridInputService;
            _cardInputService = cardInputService;
            _uiFactory = uiFactory;
            _cameraDirector = cameraDirector;
            _invocationHandlerService = invocationHandlerService;
            _stateMachine = stateMachine;
        }
        
        public void Enter()
        {
            _gridInputService.Enable();
            
            _cardInputService.Disable();
            _cardInputService.Enable(TypeInput.Drag);

            _cameraDirector.FocusSelectedShotAsync();
            
            CardHolder.Show();
            
            _invocationHandlerService.Initialize();
            _invocationHandlerService.InvocationSpawnedEvent += OnPlayBattleState;
        }

        public void Exit()
        {
            _invocationHandlerService.InvocationSpawnedEvent -= OnPlayBattleState;
            _invocationHandlerService.Dispose();
            
            _gridInputService.Disable();
            _cardInputService.Disable();
        }

        public void Update()
        {
            
        }

        private void OnPlayBattleState()
        {
            _stateMachine.Enter<PlayBattleState>();
        }
        
        private CardHolder CardHolder => _uiFactory.GameHud.CardHolder;
    }
}