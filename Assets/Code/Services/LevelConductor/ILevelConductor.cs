using System;
using System.Collections.Generic;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.LevelConductor
{
    public interface ILevelConductor
    {
        event Action RunnedBattleEvent;
        event Action EndedBattleEvent;
        event Action ChangedPowerPlayerEvent;
        event Action ChangedPowerEnemyEvent;
        event Action ChangedWaveEvent;
        
        void RunBattle();
        void EndBattle();
        void Cleanup();
        
        void AddWave();
        int GetCurrentWave { get; }
        int GetMaxWaves { get; }
        
        void AddInvocationForPlayer(InvocationDTO dto);
        void AddInvocationForEnemy(InvocationDTO dto);
        
        InvocationDTO GetInvocationForPlayer(string uniqueId);
        InvocationDTO GetInvocationForEnemy(string uniqueId);
        
        Dictionary<string, InvocationDTO> GetPlayerInvocations();
        Dictionary<string, InvocationDTO> GetEnemyInvocations();
    }
}