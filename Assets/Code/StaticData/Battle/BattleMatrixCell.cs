using System;
using UnityEngine;

namespace Code.StaticData.Battle
{
    [Serializable]
    public class BattleMatrixCell
    {
        [SerializeField] private string _invocationId = "";
        [SerializeField] private bool _isOccupied = false;
        
        public string InvocationId => _invocationId;
        public bool IsOccupied => _isOccupied;
        
        public BattleMatrixCell()
        {
            _invocationId = "";
            _isOccupied = false;
        }
        
        public void SetInvocation(string invocationId)
        {
            _invocationId = invocationId;
            _isOccupied = !string.IsNullOrEmpty(invocationId);
        }
        
        public void Clear()
        {
            _invocationId = "";
            _isOccupied = false;
        }
    }
}