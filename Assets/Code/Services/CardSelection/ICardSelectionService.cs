using Code.UI.Game.CardSelection;

namespace Code.Services.CardSelection
{
    public interface ICardSelectionService
    {
        CardSelectionWindow Open();
        void Close(CardSelectionWindow window);
    }
}

