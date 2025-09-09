using Code.StaticData.Cards;
using Code.StaticData.Invocation;
using UnityEngine;

namespace Code.Services.IInvocation.DTO
{
    public class UnitDTO : InvocationDTO
    {
        public UnitDTO(string id, GameObject prefab, CardRankType rank, CardDefinitionType cardDefinition, 
            InvocationType invocationType) : base(id, prefab, rank, cardDefinition, invocationType)
        {
        }
    }
}