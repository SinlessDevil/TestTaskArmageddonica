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
                [typeof(CardSelectionState)] = container.Resolve<CardSelectionState>,
                [typeof(CardSelectionState)] = container.Resolve<CardPlacementState>,
                [typeof(PlayBattleState)] = container.Resolve<PlayBattleState>,
                [typeof(PauseBattleState)] = container.Resolve<PauseBattleState>,
            };
        }
    }
}