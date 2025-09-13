using Code.StaticData.Invocation.Data.Skill;
using UnityEngine;

namespace Code.StaticData.Invocation.Data
{
    [CreateAssetMenu(fileName = "BuildStaticData", menuName = "StaticData/Invocation/Build", order = 0)]
    public class BuildStaticData : InvocationStaticData
    {
        [Header("Building Specific Stats")]
        public float Defense = 5f;
        public float Damage = 5f;
        public SkillData Skill = new();
    }
}