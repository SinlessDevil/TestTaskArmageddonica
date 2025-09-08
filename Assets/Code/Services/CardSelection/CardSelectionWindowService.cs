using Code.Services.Factories.UIFactory;
using Code.Services.Input.Card;
using Code.Services.Providers;
using Code.Services.Window;
using Code.UI.Game.Cards;
using Code.UI.Game.CardSelection;
using Code.Window;

namespace Code.Services.CardSelection
{
    public class CardSelectionWindowService : ICardSelectionService
    {
        private readonly IWindowService _windowService;
        private readonly IUIFactory _uiFactory;
        private readonly IPoolProvider<CardView> _poolProvider;
        private readonly ICardInputService _cardInputService;

        private ICardSelectionPM _cardSelectionPM;
        private CardSelectionWindow _cardSelectionWindow;
        public System.Action Selected { get; set; }
        
        public CardSelectionWindowService(
            IWindowService windowService,
            IUIFactory uiFactory,
            IPoolProvider<CardView> poolProvider,
            ICardInputService cardInputService)
        {
            _windowService = windowService;
            _uiFactory = uiFactory;
            _poolProvider = poolProvider;
            _cardInputService = cardInputService;
        }

        public CardSelectionWindow Open()
        {
            _cardSelectionPM = new CardSelectionPM(_poolProvider, _uiFactory, _cardInputService);
            _cardSelectionPM.Subscribe();
            _cardSelectionPM.SellectedCardViewEvent += OnSelected;
            
            _cardSelectionWindow = _windowService
                .Open(WindowTypeId.CardSelection)
                .GetComponent<CardSelectionWindow>();
            _cardSelectionWindow.Initialize(_cardSelectionPM);
            return _cardSelectionWindow;
        }

        public void Close(CardSelectionWindow window)
        {
            _cardSelectionPM.SellectedCardViewEvent -= OnSelected;
            _cardSelectionPM.Unsubscribe();
            _cardSelectionPM = null;

            _cardSelectionWindow.Dispose();
            _cardSelectionWindow = null;
        }

        private void OnSelected(CardView _)
        {
            Selected?.Invoke();
        }
    }
}

