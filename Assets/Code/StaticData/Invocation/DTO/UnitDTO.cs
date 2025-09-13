using Code.StaticData.Cards;
using UnityEngine;

namespace Code.StaticData.Invocation.DTO
{
    public class UnitDTO : InvocationDTO
    {
        public int Health = 100;
        public int Damage = 10;
        public int Speed = 5;
        
        public UnitDTO(string id, string uniqueId, GameObject prefab, CardRankType rank, 
            CardDefinitionType cardDefinition, InvocationType invocationType, int quantity,
            int health, int damage, int speed) : 
            base(id, uniqueId, prefab, rank, cardDefinition, invocationType, quantity)
        {
            Health = health;
            Damage = damage;
            Speed = speed;
        }
    }
}