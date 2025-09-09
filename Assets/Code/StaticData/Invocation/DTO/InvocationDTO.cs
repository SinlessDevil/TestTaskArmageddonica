using Code.StaticData.Cards;
using UnityEngine;

namespace Code.Services.IInvocation.DTO
{
    public class InvocationDTO
    {
        public string Id;
        public GameObject Prefab;
        public CardRankType Rank;
        public CardDefinitionType CardDefinition;
    }
}