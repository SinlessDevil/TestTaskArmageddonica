using Code.Services.Input.Card;
using Code.Services.Input.Card.Select;
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
        
        private IDragCardInputService _dragCardInputService;
        private ISelectionCardInputService _selectionCardInputService;
        
        [Inject]
        public void Constructor(
            IDragCardInputService dragCardInputService,
            ISelectionCardInputService selectionCardInputService)
        {
            _dragCardInputService = dragCardInputService;
            _selectionCardInputService = selectionCardInputService;
        }

        public void Initialize(CardView view, ICardPM cardPM)
        {
            _view = view;
            _cardPM = cardPM;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _dragCardInputService.PointerEnter(_view);
            _selectionCardInputService.PointerEnter(_view);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _dragCardInputService.PointerExit(_view);
            _selectionCardInputService.PointerExit(_view);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _dragCardInputService.PointerDown(_view, _cardPM);
            _selectionCardInputService.PointerDown(_view);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _dragCardInputService.PointerUp(_view);
            _selectionCardInputService.PointerUp(_view);
        }
    }
}