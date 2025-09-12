using Code.Logic.Grid;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Grid;
using Code.Services.Providers.CardComposites;
using Code.StaticData.Invocation.DTO;
using Code.UI.Game;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;
using UnityEngine;
using DG.Tweening;

namespace Code.Services.Input.Card.Drag
{
    public class DragCardInputService : IDragCardInputService
    {
        private CardView _dragCard;
        private RectTransform _dragRT;
        private Transform _origParent;
        private int _origSibling;
        private Vector2 _origAnchoredPos;
        private Vector3 _origScale;
        private Tween _returnTw;

        private readonly IInputService _inputService;
        private readonly IGridInputService _gridInputService;
        private readonly IUIFactory _uiFactory;
        private readonly ICardCompositeProvider _cardCompositeProvider;

        public DragCardInputService(
            IInputService inputService, 
            IGridInputService gridInputService, 
            IUIFactory uiFactory,
            ICardCompositeProvider cardCompositeProvider)
        {
            _inputService = inputService;
            _gridInputService = gridInputService;
            _uiFactory = uiFactory;
            _cardCompositeProvider = cardCompositeProvider;
        }

        public bool IsDragging {  get; private set; }
        public bool IsEnabled { get; private set; }

        public void Enable()
        {
            if (IsEnabled) 
                return;
            
            _inputService.InputUpdateEvent += OnUpdate;
            _gridInputService.DroppedInvocationInCellEvent += OnHandleFinishDrag;
            _gridInputService.СancelledDropInvocationInCellEvent += OnHandleCancelDrag;
            
            IsEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
                return;
            
            _inputService.InputUpdateEvent -= OnUpdate;
            _gridInputService.DroppedInvocationInCellEvent -= OnHandleFinishDrag;
            _gridInputService.СancelledDropInvocationInCellEvent -= OnHandleCancelDrag;
            
            IsEnabled = false;
        }

        public void PointerEnter(CardView view)
        {
            if (!IsEnabled)
                return;

            view.HoverComponent.HoverEnter();
        }

        public void PointerExit(CardView view)
        {
            if (!IsEnabled)
                return;

            if (IsDragging && ReferenceEquals(view, _dragCard))
                return;

            view.HoverComponent.HoverExit();
        }

        public void PointerDown(CardView view, ICardPM cardPM)
        {
            if (!IsEnabled)
                return;

            if (IsDragging)
                return;
            
            BeginDrag(view);
            
            view.HoverComponent.HighlightShrink();
            
            _gridInputService.SetInvocationDTO(cardPM.DTO);
        }

        public void PointerUp(CardView view)
        {
            if (!IsEnabled) 
                return;

            view.HoverComponent.ResetState();
        }

        private void OnUpdate()
        {
            if (!IsDragging || _dragRT == null)
                return;

            _dragRT.anchoredPosition = ScreenToCanvasLocal(_inputService.TouchPosition);
        }

        private void BeginDrag(CardView view)
        {
            if (IsDragging)
                return;
            
            IsDragging = true;

            _dragCard = view;
            _dragRT = view.Root;

            _origParent = _dragRT.parent;
            _origSibling = _dragRT.GetSiblingIndex();
            _origAnchoredPos = _dragRT.anchoredPosition;
            _origScale = _dragRT.localScale;

            GameHud.CardHolder.RemoveCard(view);

            _dragRT.SetParent(GameHud.Canvas.transform, worldPositionStays: false);

            _dragRT.DOKill();
            _dragRT.DOScale(_origScale * 1.05f, 0.08f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);

            _dragRT.anchoredPosition = ScreenToCanvasLocal(_inputService.TouchPosition);
        }

        private void OnHandleFinishDrag(InvocationDTO invocationDto, Cell cellOrNull)
        {
            IsDragging = false;
            
            _cardCompositeProvider.ReturnCardComposite(_dragCard);
            
            _dragCard = null;
            _dragRT = null;
            
            IsDragging = false;
        }

        private void OnHandleCancelDrag()
        {
            _dragRT.SetParent(_origParent);
            _dragRT.SetSiblingIndex(_origSibling);

            _returnTw?.Kill();
            _returnTw = _dragRT.DOAnchorPos(_origAnchoredPos, 0.15f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);

            _dragRT.DOScale(_origScale, 0.12f).SetUpdate(true);

            GameHud.CardHolder.AddCard(_dragCard);
            
            _dragCard = null;
            _dragRT = null;
        }
        
        private Vector2 ScreenToCanvasLocal(Vector2 screenPos)
        {
            RectTransform canvasRT = (RectTransform)Canvas.transform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, screenPos, 
                Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera, out var local);
            return local;
        }

        private GameHud GameHud => _uiFactory.GameHud;
        private Canvas Canvas => GameHud.Canvas;
    }
}