using UnityEngine;
using Code.StaticData.Battle;

namespace Code.StaticData.Levels
{
    [CreateAssetMenu(fileName = "LevelStaticData", menuName = "StaticData/Level", order = 0)]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelName;
        public int LevelId;
        public LevelTypeId LevelTypeId;
        public GridData GridData;
        
        [Header("Battle Data")]
        public BattleStaticData BattleStaticData;
    }
}