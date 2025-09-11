using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Services.CameraController;
using Code.Services.Factories.UIFactory;
using Code.Services.IInvocation.Factories;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;
using Code.Services.LevelConductor;
using Code.Services.StaticData;
using Code.StaticData;
using Code.UI.Game.Cards.Holder;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;
using TypeInput = Code.Services.Input.Card.TypeInput;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CardPlacementBattleState : IState, IBattleState, IUpdatable
    {
        private readonly IGridInputService _gridInputService;
        private readonly ICardInputService _cardInputService;
        private readonly IUIFactory _uiFactory;
        private readonly ICameraDirector _cameraDirector;
        private readonly IInvocationFactory _invocationFactory;
        private readonly ILevelConductor _levelConductor;
        private readonly IStateMachine<IBattleState> _stateMachine;
        private readonly IStaticDataService _staticDataService;

        public CardPlacementBattleState(
            IGridInputService gridInputService,
            ICardInputService cardInputService,
            IUIFactory uiFactory,
            ICameraDirector cameraDirector,
            IStateMachine<IBattleState> stateMachine,
            IStaticDataService staticDataService,
            IInvocationFactory invocationFactory,
            ILevelConductor levelConductor)
        {
            _gridInputService = gridInputService;
            _cardInputService = cardInputService;
            _uiFactory = uiFactory;
            _cameraDirector = cameraDirector;
            _invocationFactory = invocationFactory;
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _levelConductor = levelConductor;
        }
        
        public void Enter()
        {
            _gridInputService.Enable();
            
            _cardInputService.Disable();
            _cardInputService.Enable(TypeInput.Drag);

            _cameraDirector.FocusSelectedShotAsync();
            
            CardHolder.Show();
            
            _cardInputService.DroppedOnCell += OnCardDroppedOnCell;
        }

        public void Exit()
        {
            _cardInputService.DroppedOnCell -= OnCardDroppedOnCell;
            
            _gridInputService.Disable();
            _cardInputService.Disable();
        }

        public void Update()
        {
            
        }

        private void OnCardDroppedOnCell(CardView cardView, ICardPM cardPM, Cell targetCell)
        {
            Invocation invocation = _invocationFactory.CreateInvocationByType(cardPM.DTO, targetCell, HeadRotation.PlayerRotation);
            targetCell.SetInvocation(invocation);
            _levelConductor.AddInvocationForPlayer(cardPM.DTO);

            PlayBattleState();
        }
        
        private void PlayBattleState()
        {
            _stateMachine.Enter<PlayBattleState>();
        }
        
        private CardHolder CardHolder => _uiFactory.GameHud.CardHolder;
        
        private HeadRotation HeadRotation => _staticDataService.Balance.HeadRotation;
    }
}