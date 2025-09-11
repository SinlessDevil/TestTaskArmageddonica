using Code.Logic.Grid;
using Code.Logic.Points;
using UnityEngine;

namespace Code.Services.Context
{
    public class GameContext: IGameContext
    {
        public PlayerGrid PlayerGrid { get; private set; }
        public EnemyGird EnemyGird { get; private set; }
        public SelectionLookAtPoint SelectionLookAtPoint { get; private set; }
        public BattleLookAtPoint BattleLookAtPoint { get; private set; }
        public Camera Camera { get; private set; }

        public void SetPlayerGrid(PlayerGrid grid) => PlayerGrid = grid;
        
        public void SetEnemyGrid(EnemyGird grid) => EnemyGird = grid;

        public void SetLookAtPoint(SelectionLookAtPoint selectionLookAt) => SelectionLookAtPoint = selectionLookAt;

        public void SetBattleLookAtPoint(BattleLookAtPoint battleLookAt) => BattleLookAtPoint = battleLookAt;

        public void SetCamera(Camera camera) => Camera = camera;
        
        public void Cleanup()
        {
            PlayerGrid = null;
            EnemyGird = null;
            
            SelectionLookAtPoint = null;
            BattleLookAtPoint = null;
            
            Camera = null;
        }
    }
}
