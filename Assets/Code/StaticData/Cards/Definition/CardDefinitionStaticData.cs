using UnityEngine;

namespace Code.StaticData.Cards
{
    [CreateAssetMenu(fileName = "CardDefinitionStaticData", menuName = "StaticData/Cards/CardDefinitionStaticData")]
    public class CardDefinitionStaticData : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public CardDefinitionType Type;
    }
}