using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Game.Cards
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
        
        public RectTransform Root => _root;
    }    
}