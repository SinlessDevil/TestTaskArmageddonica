using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Services.Context;
using Code.Services.Finish;
using Code.Services.Levels;
using Code.Services.LocalProgress;
using Code.Services.PowerCalculation;
using Code.StaticData.Invocation.DTO;
using Cysharp.Threading.Tasks;

namespace Code.Services.LevelConductor
{
    public class LevelConductor : ILevelConductor
    {
        private readonly ILevelService _levelService;
        private readonly IInvocationPowerCalculationService _invocationPowerCalculationService;
        private readonly IFinishService _finishService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IGameContext _gameContext;

        public LevelConductor(
            ILevelService levelService, 
            IInvocationPowerCalculationService invocationPowerCalculationService,
            IFinishService finishService,
            ILevelLocalProgressService levelLocalProgressService,
            IGameContext gameContext)
        {
            _levelService = levelService;
            _invocationPowerCalculationService = invocationPowerCalculationService;
            _finishService = finishService;
            _levelLocalProgressService = levelLocalProgressService;
            _gameContext = gameContext;
        }

        public event Action RunnedBattleEvent;
        
        public event Action EndedBattleEvent;
        
        public event Action ChangedPowerPlayerEvent;
        
        public event Action ChangedPowerEnemyEvent;
        public event Action ChangedWaveEvent;

        public void RunBattle()
        {
            RunnedBattleEvent?.Invoke();
            
            CalculationPowerOpponentsAsync().Forget();
        }
        
        public int GetCurrentWave => _levelLocalProgressService.CurrentWave;
        
        public int GetMaxWaves => _levelService.GetCurrentLevelStaticData().BattleStaticData.BattleDataList.Count;
        
        public void AddWave()
        {
            _levelLocalProgressService.AddWave();
            ChangedWaveEvent?.Invoke();
        }
        
        public void AddInvocationForPlayer(InvocationDTO dto)
        {
            _levelLocalProgressService.AddInvocationForPlayer(dto);
            ChangedPowerPlayerEvent?.Invoke();
        }
        
        public void AddInvocationForEnemy(InvocationDTO dto)
        {
            _levelLocalProgressService.AddInvocationForEnemy(dto);
            ChangedPowerEnemyEvent?.Invoke();
        }
        
        public InvocationDTO GetInvocationForPlayer(string uniqueId) => 
            _levelLocalProgressService.GetInvocationForPlayer(uniqueId);

        public InvocationDTO GetInvocationForEnemy(string uniqueId) => 
            _levelLocalProgressService.GetInvocationForEnemy(uniqueId);

        public Dictionary<string, InvocationDTO> GetPlayerInvocations() => 
            _levelLocalProgressService.GetPlayerInvocations();

        public Dictionary<string, InvocationDTO> GetEnemyInvocations() => 
            _levelLocalProgressService.GetEnemyInvocations();
        
        private async UniTask CalculationPowerOpponentsAsync()
        {
            await Task.Delay(3000);
            
            switch (_invocationPowerCalculationService.ComparePowers())
            {
                case BattlResult.Draw:
                    EndedBattleEvent?.Invoke();
                    break;
                case BattlResult.Player:
                    _levelLocalProgressService.ClearEnemyInvocationsDTO();
                    AddWave();
                    if (GetCurrentWave > GetMaxWaves)
                    {
                        _gameContext.EnemyGird.GridAnimator.PlayBattleAnimation();
                        await Task.Delay(3000);
                        _finishService.Win();
                    }
                    else
                    {
                        EndedBattleEvent?.Invoke();   
                    }
                    break;
                case BattlResult.Enemy:
                    _gameContext.PlayerGrid.GridAnimator.PlayBattleAnimation();
                    await Task.Delay(3000);
                    _finishService.Lose();
                    break;
            }
        }
    }   
}