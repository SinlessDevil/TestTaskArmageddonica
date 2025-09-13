using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Game.Finish.InvocationIcon
{
    public class InvocationIconView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private RectTransform _rectTransform;
        [Header("Animation")]
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private Ease _animationEase = Ease.OutBack;
        [SerializeField] private float _delayBetweenIcons = 0.1f;
        
        public void Initialize(IInvocationIconPM invocationIconPm)
        {
            _nameText.text = invocationIconPm.GetName();
            _iconImage.sprite = invocationIconPm.GetSprite();
            _quantityText.text = invocationIconPm.GetQuantity().ToString();
        }
        
        public void ShowWithAnimation(float delay = 0f)
        {
            Reset();
            
            _rectTransform.DOScale(Vector3.one, _animationDuration)
                .SetDelay(delay)
                .SetEase(_animationEase);
        }
        
        private void Reset()
        {
            _rectTransform.localScale = Vector3.zero;
        }
    }
}