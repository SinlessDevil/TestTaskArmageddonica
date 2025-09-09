using System.Collections.Generic;
using Code.StaticData.Invocation.Data;
using UnityEngine;

namespace Code.StaticData.Invocation
{
    [CreateAssetMenu(fileName = "InvocationCollectionStaticData", menuName = "StaticData/InvocationCollection", order = 0)]
    public class InvocationCollectionStaticData : ScriptableObject
    {
        public List<UnitStaticData> UnitCollectionData;
        public List<BuildStaticData> BuildCollectionData;
        public List<SkillStaticData> SkillCollectionData;
    }   
}