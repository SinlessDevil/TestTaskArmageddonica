using UnityEngine;

namespace Code.Logic.Invocations.Units
{
    public class Unit : Invocation
    {
        [SerializeField] private UnitAnimation _unitAnimation;
        
        public void Initialize()
        {
            _unitAnimation.Initialize();
            _unitAnimation.PlaySpawn();
        }

        public UnitAnimation UnitAnimation => _unitAnimation;
    }
}