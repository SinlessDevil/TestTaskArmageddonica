using Code.Logic.Cell;
using Code.Logic.Grid;
using UnityEngine;
using Zenject;

namespace Code.Services.Factories.Grid
{
    public class GridFactory : Factory, IGridFactory
    {
        public GridFactory(IInstantiator instantiator) : base(instantiator)
        {
            
        }
        
        public Cell CreateCell(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject cellObject = Instantiate(ResourcePath.CellPath);
            cellObject.transform.SetParent(parent);
            cellObject.transform.SetPositionAndRotation(position, rotation);
            Cell cell = cellObject.GetComponent<Cell>();
            return cell;
        }
    }
}