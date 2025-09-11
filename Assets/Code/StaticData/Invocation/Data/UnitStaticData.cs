using UnityEngine;

namespace Code.StaticData.Invocation.Data
{
    [CreateAssetMenu(fileName = "UnitStaticData", menuName = "StaticData/Invocation/Unit", order = 0)]
    public class UnitStaticData : InvocationStaticData
    {
        [Header("Unit Stats")]
        public int Health = 100;
        public int Damage = 10;
        public int Speed = 5;
    }
}