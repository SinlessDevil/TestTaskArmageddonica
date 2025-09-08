using Code.Logic.Points;
using Services.Contex;
using UnityEngine;
using Grid = Code.Logic.Grid.Grid;

namespace Code.Services.Context
{
    public class GameContext: IGameContext
    {
        public Grid Grid { get; private set; }
        public SelectionLookAtPoint SelectionLookAtPoint { get; private set; }
        public BattleLookAtPoint BattleLookAtPoint { get; private set; }
        public Camera Camera { get; private set; }

        public void SetGrid(Grid grid) => Grid = grid;

        public void SetLookAtPoint(SelectionLookAtPoint selectionLookAt) => SelectionLookAtPoint = selectionLookAt;

        public void SetBattleLookAtPoint(BattleLookAtPoint battleLookAt) => BattleLookAtPoint = battleLookAt;

        public void SetCamera(Camera camera) => Camera = camera;
        
        public void Cleanup()
        {
            Grid = null;
            SelectionLookAtPoint = null;
            BattleLookAtPoint = null;
            Camera = null;
        }
    }
}
