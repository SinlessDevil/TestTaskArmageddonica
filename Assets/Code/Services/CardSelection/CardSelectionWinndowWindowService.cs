using System;
using Code.Services.Factories.UIFactory;
using Code.Services.Input.Card.Select;
using Code.Services.LocalProgress;
using Code.Services.Providers.CardComposites;
using Code.Services.Window;
using Code.UI.Game.Cards.View;
using Code.UI.Game.CardSelection;
using Code.UI.Game.CardSelection.PM;
using Code.UI.Game.CardSelection.View;
using Code.Window;

namespace Code.Services.CardSelection
{
    public class CardSelectionWinndowWindowService : ICardSelectionWinndowService
    {
        private readonly IWindowService _windowService;
        private readonly IUIFactory _uiFactory;
        private readonly ICardCompositeProvider _cardCompositeProvider;
        private readonly ISelectionCardInputService _selectionCardInputService;
        private readonly ILevelLocalProgressService _levelLocalProgressService;

        private ICardSelectionPM _cardSelectionPM;
        private CardSelectionWindow _cardSelectionWindow;
        
        public CardSelectionWinndowWindowService(
            IWindowService windowService,
            IUIFactory uiFactory,
            ICardCompositeProvider cardCompositeProvider,
            ISelectionCardInputService selectionCardInputService,
            ILevelLocalProgressService levelLocalProgressService)
        {
            _windowService = windowService;
            _uiFactory = uiFactory;
            _cardCompositeProvider = cardCompositeProvider;
            _selectionCardInputService = selectionCardInputService;
            _levelLocalProgressService = levelLocalProgressService;
        }

        public event Action<CardView> SelectedCardEvent;
        
        public event Action ClosedWindowEvent;

        public CardSelectionWindow CardSelectionWindow => _cardSelectionWindow;
        
        public void Open()
        {
            _cardSelectionPM = new CardSelectionPM(_cardCompositeProvider, _uiFactory, _selectionCardInputService,
                _levelLocalProgressService);
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
            
            if(!_levelLocalProgressService.HasFirstOpenCardSelection)
                _levelLocalProgressService.SetFirstOpenCardSelection();
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

