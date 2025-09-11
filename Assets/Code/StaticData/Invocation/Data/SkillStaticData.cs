using UnityEngine;

namespace Code.StaticData.Invocation.Data
{
    [CreateAssetMenu(fileName = "SkillStaticData", menuName = "StaticData/Invocation/Skill", order = 0)]
    public class SkillStaticData : InvocationStaticData
    {
        [Header("Skill Specific Stats")]
        public float Cooldown = 5f;
        public float ManaCost = 10f;
        public float Range = 8f;
        public float Duration = 0f;
    }
}