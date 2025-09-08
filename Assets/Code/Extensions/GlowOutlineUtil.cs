using UnityEngine;

namespace Code.Extensions
{
    public static class GlowOutlineUtil
    {
        private const string GlowEnabledProp = "_GlowEnabled";
        private const string PulseKeyProp = "_PulseKey";
        
        public static void SetGlowEnabled(this Material material, bool flag, bool resetPhase = true)
        {
            if (!material) 
                return;

            material.SetFloat(GlowEnabledProp, flag ? 1f : 0f);

            if (flag && resetPhase)
                material.SetFloat(PulseKeyProp, Time.time);
        }
    }
}