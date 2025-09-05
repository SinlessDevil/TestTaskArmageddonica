using Code.Logic.Grid;
using UnityEngine;

namespace Code.Services.Factories.Grid
{
    public interface IGridFactory
    {
        Cell CreateCell(Vector3 position, Quaternion rotation, Transform parent = null);
    }
}