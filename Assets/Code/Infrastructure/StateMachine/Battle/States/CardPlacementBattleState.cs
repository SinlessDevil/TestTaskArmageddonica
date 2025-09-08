using Code.Infrastructure.StateMachine.Game.States;
using Code.Services.CameraController;
using Code.Services.Factories.UIFactory;
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

        public CardPlacementBattleState(
            IGridInputService gridInputService,
            ICardInputService cardInputService,
            IUIFactory uiFactory,
            ICameraDirector cameraDirector)
        {
            _gridInputService = gridInputService;
            _cardInputService = cardInputService;
            _uiFactory = uiFactory;
            _cameraDirector = cameraDirector;
        }
        
        public void Enter()
        {
            _gridInputService.Enable();
            _cardInputService.Enable(TypeInput.Drag);

            _cameraDirector.FocusSelectedShotAsync();
            
            CardHolder.Show();
        }

        public void Exit()
        {
            _gridInputService.Disable();
            _cardInputService.Disable();
        }

        public void Update()
        {
            
        }
        
        private CardHolder CardHolder => _uiFactory.GameHud.CardHolder;
    }
}