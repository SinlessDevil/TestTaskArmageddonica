using Code.StaticData.Cards;
using Code.StaticData.Invocation.Data.Skill;
using UnityEngine;

namespace Code.StaticData.Invocation.DTO
{
    public class BuildDTO : InvocationDTO
    {
        public float Defense = 5f;
        public float Damage = 5f;
        public SkillData Skill;
        
        public BuildDTO(string id, string uniqueId, GameObject prefab, CardRankType rank, 
            CardDefinitionType cardDefinition, InvocationType invocationType, int quantity,
            float defense, float damage, SkillData skill) : 
            base(id, uniqueId, prefab, rank, cardDefinition, invocationType, quantity)
        {
            Defense = defense;
            Damage = damage;
            Skill = skill;
        }
    }
}