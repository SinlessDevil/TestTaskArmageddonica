using System;
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
using Code.StaticData.Invocation.DTO;
using Code.UI.Game.Cards.Holder;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;

namespace Code.Infrastructure.StateMachine.Battle.States
{
    public class CardPlacementBattleState : IState, IBattleState, IUpdatable
    {
        private readonly IGridInputService _gridInputService;
        private readonly IDragCardInputService _dragCardInputService;
        private readonly IUIFactory _uiFactory;
        private readonly ICameraDirector _cameraDirector;
        private readonly IInvocationFactory _invocationFactory;
        private readonly ILevelConductor _levelConductor;
        private readonly IStateMachine<IBattleState> _stateMachine;
        private readonly IStaticDataService _staticDataService;

        public CardPlacementBattleState(
            IGridInputService gridInputService,
            IDragCardInputService dragCardInputService,
            IUIFactory uiFactory,
            ICameraDirector cameraDirector,
            IStateMachine<IBattleState> stateMachine,
            IStaticDataService staticDataService,
            IInvocationFactory invocationFactory,
            ILevelConductor levelConductor)
        {
            _gridInputService = gridInputService;
            _dragCardInputService = dragCardInputService;
            _uiFactory = uiFactory;
            _cameraDirector = cameraDirector;
            _invocationFactory = invocationFactory;
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _levelConductor = levelConductor;
        }
        
        public void Enter()
        {
            _dragCardInputService.Enable();
            _gridInputService.Enable();
            _gridInputService.DroppedInvocationInCellEvent += OnDragCardDroppedOnCell;
            
            _cameraDirector.FocusSelectedShotAsync();
            
            CardHolder.Show();
        }

        public void Exit()
        {
            _dragCardInputService.Disable();
            _gridInputService.Disable();
            _gridInputService.DroppedInvocationInCellEvent -= OnDragCardDroppedOnCell;
        }

        public void Update()
        {
            
        }

        private void OnDragCardDroppedOnCell(InvocationDTO dto, Cell targetCell)
        {
            Invocation invocation = _invocationFactory.CreateInvocationByType(dto, targetCell, HeadRotation.PlayerRotation);
            targetCell.InvocationController.AddInvocation(invocation, dto.InvocationType, dto.Id);
            _levelConductor.AddInvocationForPlayer(dto);

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