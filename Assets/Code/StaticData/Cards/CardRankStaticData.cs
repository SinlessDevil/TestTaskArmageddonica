using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.StaticData.Cards
{
    [CreateAssetMenu(fileName = "CardRankStaticData", menuName = "StaticData/CardRank", order = 0)]
    public class CardRankStaticData : SerializedScriptableObject
    {
        public Dictionary<CardRankType, CardRankData> CardDataRankCollection = new();
        
        public CardRankData? GetCardDataByRank(CardRankType rankType)
        {
            if (CardDataRankCollection.TryGetValue(rankType, out var cardData))
                return cardData;
            
            Debug.LogError($"Card data for rank {rankType} not found!");
            return null;
        }
    }
}