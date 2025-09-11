using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.StaticData.Battle
{
    [CreateAssetMenu(fileName = "BattleStaticData", menuName = "StaticData/Battle/Battle Static Data")]
    public class BattleStaticData : ScriptableObject
    {
        [Header("Battle Data List")]
        [SerializeField] private List<BattleData> _battleDataList = new();
        
        public List<BattleData> BattleDataList => _battleDataList;
        
        public void AddBattleData(BattleData battleData)
        {
            if (!_battleDataList.Contains(battleData))
            {
                _battleDataList.Add(battleData);
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }
        
        public void RemoveBattleData(BattleData battleData)
        {
            if (_battleDataList.Contains(battleData))
            {
                _battleDataList.Remove(battleData);
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }
    }
}
