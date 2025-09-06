using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Code.StaticData.Cards;

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
        
        [Header("Card Data")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private int _cardLevel = 1;
        
        public CardData CardData => _cardData;
        public int CardLevel => _cardLevel;
        
        public void Initialize(CardData cardData, int level = 1)
        {
            _cardData = cardData;
            _cardLevel = level;
            UpdateCardDisplay();
        }
        
        public void UpdateCardDisplay()
        {
            if (_cardData == null) return;
            
            // Обновляем текст ранга
            if (_nameText != null)
                _nameText.text = _cardData.RankText;
            
            // Обновляем уровень
            if (_levelText != null)
                _levelText.text = _cardLevel.ToString();
            
            // Обновляем спрайты
            if (_bgImage != null && _cardData.BgSprite != null)
                _bgImage.sprite = _cardData.BgSprite;
            
            if (_BodyImage != null && _cardData.BodySprite != null)
                _BodyImage.sprite = _cardData.BodySprite;
            
            if (_bgLevelImage != null && _cardData.BgLevelSprite != null)
                _bgLevelImage.sprite = _cardData.BgLevelSprite;
            
            // Обновляем цвет
            if (_bgImage != null)
                _bgImage.color = _cardData.Color;
        }
        
        public void SetCardLevel(int level)
        {
            _cardLevel = level;
            if (_levelText != null)
                _levelText.text = _cardLevel.ToString();
        }
        
        public void SetCardData(CardData cardData)
        {
            _cardData = cardData;
            UpdateCardDisplay();
        }
    }    
}