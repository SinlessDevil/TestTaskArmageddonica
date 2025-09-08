using Code.StaticData.Cards;
using UnityEngine;

namespace Code.StaticData.Invocation.Data
{
    [CreateAssetMenu(fileName = "InvocationStaticData", menuName = "StaticData/Invocation", order = 0)]
    public class InvocationStaticData : ScriptableObject 
    {
        public string Id;
        public GameObject Prefab;
        public CardRankType Rank;
        public CardDefinitionType CardDefinition;
    }
}