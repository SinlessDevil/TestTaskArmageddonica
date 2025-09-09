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
            if (_iconImage != null && _cardPM.CardIcon != null)
            {
                _iconImage.sprite = _cardPM.CardIcon;
                _iconImage.color = Color.white;
            }
        }
        
        private void SetupTexts()
        {
            if (_nameText != null)
            {
                _nameText.text = _cardPM.CardName;
                _nameText.color = _cardPM.RankColor;
            }
            
            if (_infoText != null)
            {
                _infoText.text = _cardPM.CardDescription;
            }
        }
        
        private void SetupBackgrounds()
        {
            if (_bgImage != null && _cardPM.BackgroundSprite != null)
            {
                _bgImage.sprite = _cardPM.BackgroundSprite;
                _bgImage.color = _cardPM.RankColor;
            }
            
            if (_BodyImage != null && _cardPM.BodySprite != null)
            {
                _BodyImage.sprite = _cardPM.BodySprite;
                _BodyImage.color = Color.white;
            }
        }
        
        private void SetupLevel()
        {
            if (_levelText != null)
            {
                _levelText.text = _cardPM.Level.ToString();
            }
            
            if (_bgLevelImage != null && _cardPM.LevelBackgroundSprite != null)
            {
                _bgLevelImage.sprite = _cardPM.LevelBackgroundSprite;
                _bgLevelImage.color = _cardPM.RankColor;
            }
        }
        
        private void ResetCardComponents()
        {
            if (_iconImage != null)
            {
                _iconImage.sprite = null;
                _iconImage.color = Color.white;
            }
            
            if (_nameText != null)
            {
                _nameText.text = string.Empty;
                _nameText.color = Color.white;
            }
            
            if (_infoText != null)
            {
                _infoText.text = string.Empty;
            }
            
            if (_levelText != null)
            {
                _levelText.text = string.Empty;
            }
            
            if (_bgImage != null)
            {
                _bgImage.sprite = null;
                _bgImage.color = Color.white;
            }
            
            if (_BodyImage != null)
            {
                _BodyImage.sprite = null;
                _BodyImage.color = Color.white;
            }
            
            if (_bgLevelImage != null)
            {
                _bgLevelImage.sprite = null;
                _bgLevelImage.color = Color.white;
            }
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