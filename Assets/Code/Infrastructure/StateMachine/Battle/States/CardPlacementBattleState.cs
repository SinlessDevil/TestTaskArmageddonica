using Code.Infrastructure.StateMachine.Game.States;
using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Services.CameraController;
using Code.Services.Factories.UIFactory;
using Code.Services.IInvocation.Factories;
using Code.Services.Input.Card;
using Code.Services.Input.Grid;
using Code.Services.LevelConductor;
using Code.Services.Skills;
using Code.Services.StaticData;
using Code.StaticData;
using Code.StaticData.Invocation.DTO;
using Code.UI.Game.Cards.Holder;

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
        private readonly ISkillExecutorService _skillExecutorService;
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
            ILevelConductor levelConductor,
            ISkillExecutorService skillExecutorService)
        {
            _gridInputService = gridInputService;
            _dragCardInputService = dragCardInputService;
            _uiFactory = uiFactory;
            _cameraDirector = cameraDirector;
            _invocationFactory = invocationFactory;
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _levelConductor = levelConductor;
            _skillExecutorService = skillExecutorService;
        }
        
        public void Enter()
        {
            _dragCardInputService.Enable();
            _gridInputService.Enable();
            _gridInputService.DroppedInvocationInCellEvent += OnDroppedInvocationInCell;
            
            _cameraDirector.FocusSelectedShotAsync();
            
            CardHolder.Show();
        }

        public void Exit()
        {
            _dragCardInputService.Disable();
            _gridInputService.Disable();
            _gridInputService.DroppedInvocationInCellEvent -= OnDroppedInvocationInCell;
        }

        public void Update()
        {
            
        }

        private void OnDroppedInvocationInCell(InvocationDTO dto, Cell targetCell)
        {
            if (dto is SkillDTO skillDTO)
                ExecuteSkill(skillDTO, targetCell);
            else
                CreateUnitOrBuild(dto, targetCell);

            PlayBattleState();
        }

        private void ExecuteSkill(SkillDTO skillDTO, Cell targetCell)
        {
            _skillExecutorService.SkillExecute(skillDTO, targetCell);
        }
        
        private void CreateUnitOrBuild(InvocationDTO dto, Cell targetCell)
        {
            Invocation invocation = _invocationFactory.CreateInvocationByType(dto, targetCell, HeadRotation.PlayerRotation);
            targetCell.InvocationController.AddInvocation(invocation, dto.InvocationType, dto.Id, dto.UniqueId);
            _levelConductor.AddInvocationForPlayer(dto);
        }

        private void PlayBattleState()
        {
            _stateMachine.Enter<PlayBattleState>();
        }
        
        private CardHolder CardHolder => _uiFactory.GameHud.CardHolder;
        
        private HeadRotation HeadRotation => _staticDataService.Balance.HeadRotation;
    }
}