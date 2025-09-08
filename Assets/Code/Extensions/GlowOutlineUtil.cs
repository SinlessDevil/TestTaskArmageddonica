using UnityEngine;

namespace Code.Extensions
{
    public static class GlowOutlineUtil
    {
        private const string GlowEnabledProp = "_GlowEnabled";
        private const string PulseSpeedProp = "_PulseSpeed";
        private const string PulseKeyProp = "_PulseKey";
        
        public static void SetGlowEnabled(this Material mat, bool on, bool resetPhase = true)
        {
            if (!mat) 
                return;

            mat.SetFloat(GlowEnabledProp, on ? 1f : 0f);

            if (on && resetPhase)
                mat.SetFloat(PulseKeyProp, Time.time);
        }
    }
}