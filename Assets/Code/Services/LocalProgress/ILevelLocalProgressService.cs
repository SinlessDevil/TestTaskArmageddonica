using System.Collections.Generic;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.LocalProgress
{
    public interface ILevelLocalProgressService
    {
        void AddInvocationForPlayer(InvocationDTO dto);
        void AddInvocationForEnemy(InvocationDTO dto);
        InvocationDTO GetInvocationForPlayer(string uniqueId);
        InvocationDTO GetInvocationForEnemy(string uniqueId);
        Dictionary<string, InvocationDTO> GetPlayerInvocations();
        Dictionary<string, InvocationDTO> GetEnemyInvocations();
        void ClearEnemyInvocationsDTO();
        
        int CurrentWave { get; }
        void AddWave();
        void ResetWave();
        
        void Cleanup();
    }   
}