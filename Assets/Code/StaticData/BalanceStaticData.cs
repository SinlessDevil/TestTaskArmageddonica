using Code.StaticData.Cards;
using Code.StaticData.Cards.Definition;
using Code.StaticData.СameraShots;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Balance", fileName = "Balance", order = 0)]
    public class BalanceStaticData : SerializedScriptableObject
    {
        [InlineEditor(Expanded = true, DrawHeader = true)] public CardRankStaticData CardRankStaticData;
        [InlineEditor(Expanded = true, DrawHeader = true)] public CardDefinitionCollectionStaticData CardDefinitionCollectionStaticData;
        [InlineEditor(Expanded = true, DrawHeader = true)] public CameraShotStaticData CameraShotStaticData;
    }
}