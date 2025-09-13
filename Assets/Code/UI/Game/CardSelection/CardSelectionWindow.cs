using System.Linq;
using System.Collections.Generic;
using Code.UI.Game.Cards.View;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Code.UI.Game.CardSelection
{
    public class CardSelectionWindow : MonoBehaviour
    {
        [SerializeField] private Transform _cardsRoot;
        [SerializeField] private Button _rerollButton;
        [SerializeField] private Button _toggleVisibilityButton;
        [SerializeField] private Image _visibleImage;
        [SerializeField] private CardSelectionWindowAnimator _animator;
        [SerializeField] private CanvasGroup _rootCanvasGroup;
        [Header("Sprites")]
        [SerializeField] private Sprite _visibleSprite;
        [SerializeField] private Sprite _invisibleSprite;
        [Header("Layout Settings")]
        [SerializeField] private float _cardSpacing = 200f;
        [SerializeField] private float _verticalOffset = 0f;
        [SerializeField] private float _cardWidth = 100f;
        
        private List<CardView> _cardViews = new();
        private bool _visible = true;
        
        private ICardSelectionPM _cardSelectionPM;

        public void Initialize(ICardSelectionPM cardSelectionPm)
        {
            _cardSelectionPM = cardSelectionPm;
            
            Subscribe();
            SetCards(_cardSelectionPM.GetCards());
            
            _animator.Initialize();
            _animator.ShowCanvas();
            
            if (_cardSelectionPM.HasFirstOpenWindow())
            {
                _animator.ShowButton();
            }
            else
            {
                _rerollButton.gameObject.SetActive(false);
                _toggleVisibilityButton.gameObject.SetActive(false);
                
                AutoSelectAllCards().Forget();
            }
        }

        public void Dispose()
        {
            Unsubscribe();
            
            Destroy(this.gameObject);
        }

        private void Subscribe()
        {
            _cardSelectionPM.RolledCardsEvent += OnChangedCards;
            _cardSelectionPM.SellectedCardViewEvent += OnSelect;
            
            _rerollButton.onClick.AddListener(OnReRollButtonClick);
            _toggleVisibilityButton.onClick.AddListener(OnToggleVisibilityButtonClick);
        }
        
        private void Unsubscribe()
        {
            _cardSelectionPM.RolledCardsEvent -= OnChangedCards;
            _cardSelectionPM.SellectedCardViewEvent -= OnSelect;
            
            _rerollButton.onClick.RemoveListener(OnReRollButtonClick);
            _toggleVisibilityButton.onClick.RemoveListener(OnToggleVisibilityButtonClick);
        }

        private void OnChangedCards()
        {
            _cardViews.Clear();
            
            SetCards(_cardSelectionPM.GetCards());
        }
        
        private void OnReRollButtonClick()
        {
            _cardSelectionPM.OnRollCards();
        }

        private void OnToggleVisibilityButtonClick()
        {
            _visible = !_visible;

            if (_visible)
            {
                _visibleImage.sprite = _visibleSprite;
                _animator.ShowCanvas();
            }
            else
            {
                _visibleImage.sprite = _invisibleSprite;
                _animator.HideCanvas();
            }
        }

        private void OnSelect(CardView view)
        {
            OnClosed(view);
            
            _animator.HideCanvas(() => _cardSelectionPM.OnCloseWindow());
            _animator.HideButton();
        }
        
        private void OnClosed(CardView selected)
        {
            _cardSelectionPM.OnAddCardToHolder(selected);
        }
        
        private void SetCards(IReadOnlyList<CardView> cards)
        {
            foreach (CardView cardView in cards)
            {
                if (cardView == null) 
                    continue;
                
                cardView.transform.SetParent(_cardsRoot, false);
            }
            _cardViews = cards.ToList();
            LayoutCards(cards);
        }
        
        private async UniTask AutoSelectAllCards()
        {
            _rootCanvasGroup.blocksRaycasts = false;
            
            await UniTask.Delay(1500);
            
            _rootCanvasGroup.blocksRaycasts = true;

            foreach (CardView cardView in _cardViews.Where(cardView => cardView != null))
            {
                _cardSelectionPM.OnAddCardToHolder(cardView);
                cardView.HoverComponent.ResetState();
                cardView.HoverComponent.HoverExit();
            }

            _animator.HideCanvas(() => _cardSelectionPM.OnCloseWindow());
        }

        private void LayoutCards(IReadOnlyList<CardView> cards)
        {
            RectTransform root = _cardsRoot as RectTransform;
            if (root == null)
                return;

            int count = cards.Count(c => c != null);
            if (count == 0)
                return;

            Vector2[] positions = Cards.Extensions.CardLayoutExtensions.CalculateCenteredRowPositions(
                root, count, _cardWidth, _cardSpacing, _verticalOffset);

            int i = 0;
            foreach (CardView card in cards)
            {
                if (card == null) 
                    continue;
                
                RectTransform cardRoot = card.Root;
                if (!cardRoot) 
                    continue;
                
                cardRoot.anchoredPosition = positions[i];
                cardRoot.localScale = Vector3.one;
                i++;
            }
        }
    }
}