using System.Linq;
using System.Collections.Generic;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.LevelConductor
{
    public class LevelConductor : ILevelConductor
    {
        private Dictionary<string, InvocationDTO> _playerInvocations;
        
        private Dictionary<string, InvocationDTO> _enemyInvocations;
        
        public LevelConductor()
        {
            
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
        }
        
        public void AddInvocationForPlayer(InvocationDTO dto)
        {
            
        }
        
        public void AddInvocationForEnemy(InvocationDTO dto)
        {
            
        }
        
        public InvocationDTO GetInvocationForPlayer(string uniqueId)
        {
            return _playerInvocations.Values.FirstOrDefault(dto => dto.UniqueId == uniqueId);
        }
        
        public InvocationDTO GetInvocationForEnemy(string uniqueId)
        {
            return _enemyInvocations.Values.FirstOrDefault(dto => dto.UniqueId == uniqueId);
        }
    }   
}