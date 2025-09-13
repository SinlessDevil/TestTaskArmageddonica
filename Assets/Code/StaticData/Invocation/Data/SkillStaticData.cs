using Code.StaticData.Invocation.Data.Skill;
using UnityEngine;

namespace Code.StaticData.Invocation.Data
{
    [CreateAssetMenu(fileName = "SkillStaticData", menuName = "StaticData/Invocation/Skill", order = 0)]
    public class SkillStaticData : InvocationStaticData
    {
        public SkillData Skill;
    }
}