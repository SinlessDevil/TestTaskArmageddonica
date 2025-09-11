using System.Collections.Generic;
using Code.Services.Levels;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.LevelConductor
{
    public class LevelConductor : ILevelConductor
    {
        private readonly ILevelService _levelService;
        private Dictionary<string, InvocationDTO> _playerInvocations;
        private Dictionary<string, InvocationDTO> _enemyInvocations;
        
        private int _currentGetCurrentWave = 1;
        
        public LevelConductor(ILevelService levelService)
        {
            _levelService = levelService;
            
            _playerInvocations = new Dictionary<string, InvocationDTO>();
            _enemyInvocations = new Dictionary<string, InvocationDTO>();
        }

        public void RunBattle()
        {
            
        }
        
        public void EndBattle()
        {
            
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
        }
        
        public InvocationDTO GetInvocationForPlayer(string uniqueId) => 
            _playerInvocations.ContainsKey(uniqueId) ? _playerInvocations[uniqueId] : null;

        public InvocationDTO GetInvocationForEnemy(string uniqueId) => 
            _enemyInvocations.ContainsKey(uniqueId) ? _enemyInvocations[uniqueId] : null;

        public Dictionary<string, InvocationDTO> GetPlayerInvocations() => 
            _playerInvocations;

        public Dictionary<string, InvocationDTO> GetEnemyInvocations() => 
            _enemyInvocations;
    }   
}