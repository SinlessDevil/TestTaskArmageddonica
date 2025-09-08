using Code.UI.Game.CardSelection;

namespace Code.Services.CardSelection
{
    public interface ICardSelectionService
    {
        System.Action Selected { get; set; }
        CardSelectionWindow Open();
        void Close(CardSelectionWindow window);
    }
}

