using System;
using Code.Logic.Grid;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Grid;
using Code.UI.Game;
using Code.UI.Game.Cards;
using UnityEngine;
using DG.Tweening;

namespace Code.Services.Input.Card
{
    public class CardInputService : ICardInputService
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
        
        public CardInputService(
            IInputService inputService, 
            IGridInputService gridInputService, 
            IUIFactory uiFactory)
        {
            _inputService = inputService;
            _gridInputService = gridInputService;
            _uiFactory = uiFactory;
        }

        public event Action<CardView, Cell> DroppedOnCell;

        public bool IsDragging { get; private set; }
        public bool IsEnabled { get; private set; }

        public void Enable()
        {
            if (IsEnabled) 
                return;
            
            _inputService.InputUpdateEvent += OnUpdate;
            _inputService.PointerUpEvent += OnGlobalPointerUp;
            
            IsEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
                return;
            
            _inputService.InputUpdateEvent -= OnUpdate;
            _inputService.PointerUpEvent -= OnGlobalPointerUp;
            
            CancelDrag();
            
            IsEnabled = false;
        }

        public void PointerEnter(CardView view)
        {
            if (!IsEnabled || view == null)
                return;

            view.HoverComponent?.Enter();
        }

        public void PointerExit(CardView view)
        {
            if (!IsEnabled || view == null)
                return;

            if (IsDragging && ReferenceEquals(view, _dragCard))
                return;

            view.HoverComponent?.Exit();
        }

        public void PointerDown(CardView view)
        {
            if (!IsEnabled || view == null)
                return;

            view.HoverComponent?.Enter();

            if (!IsDragging)
                BeginDrag(view);
        }

        public void PointerUp(CardView view)
        {
            if (!IsEnabled || view == null) return;

            view.HoverComponent?.Exit();

            if (IsDragging && ReferenceEquals(view, _dragCard))
                OnGlobalPointerUp();
        }

        private void OnUpdate()
        {
            if (!IsDragging || _dragRT == null)
                return;

            _dragRT.anchoredPosition = ScreenToCanvasLocal(_inputService.TouchPosition);
        }

        private void OnGlobalPointerUp()
        {
            if (!IsDragging)
                return;

            Cell cell = _gridInputService?.HoverCell;
            FinishDrag(cell);
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

            view.HoverComponent?.Enter();
        }
        
        private void CancelDrag()
        {
            if (!IsDragging)
                return;

            FinishDrag(null);
        }

        private void FinishDrag(Cell cellOrNull)
        {
            IsDragging = false;

            if (cellOrNull != null)
            {
                DroppedOnCell?.Invoke(_dragCard, cellOrNull);

                _dragRT.DOKill();
                _dragRT.DOScale(_origScale, 0.08f).SetUpdate(true);

                _dragCard.gameObject.SetActive(false);
            }
            else
            {
                _dragRT.SetParent(_origParent, false);
                _dragRT.SetSiblingIndex(_origSibling);

                _returnTw?.Kill();
                _returnTw = _dragRT.DOAnchorPos(_origAnchoredPos, 0.15f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => { DroppedOnCell?.Invoke(_dragCard, null); })
                    .SetUpdate(true);

                _dragRT.DOScale(_origScale, 0.12f).SetUpdate(true);

                GameHud.CardHolder.AddCard(_dragCard);

                _dragCard.HoverComponent?.Exit();
            }

            _dragCard = null;
            _dragRT = null;
        }

        private Vector2 ScreenToCanvasLocal(Vector2 screenPos)
        {
            var canvas = GameHud.Canvas;
            var canvasRT = (RectTransform)canvas.transform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRT,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out var local);

            return local;
        }

        private GameHud GameHud => _uiFactory.GameHud;
    }
}