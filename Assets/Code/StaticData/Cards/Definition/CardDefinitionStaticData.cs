using UnityEngine;

namespace Code.StaticData.Cards
{
    [CreateAssetMenu(fileName = "CardDefinitionStaticData", menuName = "StaticData/Cards/CardDefinitionStaticData")]
    public class CardDefinitionStaticData : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _cost;
        [SerializeField] private CardDefinitionType _type;

        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public GameObject Prefab => _prefab;
        public int Cost => _cost;
        public CardDefinitionType Type => _type;
    }
}