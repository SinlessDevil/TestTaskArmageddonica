using UnityEngine;

namespace Code.Logic.Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private CellVisualController _visualController;
        [Header("Layout Settings")]
        [SerializeField] private float _objectSize = 1f;
        [SerializeField] private float _spacing = 0.2f;
        [SerializeField] private int _maxColumns = 3;
        [SerializeField] private bool _useCircularLayout = false;
        [SerializeField] private float _circularRadius = 0.5f;
        
        private CellInvocationController _invocationController;
        
        public void Initialize()
        {
            _visualController?.Initialize();
            
            Vector3 cellCenter = transform.position;
            
            _invocationController = new CellInvocationController();
            _invocationController.Initialize(cellCenter, _objectSize, _spacing, _maxColumns);
            _invocationController.SetLayoutMode(_useCircularLayout, _circularRadius);
        }

        public CellInvocationController CellInvocationController => _invocationController;

        public CellVisualController VisualController  => _visualController;
    }
}