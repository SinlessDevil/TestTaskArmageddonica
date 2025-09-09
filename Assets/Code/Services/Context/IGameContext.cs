using Code.Logic.Points;
using UnityEngine;
using Grid = Code.Logic.Grid.Grid;

namespace Code.Services.Context
{
    public interface IGameContext
    {
        Grid Grid { get; }
        SelectionLookAtPoint SelectionLookAtPoint { get; }
        BattleLookAtPoint BattleLookAtPoint { get; }
        Camera Camera { get; }
        
        void SetGrid(Grid grid);
        void SetLookAtPoint(SelectionLookAtPoint selectionLookAt);
        void SetBattleLookAtPoint(BattleLookAtPoint battleLookAt);
        void SetCamera(Camera camera);
        
        void Cleanup();
    }
}