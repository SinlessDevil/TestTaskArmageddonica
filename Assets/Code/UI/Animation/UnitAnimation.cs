using DG.Tweening;
using UnityEngine;

namespace Code.UI.Animation
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private ParticleSystem _spawnPartical;
        [SerializeField] private ParticleSystem _attackBufPartical;
        [SerializeField] private ParticleSystem _healthBufParticleSystem;
        [SerializeField] private ParticleSystem _speedBufPartricleSystem;
        [Header("Animator Serring")]
        [SerializeField] private float _duration = 0.5f;
        
        private Vector3 _saveScale;

        public void Initialize()
        {
            Setup();
        }
        
        private void Setup()
        {
            _saveScale = _root.localScale;
            _root.localScale = Vector3.zero;
        }
        
        public void PlaySpawn()
        {
            _spawnPartical.Play();

            _root.DOScale(_saveScale, _duration)
                .SetEase(Ease.OutBounce);
        }

        public void PlayAttackBuf()
        {
            _attackBufPartical.Play();
        }
        
        public void PlayHealthBuf()
        {
            _healthBufParticleSystem.Play();
        }
        
        public void PlaySpeedBuf()
        {
            _speedBufPartricleSystem.Play();
        }
    }
}
