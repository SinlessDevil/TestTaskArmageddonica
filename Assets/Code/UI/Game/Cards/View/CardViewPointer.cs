using Code.Services.Input.Card;
using Code.UI.Game.Cards.PM;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.UI.Game.Cards.View
{
    public class CardViewPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, 
        IPointerUpHandler
    {
        private CardView _view;
        private ICardPM _cardPM;
        private ICardInputService _cardInputService;
        
        [Inject]
        public void Constructor(ICardInputService cardInputService)
        {
            _cardInputService = cardInputService;
        }

        public void Initialize(CardView view, ICardPM cardPM)
        {
            _view = view;
            _cardPM = cardPM;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _cardInputService.PointerEnter(_view, _cardPM);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cardInputService.PointerExit(_view, _cardPM);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _cardInputService.PointerDown(_view, _cardPM);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _cardInputService.PointerUp(_view, _cardPM);
        }
    }
}