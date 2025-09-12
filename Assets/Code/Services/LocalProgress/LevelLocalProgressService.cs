using System;
using System.Collections.Generic;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.LocalProgress
{
    public class LevelLocalProgressService : ILevelLocalProgressService
    {
        public event Action<int> UpdateScoreEvent; 
        
        public int Score { get; private set; }
        public int CurrentWave { get; private set; } = 1;
        
        private Dictionary<string, InvocationDTO> _playerInvocations;
        private Dictionary<string, InvocationDTO> _enemyInvocations;
        
        public LevelLocalProgressService()
        {
            _playerInvocations = new Dictionary<string, InvocationDTO>();
            _enemyInvocations = new Dictionary<string, InvocationDTO>();
        }
        
        public void AddScore(int score)
        {
            Score += score;
            UpdateScoreEvent?.Invoke(Score);
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

        public Dictionary<string, InvocationDTO> GetPlayerInvocations() => _playerInvocations;

        public Dictionary<string, InvocationDTO> GetEnemyInvocations() => _enemyInvocations;
        
        public void ClearEnemyInvocations()
        {
            _enemyInvocations.Clear();
        }
        
        public void AddWave()
        {
            CurrentWave++;
        }
        
        public void ResetWave()
        {
            CurrentWave = 1;
        }
        
        public void Cleanup()
        {
            Score = 0;
            CurrentWave = 1;
            _playerInvocations.Clear();
            _enemyInvocations.Clear();
        }
    }
}