using Code.StaticData.Cards;
using UnityEngine;

namespace Code.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Balance", fileName = "Balance", order = 0)]
    public class BalanceStaticData : ScriptableObject
    {
        public CardRankStaticData CardRankStaticData;
    }
}