using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Code.UI.Game.Cards.PM;

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
        [SerializeField] private CardViewPointer _pointerComponent;
        
        private ICardPM _cardPM;

        public void Initialize(ICardPM cardPM)
        {
            _cardPM = cardPM;
            
            _pointerComponent.Initialize(this, _cardPM);
            
            SetupCardComponents();
        }
        
        public void Dispose()
        {
            _cardPM = null;
            
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;

            _hoverComponent.ResetState();
            
            ResetCardComponents();
        }

        public RectTransform Root => _root;
        
        public CardViewHover HoverComponent => _hoverComponent;
        
        public ICardPM CardPM => _cardPM;
        
        private void SetupCardComponents()
        {
            SetupIcon();
            SetupTexts();
            SetupBackgrounds();
            SetupLevel();
        }
        
        private void SetupIcon()
        {
            _iconImage.sprite = _cardPM.CardIcon;
            _iconImage.color = Color.white;
        }
        
        private void SetupTexts()
        {
            _levelText.text = _cardPM.RankText;
            _nameText.text = _cardPM.CardName;
            _infoText.text = _cardPM.CardDescription;
        }
        
        private void SetupBackgrounds()
        {
            _bgImage.sprite = _cardPM.BackgroundSprite;
            
            _BodyImage.color = _cardPM.RankColor;
        }
        
        private void SetupLevel()
        {
            _bgLevelImage.sprite = _cardPM.LevelBackgroundSprite;
        }
        
        private void ResetCardComponents()
        {
            _iconImage.sprite = null;
            _iconImage.color = Color.white;
            
            _nameText.text = string.Empty;
            _nameText.color = Color.white;
            
            _infoText.text = string.Empty;
            _levelText.text = string.Empty;
            
            _bgImage.sprite = null;
            _bgImage.color = Color.white;
            
            _BodyImage.sprite = null;
            _BodyImage.color = Color.white;
            
            _bgLevelImage.sprite = null;
            _bgLevelImage.color = Color.white;
        }
        
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