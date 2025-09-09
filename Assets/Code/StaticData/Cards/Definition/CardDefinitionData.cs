using UnityEngine;

namespace Code.StaticData.Cards
{
    [System.Serializable]
    public struct CardDefinitionData
    {
        public string Title;
        [TextArea] public string Description;
        public Sprite Icon;
    }
}