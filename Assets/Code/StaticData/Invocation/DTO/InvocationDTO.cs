using Code.StaticData.Cards;
using UnityEngine;

namespace Code.StaticData.Invocation.DTO
{
    public class InvocationDTO
    {
        public string Id;
        public string UniqueId;
        public GameObject Prefab;
        public CardRankType Rank;
        public CardDefinitionType CardDefinition;
        public InvocationType InvocationType;
        public int Quantity = 1; // Количество юнитов/зданий/навыков
        
        public InvocationDTO(
            string id,
            string uniqueId,
            GameObject prefab, 
            CardRankType rank, 
            CardDefinitionType cardDefinition,
            InvocationType invocationType,
            int quantity = 1)
        {
            Id = id;
            UniqueId = uniqueId;
            Prefab = prefab;
            Rank = rank;
            CardDefinition = cardDefinition;
            InvocationType = invocationType;
            Quantity = quantity;
        }
    }
}