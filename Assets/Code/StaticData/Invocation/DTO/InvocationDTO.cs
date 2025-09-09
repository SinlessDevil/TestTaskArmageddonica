using Code.StaticData.Cards;
using Code.StaticData.Invocation;
using UnityEngine;

namespace Code.Services.IInvocation.DTO
{
    public class InvocationDTO
    {
        public string Id;
        public GameObject Prefab;
        public CardRankType Rank;
        public CardDefinitionType CardDefinition;
        public InvocationType InvocationType;
        
        public InvocationDTO(string id, GameObject prefab, CardRankType rank, CardDefinitionType cardDefinition,
            InvocationType invocationType)
        {
            Id = id;
            Prefab = prefab;
            Rank = rank;
            CardDefinition = cardDefinition;
            InvocationType = invocationType;
        }
    }
}