using System;
using UnityEngine;

namespace Code.StaticData.Levels
{
    [CreateAssetMenu(fileName = "LevelStaticData", menuName = "StaticData/Level", order = 0)]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelName;
        public int LevelId;
        public LevelTypeId LevelTypeId;
        public GridData GridData;
    }

    [Serializable]
    public enum LevelTypeId
    {
        Regular = 0, 
        Special = 1,
        Bonus = 2,
    }

    [Serializable]
    public struct GridData
    {
        public int Columns;
        public int Rows;
    }
}