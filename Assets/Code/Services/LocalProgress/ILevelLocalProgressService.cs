using System;
using System.Collections.Generic;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.LocalProgress
{
    public interface ILevelLocalProgressService
    {
        void AddScore(int score);
        int Score { get; }
        event Action<int> UpdateScoreEvent;
        
        // Invocation management
        void AddInvocationForPlayer(InvocationDTO dto);
        void AddInvocationForEnemy(InvocationDTO dto);
        InvocationDTO GetInvocationForPlayer(string uniqueId);
        InvocationDTO GetInvocationForEnemy(string uniqueId);
        Dictionary<string, InvocationDTO> GetPlayerInvocations();
        Dictionary<string, InvocationDTO> GetEnemyInvocations();
        void ClearEnemyInvocations();
        
        void Cleanup();
    }   
}