using Code.StaticData.Cards;
using UnityEngine;

namespace Code.StaticData.Invocation.DTO
{
    public class SkillDTO : InvocationDTO
    {
        public SkillDTO(
            string id, 
            string uniqueId,
            GameObject prefab, 
            CardRankType rank, 
            CardDefinitionType cardDefinition, 
            InvocationType invocationType) : base(id, uniqueId, prefab, rank, cardDefinition, invocationType)
        {
        }
    }
}