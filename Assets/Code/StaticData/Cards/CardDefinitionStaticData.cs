using System.Collections.Generic;
using UnityEngine;

namespace Code.StaticData.Cards
{
    [CreateAssetMenu(fileName = "CardDefinitionStaticData", menuName = "StaticData/CardDefinition", order = 0)]
    public class CardDefinitionStaticData : ScriptableObject
    {
        public Dictionary<CardDefinitionType, CardDefinitionData> CardDataRankCollection = new();
        
        public CardDefinitionData GetCardDataByDefinition(CardDefinitionType definitionType)
        {
            if (CardDataRankCollection.TryGetValue(definitionType, out var cardData))
                return cardData;
            
            Debug.LogError($"Card data for rank {definitionType} not found!");
            return null;
        }
    }
}