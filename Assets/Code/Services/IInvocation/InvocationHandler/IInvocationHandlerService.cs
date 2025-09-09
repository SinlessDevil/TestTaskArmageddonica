using Code.Logic.Grid;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.View;

namespace Code.Services.IInvocation.InvocationHandle
{
    public interface IInvocationHandlerService
    {
        void Initialize();
        void Cleanup();
        void SpawnInvocation(CardView cardView, Cell targetCell);
    }
}