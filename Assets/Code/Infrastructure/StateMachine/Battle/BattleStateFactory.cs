using System;
using System.Collections.Generic;
using Code.Infrastructure.StateMachine.Battle.States;
using Code.Infrastructure.StateMachine.Game.States;
using Zenject;

namespace Code.Infrastructure.StateMachine.Battle
{
    public class BattleStateFactory : StateFactory
    {
        public BattleStateFactory(DiContainer container) : base(container)
        {
        }

        protected override Dictionary<Type, Func<IExitable>> BuildStatesRegister(DiContainer container)
        {
            return new Dictionary<Type, Func<IExitable>>
            {
                [typeof(InitializeBattleState)] = container.Resolve<InitializeBattleState>,
                [typeof(CardSelectionBattleState)] = container.Resolve<CardSelectionBattleState>,
                [typeof(CardPlacementBattleState)] = container.Resolve<CardPlacementBattleState>,
                [typeof(PlayBattleState)] = container.Resolve<PlayBattleState>,
                [typeof(CleanupBattleState)] = container.Resolve<CleanupBattleState>,
                [typeof(DisposeBattleState)] = container.Resolve<DisposeBattleState>,
            };
        }
    }
}