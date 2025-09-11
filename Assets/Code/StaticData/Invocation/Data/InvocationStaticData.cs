using Code.StaticData.Cards;
using UnityEngine;

namespace Code.StaticData.Invocation.Data
{
    [CreateAssetMenu(fileName = "InvocationStaticData", menuName = "StaticData/InvocationFactory", order = 0)]
    public class InvocationStaticData : ScriptableObject 
    {
        [Header("Basic Info")]
        public string Id;
        public GameObject Prefab;
        public CardRankType Rank;
        public CardDefinitionType CardDefinition;
        public InvocationType InvocationType;
    }
}