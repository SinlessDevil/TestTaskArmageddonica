using System;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Card;
using Code.Services.Input.Card.Select;
using Code.Services.Providers.CardComposites;
using Code.Services.Window;
using Code.UI.Game.Cards.View;
using Code.UI.Game.CardSelection;
using Code.Window;

namespace Code.Services.CardSelection
{
    public class CardSelectionWindowService : ICardSelectionService
    {
        private readonly IWindowService _windowService;
        private readonly IUIFactory _uiFactory;
        private readonly ICardCompositeProvider _cardCompositeProvider;
        private readonly ISelectionCardInputService _selectionCardInputService;

        private ICardSelectionPM _cardSelectionPM;
        private CardSelectionWindow _cardSelectionWindow;
        
        public CardSelectionWindowService(
            IWindowService windowService,
            IUIFactory uiFactory,
            ICardCompositeProvider cardCompositeProvider,
            ISelectionCardInputService selectionCardInputService)
        {
            _windowService = windowService;
            _uiFactory = uiFactory;
            _cardCompositeProvider = cardCompositeProvider;
            _selectionCardInputService = selectionCardInputService;
        }

        public event Action<CardView> SelectedCardEvent;
        
        public event Action ClosedWindowEvent;

        public CardSelectionWindow CardSelectionWindow => _cardSelectionWindow;
        
        public void Open()
        {
            _cardSelectionPM = new CardSelectionPM(_cardCompositeProvider, _uiFactory, _selectionCardInputService);
            _cardSelectionPM.Subscribe();
            _cardSelectionPM.SellectedCardViewEvent += OnSelectCard;
            _cardSelectionPM.ClosedWindowEvent += OnCloseWindow;
            
            _cardSelectionWindow = _windowService
                .Open(WindowTypeId.CardSelection)
                .GetComponent<CardSelectionWindow>();
            _cardSelectionWindow.Initialize(_cardSelectionPM);
        }

        public void Close()
        {
            _cardSelectionPM.SellectedCardViewEvent -= OnSelectCard;
            _cardSelectionPM.ClosedWindowEvent -= OnCloseWindow;
            
            _cardSelectionPM.Unsubscribe();
            _cardSelectionPM.Dispose();
            _cardSelectionPM = null;

            _cardSelectionWindow.Dispose();
            _cardSelectionWindow = null;
        }
        
        private void OnSelectCard(CardView cardView)
        {
            SelectedCardEvent?.Invoke(cardView);
        }
        
        private void OnCloseWindow()
        {
            ClosedWindowEvent?.Invoke();
        }
    }
}

