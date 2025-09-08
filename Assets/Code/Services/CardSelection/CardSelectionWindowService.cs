using Code.Services.Factories.UIFactory;
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

        private ICardSelectionPM _cardSelectionPM;
        private CardSelectionWindow _cardSelectionWindow;
        
        public CardSelectionWindowService(
            IWindowService windowService,
            IUIFactory uiFactory,
            IPoolProvider<CardView> poolProvider)
        {
            _windowService = windowService;
            _uiFactory = uiFactory;
            _poolProvider = poolProvider;
        }

        public CardSelectionWindow Open()
        {
            _cardSelectionPM = new CardSelectionPM(_poolProvider, _uiFactory);
            _cardSelectionWindow = _windowService
                .Open(WindowTypeId.CardSelection)
                .GetComponent<CardSelectionWindow>();
            _cardSelectionWindow.Initialize(_cardSelectionPM);
            return _cardSelectionWindow;
        }

        public void Close(CardSelectionWindow window)
        {
            _cardSelectionPM = null;
            _cardSelectionWindow.Dispose();
        }
    }
}

