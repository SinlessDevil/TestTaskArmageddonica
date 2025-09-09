using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.StaticData.Cards.Definition
{
    [CreateAssetMenu(fileName = "CardDefinitionCollectionStaticData", menuName = "StaticData/Cards/CardDefinitionCollection")]
    public class CardDefinitionCollectionStaticData : SerializedScriptableObject
    {
        public Dictionary<CardDefinitionType, CardDefinitionStaticData> CardDefinitionStaticData;
        
        public CardDefinitionStaticData? GetCardDefinitionByType(CardDefinitionType cardDefenition)
        {
            if (CardDefinitionStaticData.TryGetValue(cardDefenition, out var cardData))
                return cardData;
            
            Debug.LogError($"Card data for rank {cardDefenition} not found!");
            return null;
        }
    }   
}