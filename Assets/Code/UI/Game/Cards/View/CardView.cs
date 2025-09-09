using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Game.Cards.View
{
    public class CardView : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _infoText;
        [SerializeField] private TMP_Text _levelText;
        [Header("BG")]
        [SerializeField] private Image _bgImage;
        [SerializeField] private Image _BodyImage;
        [SerializeField] private Image _bgLevelImage;
        [Header("Components")] 
        [SerializeField] private RectTransform _root;
        [SerializeField] private CardViewHover _hoverComponent;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public RectTransform Root => _root;
        public CardViewHover HoverComponent => _hoverComponent;

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }
        
        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }    
}