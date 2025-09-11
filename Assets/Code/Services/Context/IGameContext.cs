using Code.Logic.Grid;
using Code.Logic.Points;
using UnityEngine;
using Grid = Code.Logic.Grid.Grid;

namespace Code.Services.Context
{
    public interface IGameContext
    {
        PlayerGrid PlayerGrid { get; }
        EnemyGird EnemyGird { get; }
        SelectionLookAtPoint SelectionLookAtPoint { get; }
        BattleLookAtPoint BattleLookAtPoint { get; }
        Camera Camera { get; }
        
        void SetPlayerGrid(PlayerGrid grid);
        void SetEnemyGrid(EnemyGird grid);
        void SetLookAtPoint(SelectionLookAtPoint selectionLookAt);
        void SetBattleLookAtPoint(BattleLookAtPoint battleLookAt);
        void SetCamera(Camera camera);
        
        void Cleanup();
    }
}