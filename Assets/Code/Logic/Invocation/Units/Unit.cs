using Code.UI.Animation;
using UnityEngine;

namespace Code.Logic.Invocation.Units
{
    public class Unit : Invocation
    {
        [SerializeField] private UnitAnimation _unitAnimation;
        
        public void Initialize()
        {
            _unitAnimation.Initialize();
            _unitAnimation.PlaySpawn();
        }
    }
}