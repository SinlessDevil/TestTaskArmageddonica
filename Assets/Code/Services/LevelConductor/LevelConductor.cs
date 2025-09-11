using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Services.Finish;
using Code.Services.Levels;
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
        
        private Dictionary<string, InvocationDTO> _playerInvocations;
        private Dictionary<string, InvocationDTO> _enemyInvocations;
        
        private int _currentGetCurrentWave = 1;
        
        public LevelConductor(
            ILevelService levelService, 
            IInvocationPowerCalculationService invocationPowerCalculationService,
            IFinishService finishService)
        {
            _levelService = levelService;
            _invocationPowerCalculationService = invocationPowerCalculationService;
            _finishService = finishService;

            _playerInvocations = new Dictionary<string, InvocationDTO>();
            _enemyInvocations = new Dictionary<string, InvocationDTO>();
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
        
        public void Cleanup()
        {
            _playerInvocations.Clear();
            _enemyInvocations.Clear();

            _currentGetCurrentWave = 1;
        }
        
        public int GetCurrentWave => _currentGetCurrentWave;
        
        public int GetMaxWaves => _levelService.GetCurrentLevelStaticData().BattleStaticData.BattleDataList.Count;
        
        public void AddWave()
        {
            _currentGetCurrentWave++;
            ChangedWaveEvent?.Invoke();
        }
        
        public void AddInvocationForPlayer(InvocationDTO dto)
        {
            if (_playerInvocations.ContainsKey(dto.UniqueId))
            {
                _playerInvocations[dto.UniqueId].Quantity++;
            }
            else
            {
                _playerInvocations[dto.UniqueId] = dto;
            }
            
            ChangedPowerPlayerEvent?.Invoke();
        }
        
        public void AddInvocationForEnemy(InvocationDTO dto)
        {
            if (_enemyInvocations.ContainsKey(dto.UniqueId))
            {
                _enemyInvocations[dto.UniqueId].Quantity++;
            }
            else
            {
                _enemyInvocations[dto.UniqueId] = dto;
            }
            
            ChangedPowerEnemyEvent?.Invoke();
        }
        
        public InvocationDTO GetInvocationForPlayer(string uniqueId) => 
            _playerInvocations.ContainsKey(uniqueId) ? _playerInvocations[uniqueId] : null;

        public InvocationDTO GetInvocationForEnemy(string uniqueId) => 
            _enemyInvocations.ContainsKey(uniqueId) ? _enemyInvocations[uniqueId] : null;

        public Dictionary<string, InvocationDTO> GetPlayerInvocations() => _playerInvocations;

        public Dictionary<string, InvocationDTO> GetEnemyInvocations() => _enemyInvocations;
        
        private async UniTask CalculationPowerOpponentsAsync()
        {
            await Task.Delay(2000);
            
            switch (_invocationPowerCalculationService.ComparePowers())
            {
                case BattlResult.Draw:
                    EndedBattleEvent?.Invoke();
                    break;
                case BattlResult.Player:
                    _enemyInvocations.Clear();
                    AddWave();
                    if (GetCurrentWave > GetMaxWaves)
                    {
                        _finishService.Win();
                    }
                    EndedBattleEvent?.Invoke();
                    break;
                case BattlResult.Enemy:
                    _finishService.Lose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }   
}