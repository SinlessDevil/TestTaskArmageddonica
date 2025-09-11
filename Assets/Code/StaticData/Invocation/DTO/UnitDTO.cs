using Code.StaticData.Cards;
using UnityEngine;

namespace Code.StaticData.Invocation.DTO
{
    public class UnitDTO : InvocationDTO
    {
        public UnitDTO(
            string id,
            string uniqueId,
            GameObject prefab, 
            CardRankType rank, 
            CardDefinitionType cardDefinition, 
            InvocationType invocationType,
            int quantity = 1) : base(id, uniqueId, prefab, rank, cardDefinition, invocationType, quantity)
        {
        }
    }
}