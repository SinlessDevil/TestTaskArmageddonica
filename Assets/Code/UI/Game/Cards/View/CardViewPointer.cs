using Code.Services.Input.Card;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.UI.Game.Cards.View
{
    public class CardViewPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, 
        IPointerUpHandler
    {
        [SerializeField] private CardView _view;
        
        private ICardInputService _cardInputService;
        
        [Inject]
        public void Constructor(ICardInputService cardInputService)
        {
            _cardInputService = cardInputService;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _cardInputService.PointerEnter(_view);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cardInputService.PointerExit(_view);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _cardInputService.PointerDown(_view);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _cardInputService.PointerUp(_view);
        }
    }
}